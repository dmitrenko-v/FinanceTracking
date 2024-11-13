namespace Domain.Repositories;

public interface IRepository<in TEntity>
{
    public void Add(TEntity entity);
    
    public void Delete(TEntity entity);
    
    public void Update(TEntity entity);
}