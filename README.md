
# Chronos Specification

Chronos Specification is a C# package designed to simplify the process of creating custom repositories by providing basic implementations. This package helps developers to quickly set up and manage their repositories, reducing the time and effort required for this task




## Requirements

 - [.NET SDK](https://awesomeopensource.com/project/elangosundar/awesome-README-templates)
 - [Entity Framework Core](https://github.com/matiassingers/awesome-readme)
 - [.NET 8](https://bulldogjob.com/news/449-how-to-write-a-good-readme-for-your-github-project)


## Authors

- [Souvik Kundu](https://github.com/souvikk27)


## Usage/Examples

Define an interface for your repository that extends the base repository interface: 

```c#
public interface IProductRepository : IRepository<Model.Product>
{
}

```



Extend RepositoryBase class to your custom repository 

```c#
public class ProductRepository : RepositoryBase<Product, ApplicationDbContext>, IProductRepository
{
    public ProductRepository(IRepositoryOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public override Expression<Func<ApplicationDbContext, DbSet<Model.Product>>> DataSet() => o => o.Products;

    public override Expression<Func<Model.Product, object>> Key() => o => o.Id;
}
```


Create a specification by inheriting from Specification<Product> and defining criteria, includes, ordering, and paging:

```c#
public class ActiveProductsSpecification : Specification<Product>
{
    public ActiveProductsSpecification()
        : base(p => p.IsActive) // Criteria: Active products only
    {
        AddInclude(p => p.Category); // Include related category
        AddOrderBy(p => p.Name, OrderingDirection.Ascending); // Order by product name
        ApplyPaging(1, 10); // Apply paging: page 1, 10 items per page
    }
}
```

Apply specifications to retrieve entities that match specific criteria:

```c#
// Create a new instance of the specification
var spec = new ActiveProductsSpecification();

// Use the repository to apply the specification and retrieve matching products
var products = await productRepository.ListAsync(spec);
```




## License

[MIT](https://choosealicense.com/licenses/mit/)

