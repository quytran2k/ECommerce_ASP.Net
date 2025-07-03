using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.DataAccess.Repository;

public class CategoryRepository : Repository<Category> ,ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Category obj)
    {
        _dbContext.Categories.Update(obj);
    }

}