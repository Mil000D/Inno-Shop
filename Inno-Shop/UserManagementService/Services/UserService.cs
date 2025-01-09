using UserManagementService.DAL.Repositories;
using UserManagementService.DTOs;
using UserManagementService.Models;
using System.Security.Claims;
using EventBus;
using MassTransit;
using FluentValidation;

namespace UserManagementService.Services
{
    public class UserService(IUserRepository userRepository, IPublishEndpoint publishEndpoint, IValidator<UserDTO> validator) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IValidator<UserDTO> _validator = validator;

        public async Task<IEnumerable<User>> GetUsersAsync(ClaimsPrincipal user)
        {
            if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return await _userRepository.GetUsersAsync(userId);
            }
            throw new UnauthorizedAccessException("User was not found.");
        }

        public async Task<User?> GetUserAsync(int id, ClaimsPrincipal user)
        {
            if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId) && id != userId)
            {
                return await _userRepository.GetUserByIdAsync(id, userId);
            }
            throw new UnauthorizedAccessException("User was not found.");
        }

        public async Task<User> CreateUserAsync(UserDTO register)
        {
            await _validator.ValidateAndThrowAsync(register);

            if (await _userRepository.GetUserByEmailAsync(register.Email) != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            var user = new User
            {
                Name = register.Name,
                Email = register.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = register.Role,
                IsActive = true
            };

            await _userRepository.AddUserAsync(user);
            return user;
        }

        public async Task UpdateUserAsync(int id, UserDTO user, ClaimsPrincipal userPrincipal)
        {
            await _validator.ValidateAndThrowAsync(user);

            if (int.TryParse(userPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId) && id != userId)
            {
                var userToUpdate = await _userRepository.GetActivatedUserByIdAsync(id, userId) 
                    ?? throw new KeyNotFoundException("There is no such user. Try again!");
                userToUpdate.Name = user.Name;
                userToUpdate.Email = user.Email;
                userToUpdate.Role = user.Role;
                userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

                await _userRepository.UpdateUserAsync(userToUpdate);
            }
            else
            {
                throw new UnauthorizedAccessException("User was not found.");
            }
        }

        public async Task DeleteUserAsync(int id, ClaimsPrincipal user)
        {
            if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId) && id != userId)
            {
                var userToDelete = await _userRepository.GetUserByIdAsync(id, userId)
                    ?? throw new KeyNotFoundException("There is no such user. Try again!");
                await _userRepository.DeleteUserAsync(userToDelete);
                await _publishEndpoint.Publish<UserDeleted>(new { Id = id });
            }
            else
            {
                throw new UnauthorizedAccessException("User was not found.");
            }
        }

        public async Task DeactivateUserAsync(int id, ClaimsPrincipal user)
        {
            if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId) && id != userId)
            {
                var userToDeactivate = await _userRepository.GetActivatedUserByIdAsync(id, userId) 
                    ?? throw new KeyNotFoundException("There is no such user. Try again!");
                userToDeactivate.IsActive = false;
                await _userRepository.UpdateUserAsync(userToDeactivate);
                await _publishEndpoint.Publish<UserDeactivated>(new { Id = id });
            }
            else
            {
                throw new UnauthorizedAccessException("User was not found.");
            }
        }

        public async Task ActivateUserAsync(int id, ClaimsPrincipal user)
        {
            if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId) && id != userId)
            {
                var userToActivate = await _userRepository.GetDeactivatedUserByIdAsync(id, userId) 
                    ?? throw new KeyNotFoundException("There is no such user. Try again!");
                userToActivate.IsActive = true;
                await _userRepository.UpdateUserAsync(userToActivate);
                await _publishEndpoint.Publish<UserActivated>(new { Id = id });
            }
            else
            {
                throw new UnauthorizedAccessException("User was not found.");
            }
        }
    }
}
