namespace Ecommerce.DataAccess.Repository.IRepository;
// This will cover all of repository
public interface IUnitOfWork
{
    ICategoryRepository Category {get;}
    IProductRepository Product {get;}
    ICompanyRepository Company {get; }
    IShoppingCartRepository ShoppingCart {get; }
    IApplicationUserRepository ApplicationUser {get; }
    void Save();
}