using Ecommerce.Models;

namespace Ecommerce.DataAccess.Repository.IRepository;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category obj);
}