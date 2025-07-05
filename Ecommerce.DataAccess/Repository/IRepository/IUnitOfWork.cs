namespace Ecommerce.DataAccess.Repository.IRepository;
// This will cover all of repository
public interface IUnitOfWork
{
    ICategoryRepository Category {get;}
    IProductRepository Product {get;}
    void Save();
}