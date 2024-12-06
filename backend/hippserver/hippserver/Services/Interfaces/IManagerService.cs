using hippserver.Models.Domain;

public interface IManagerService
{
    // Get Operations
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);

    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);

    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);

    Task<IEnumerable<Driver>> GetAllDriversAsync();
    Task<Driver?> GetDriverByIdAsync(int id);

    Task<IEnumerable<SalesPerson>> GetAllSalesPeopleAsync();
    Task<SalesPerson?> GetSalesPersonByIdAsync(int id);

    // Create Operations
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<Driver> CreateDriverAsync(Driver driver);
    Task<SalesPerson> CreateSalesPersonAsync(SalesPerson salesPerson);
    Task<Product> CreateProductAsync(Product product);
}
