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
        List<ListUserModel> users = await _context.Set<User>().Select(user => new ListUserModel()
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

        if (users.Count == 0)
        {
            throw new Exception("No users registered!");
        }

        return users;
    }
    
    public async Task<ListUserModel> CreateUser(CreateUserModel user)
    {
        var existingUser = await GetUserByEmail(user.Email);

        if (existingUser != null)
        {
            throw new ArgumentException("The email is already registed, try another one!");
        }
        _context.Set<User>().Add(new User()
        {
            Name = user.Name,
            Dateofbirth = user.DateOfBirth,
            Genre = user.Genre,
            Username = user.Username,
            Password = user.Password,
            Email = user.Email,
            RoleId = user.Role_id
        });
        
        await _context.SaveChangesAsync();
        return await GetUserByEmail(user.Email) ?? throw new InvalidOperationException("Impossible to create a new user, try again later!");
    }
    

    public async Task UpdateUser(UpdateUserModel user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.ID);

        if (existingUser == null)
        {
            throw new ArgumentException("User not found!");
        }

        if (existingUser.Email != null && existingUser.Email.Equals(user.Email))
        {
            throw new ArgumentException("The email is already registed, try another one!");
        }

        existingUser.Name = user.Name;
        existingUser.Dateofbirth = user.DateOfBirth;
        existingUser.Genre = user.Genre;
        existingUser.Username = user.Username;
        existingUser.Password = user.Password;
        existingUser.Email = user.Email;
        existingUser.RoleId = user.Role_id;

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUser(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
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