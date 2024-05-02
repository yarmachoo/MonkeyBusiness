using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.Models.Entity;

namespace MonkeyBuisness.DAL.Repositories;
public class BookRepository : IBaseRepository<BookEntity>
{
    private readonly AppDbContext _appDbContext;
    public BookRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task Create(BookEntity entity)
    {
        await _appDbContext.Books.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Delete(BookEntity entity)
    {
        _appDbContext.Books.Remove(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public IQueryable<BookEntity> GetAll()
    {
        return _appDbContext.Books;
    }

    public async Task<BookEntity> Update(BookEntity entity)
    {
        _appDbContext.Books.Update(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }
}
