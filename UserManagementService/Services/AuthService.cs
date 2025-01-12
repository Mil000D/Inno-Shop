using FluentValidation;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using UserManagementService.DAL.Repositories;
using UserManagementService.DTOs;
using UserManagementService.TokenHandlers;

namespace UserManagementService.Services
{
    public class AuthService(IUserRepository userRepository, ITokenHandler tokenHandler,
        IValidator<LoginDTO> loginDTOValidator, IValidator<PasswordResetRequestDTO> passwordResetRequestDTOValidator,
        IValidator<PasswordResetDTO> passwordResetDTOValidator, IValidator<AccountVerificationDTO> accountVerificationDTOValidator) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenHandler _tokenHandler = tokenHandler;
        private readonly IValidator<LoginDTO> _loginDTOValidator = loginDTOValidator;
        private readonly IValidator<PasswordResetRequestDTO> _passwordResetRequestDTOValidator = passwordResetRequestDTOValidator;
        private readonly IValidator<PasswordResetDTO> _passwordResetDTOValidator = passwordResetDTOValidator;
        private readonly IValidator<AccountVerificationDTO> _accountVerificationDTOValidator = accountVerificationDTOValidator;
        public async Task<string> LoginAsync(LoginDTO login)
        {
            await _loginDTOValidator.ValidateAndThrowAsync(login);
            var user = await _userRepository.GetUserByEmailAsync(login.Email);
            if (user == null || !user.IsVerified || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            return _tokenHandler.GenerateJwtToken(user);
        }

        public async Task RequestPasswordResetAsync(PasswordResetRequestDTO request)
        {
            await _passwordResetRequestDTOValidator.ValidateAndThrowAsync(request);
            var user = await _userRepository.GetUserByEmailAsync(request.Email)
                ?? throw new KeyNotFoundException("User not found.");
            var token = _tokenHandler.GenerateGuidBasedToken();

            user.PasswordResetToken = token;
            await _userRepository.SaveChangesAsync();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("no-reply", "no-reply@inno-shop.com"));
            message.To.Add(new MailboxAddress(user.Name, user.Email));
            message.Subject = "Password Reset";
            message.Body = new TextPart("plain") { Text = $"Please reset your password using the following token: {token}" };

            using var client = new SmtpClient();
            client.CheckCertificateRevocation = false;
            await client.ConnectAsync("mailhog", 1025, SecureSocketOptions.Auto);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task ResetPasswordAsync(PasswordResetDTO reset)
        {
            await _passwordResetDTOValidator.ValidateAndThrowAsync(reset);
            var user = await _userRepository.GetUserByPasswordResetTokenAsync(reset.Token)
                ?? throw new KeyNotFoundException("Invalid or expired password reset token.");
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(reset.NewPassword);
            user.PasswordResetToken = null;
            await _userRepository.SaveChangesAsync();
        }

        public async Task VerifyAccountAsync(AccountVerificationDTO verification)
        {
            await _accountVerificationDTOValidator.ValidateAndThrowAsync(verification);
            var user = await _userRepository.GetUserByAccountVerificationTokenAsync(verification.Token)
                ?? throw new KeyNotFoundException("Invalid or expired account verification token.");
            user.IsVerified = true;
            user.AccountVerificationToken = null;
            await _userRepository.SaveChangesAsync();
        }
    }
}