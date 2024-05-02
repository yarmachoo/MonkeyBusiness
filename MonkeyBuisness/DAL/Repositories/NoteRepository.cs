using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.Models.Entity;

namespace MonkeyBuisness.DAL.Repositories;
public class NoteRepository:IBaseRepository<NoteEntity>
{
    private readonly AppDbContext _appDbContext;

    public NoteRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task Create(NoteEntity entity)
    {
        await _appDbContext.Notes.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Delete(NoteEntity entity)
    {
        _appDbContext.Notes.Remove(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public IQueryable<NoteEntity> GetAll()
    {
        return _appDbContext.Notes;
    }

    public async Task<NoteEntity> Update(NoteEntity entity)
    {
        _appDbContext.Notes.Update(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }
}
