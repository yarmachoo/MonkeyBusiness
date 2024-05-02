using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.Models.Entity;

namespace MonkeyBuisness.DAL.Repositories;
public class PersonRepository : IBaseRepository<PersonEntity>
{
    private readonly AppDbContext _appDbContext;

    public PersonRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task Create(PersonEntity entity)
    {
        await _appDbContext.People.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Delete(PersonEntity entity)
    {
        _appDbContext.People.Remove(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public IQueryable<PersonEntity> GetAll()
    {
        return _appDbContext.People;
    }

    public async Task<PersonEntity> Update(PersonEntity entity)
    {
        _appDbContext.People.Update(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }
}

