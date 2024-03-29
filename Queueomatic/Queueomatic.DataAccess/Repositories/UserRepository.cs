﻿using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.Repositories.Interfaces;

namespace Queueomatic.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User?> GetAsync(string email)
    {
        var result = await _context.Users.Include(u => u.Rooms).ThenInclude(p => p.Participants).FirstOrDefaultAsync(u => u.Email.Equals(email));
        return result;
    }

    public async Task<User?> GetUserByToken(string token)
    {
        var result = await _context.Users.Include(u => u.Rooms)
            .ThenInclude(p => p.Participants)
            .FirstOrDefaultAsync(u => u.PasswordResetToken.Equals(token));
        return result;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.Include(u => u.Rooms).ToListAsync();
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
    }

    public Task UpdateAsync(User entity)
    { 
        _context.Users.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string id)
    {
        var user = await GetAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }
}