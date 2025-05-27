using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class AuthRepository(DataContext context) : BaseRepository<UserEntity>(context), IAuthRepository
{
    public override async Task<RepositoryResult<IEnumerable<UserEntity>>> GetAllAsync()
    {
        try
        {
            var users = await _table.ToListAsync();

            return new RepositoryResult<IEnumerable<UserEntity>>
            {
                Success = true,
                Result = users
            };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<IEnumerable<UserEntity>>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

}

