using BusinessLogic.Models.User;
using BusinessLogic.Models.Authentication;
using BusinessLogic.Models.Role;

namespace FitJourney_BackEnd.Interface;

public interface IUserRepository
{
    Task<List<ListUserModel>> GetUsers();
    Task<ListUserModel> CreateUser(CreateUserModel user);
    Task UpdateUser(UpdateUserModel user);
    Task DeleteUser(int id);
    Task<ListUserModel?> GetUserByEmail(string email);
    Task<List<RoleModel>> GetRole();
}