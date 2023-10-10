using System;
using System.Collections.Generic;
using UsersManipulation.Business.DTOs;
using UsersManipulation.Business.DTOs.UserDtos;
using UsersManipulation.Data.Entity;

namespace UsersManipulation.Business.Services.Contracts
{
    public interface IUserService
    {
        Task<RegisterSuccessDto> RegisterUserAsync(RegisterUserDto model);
        Task<LoginSuccessDto> LoginUserAsync(LoginUserDto model);
        void BlockUser(int userId);
        void UnblockUser(int userId);
        void DeleteUser(int userId);
        bool CanUserLogin(int userId);
        IEnumerable<UserEntity> GetAllUsers();
    }
}
