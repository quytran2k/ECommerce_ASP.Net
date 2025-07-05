using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Product product)
    {
        var productInDb = _dbContext.Products.FirstOrDefault(p=> p.Id == product.Id);
        if (productInDb != null)
        {
            productInDb.Id = product.Id;
            productInDb.ISBN = product.ISBN;
            productInDb.Title = product.Title;
            productInDb.Description = product.Description;
            productInDb.Price = product.Price;
            productInDb.ListPrice = product.ListPrice;
            productInDb.Price50 = product.Price50;
            productInDb.Price100 = product.Price100;
            productInDb.Category =  product.Category;
            productInDb.Author = product.Author;
            if (productInDb.ImageUrl != null)
            {
                productInDb.ImageUrl = product.ImageUrl;
            }
        }
    }
}