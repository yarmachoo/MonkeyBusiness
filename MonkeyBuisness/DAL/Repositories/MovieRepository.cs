using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.Models.Entity;

namespace MonkeyBuisness.DAL.Repositories;
public class MovieRepository : IBaseRepository<MovieEntity>
{
    private readonly AppDbContext _appDbContext;
    public MovieRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task Create(MovieEntity entity)
    {
        await _appDbContext.Movies.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Delete(MovieEntity entity)
    {
        _appDbContext.Movies.Remove(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public IQueryable<MovieEntity> GetAll()
    {
        return _appDbContext.Movies;
    }

    public async Task<MovieEntity> Update(MovieEntity entity)
    {
        _appDbContext.Movies.Update(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }
}
