using hippserver.Infrastructure.Repositories.Interfaces;
using hippserver.Models.Domain;

public class ManagerService : IManagerService
{
    private readonly IBaseRepository<Employee> _employeeRepository;
    private readonly IBaseRepository<Product> _productRepository;
    private readonly IBaseRepository<Order> _orderRepository;
    private readonly IBaseRepository<Driver> _driverRepository;
    private readonly IBaseRepository<SalesPerson> _salesPersonRepository;

    public ManagerService(
        IBaseRepository<Employee> employeeRepository,
        IBaseRepository<Product> productRepository,
        IBaseRepository<Order> orderRepository,
        IBaseRepository<Driver> driverRepository,
        IBaseRepository<SalesPerson> salesPersonRepository)
    {
        _employeeRepository = employeeRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _driverRepository = driverRepository;
        _salesPersonRepository = salesPersonRepository;
    }

    // Get Methods
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync() => await _employeeRepository.GetAllAsync();
    public async Task<Employee?> GetEmployeeByIdAsync(int id) => await _employeeRepository.GetByIdAsync(id.ToString());

    public async Task<IEnumerable<Product>> GetAllProductsAsync() => await _productRepository.GetAllAsync();
    public async Task<Product?> GetProductByIdAsync(int id) => await _productRepository.GetByIdAsync(id.ToString());

    public async Task<IEnumerable<Order>> GetAllOrdersAsync() => await _orderRepository.GetAllAsync();
    public async Task<Order?> GetOrderByIdAsync(int id) => await _orderRepository.GetByIdAsync(id.ToString());

    public async Task<IEnumerable<Driver>> GetAllDriversAsync() => await _driverRepository.GetAllAsync();
    public async Task<Driver?> GetDriverByIdAsync(int id) => await _driverRepository.GetByIdAsync(id.ToString());

    public async Task<IEnumerable<SalesPerson>> GetAllSalesPeopleAsync() => await _salesPersonRepository.GetAllAsync();
    public async Task<SalesPerson?> GetSalesPersonByIdAsync(int id) => await _salesPersonRepository.GetByIdAsync(id.ToString());

    // Create Methods
    public async Task<Employee> CreateEmployeeAsync(Employee employee) => await _employeeRepository.AddAsync(employee);
    public async Task<Driver> CreateDriverAsync(Driver driver) => await _driverRepository.AddAsync(driver);
    public async Task<SalesPerson> CreateSalesPersonAsync(SalesPerson salesPerson) => await _salesPersonRepository.AddAsync(salesPerson);
    public async Task<Product> CreateProductAsync(Product product) => await _productRepository.AddAsync(product);
}
