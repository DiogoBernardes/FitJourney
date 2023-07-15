using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models.Role;
using BusinessLogic.Models.User;
using FitJourney_BackEnd.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitJourney_BackEnd.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FitJourneyDbContext _context;

    public UserRepository(FitJourneyDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ListUserModel>> GetUsers()
    {
        return await _context.Set<User>().Select(user => new ListUserModel()
        {
            ID = user.UserId,
            Name = user.Name,
            DateOfBirth = user.Dateofbirth,
            Genre = user.Genre,
            Email = user.Email,
            Username = user.Username,
            Password = user.Password,
            Role = new RoleModel()
            {
                ID = user.Role.RoleId,
                Name = user.Role.RoleName
            }
        }).ToListAsync();
    }
    
    
    public async Task<ListUserModel?> GetUserByEmail(string email)
    {
        return _context.Set<User>().Select(user => new ListUserModel()
        {
            ID = user.UserId,
            Name = user.Name,
            DateOfBirth = user.Dateofbirth,
            Genre = user.Genre,
            Email = user.Email,
            Username = user.Username,
            Password = user.Password,
            Role = new RoleModel()
            {
                ID = user.Role.RoleId,
                Name = user.Role.RoleName
            }
        }).FirstOrDefault(u => u.Email.Equals(email));
    }
    
    
    public async Task<List<RoleModel>> GetRole()
    {
        return await _context.Set<Role>().Select(role => new RoleModel()
        {
            ID = role.RoleId,
            Name = role.RoleName
        }).ToListAsync();
    }
}