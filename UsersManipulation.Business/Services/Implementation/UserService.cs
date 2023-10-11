using UsersManipulation.Business.DTOs;
using UsersManipulation.Business.DTOs.UserDtos;
using UsersManipulation.Business.Exceptions;
using UsersManipulation.Business.Services.Common;
using UsersManipulation.Business.Services.Contracts;
using UsersManipulation.Data.Entity;
using UsersManipulation.Data.Repositories.Contracts;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<RegisterSuccessDto> RegisterUserAsync(RegisterUserDto model)
    {
        var oldUser = await _userRepository.FindByEmailAsync(model.Email);

        if(oldUser != null)
        {
            throw new NotSucceededException("Register failed. User already exist");
        }

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model), "Register Model is null");
        }

        if (model.Password == model.RepeatPassword)
        {
            var user = new UserEntity
            {
                Email = model.Email,
                Name = model.Name,
                RegistrationDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow,
                IsBlocked = false
            };

            var passwordHash = HashPassword(model.Password);
            user.Password = passwordHash;

            var result = await _userRepository.CreateAsync(user);

            if (result == null)
            {
                throw new NotSucceededException("Register failed");
            }

            return new RegisterSuccessDto
            {
                Email = user.Email
            };
        }
        else
        {
            throw new NotSucceededException("The password does not match. Please try again.");
        }
    }

    private string HashPassword(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);

        return passwordHash;
    }

    public async Task<LoginSuccessDto> LoginUserAsync(LoginUserDto model)
    {
        var user = await _userRepository.FindByEmailAsync(model.Email);

        if (user == null)
        {
            throw new NotFoundException("No such account exists. Please check the entered data.");
        }

        user.LastLoginDate = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        var canLogin = CanUserLogin(user.Id);

        if(!canLogin)
        {
            throw new NotSucceededException("Your account is temporarily blocked. If you believe this was an error, please contact your administrator.");
        }

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model), "LoginUserDto Model is null");
        }

        var result = CheckPassword(user, model.Password);

        if (!result)
        {
            throw new LoginException("Invalid password. Try again.");
        }

        return new LoginSuccessDto
        {
            Id = user.Id.ToString(),
            Token = _tokenService.GenerateAccessToken(await _tokenService.GetClaimsAsync(user.Email))
        };
    }

    public bool CheckPassword(UserEntity user, string password)
    {
        if (user == null || string.IsNullOrEmpty(password))
        {
            return false;
        }

        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    public void BlockUser(int userId)
    {
        var user = _userRepository.GetByIdAsync(userId).Result;
        if (user != null)
        {
            user.IsBlocked = true;
            _userRepository.UpdateAsync(user).Wait();
        }
        else throw new NotFoundException(nameof(userId));
    }

    public void UnblockUser(int userId)
    {
        var user = _userRepository.GetByIdAsync(userId).Result;
        if (user != null)
        {
            user.IsBlocked = false;
            _userRepository.UpdateAsync(user).Wait();
        }
        else throw new NotFoundException(nameof(userId));
    }

    public void DeleteUser(int userId)
    {
        var user = _userRepository.GetByIdAsync(userId).Result;
        if (user != null)
        {
            _userRepository.DeleteAsync(user).Wait();
        }
        else throw new NotFoundException(nameof(userId));
    }

    public bool CanUserLogin(int userId)
    {
        var user = _userRepository.GetByIdAsync(userId).Result;
        return user != null && !user.IsBlocked;
    }

    public IEnumerable<UserEntity> GetAllUsers()
    {
        return _userRepository.GetAllAsync().Result;
    }

    public string GetUserName(int userId)
    {
        var user = _userRepository.GetByIdAsync(userId).Result;

        if (user != null)
        {
            return user.Name;
        }
        else
        {
            throw new NotFoundException(nameof(userId));
        }
    }
}
