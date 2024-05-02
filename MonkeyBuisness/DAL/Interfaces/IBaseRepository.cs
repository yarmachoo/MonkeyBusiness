namespace MonkeyBuisness.DAL.Interfaces;
public interface IBaseRepository<T>
{
    Task Create(T entity);
    Task Delete(T entity);
    //select
    IQueryable<T> GetAll();
    Task<T> Update(T entity);
}
