using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.DTOs;
using UserManagementService.Services;

namespace UserManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
        {
            try
            {
                var token = await _authService.LoginAsync(login);
                return Ok(new { token });
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordResetAsync([FromBody] PasswordResetRequestDTO request)
        {
            try
            {
                await _authService.RequestPasswordResetAsync(request);
                return Ok("Password reset email sent.");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Error sending email: " + ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] PasswordResetDTO reset)
        {
            try
            {
                await _authService.ResetPasswordAsync(reset);
                return Ok("Password has been reset. Now you can log in.");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("verify-account")]
        public async Task<IActionResult> VerifyAccountAsync([FromBody] AccountVerificationDTO verification)
        {
            try
            {
                await _authService.VerifyAccountAsync(verification);
                return Ok("Account has been verified. Now you can log in.");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}