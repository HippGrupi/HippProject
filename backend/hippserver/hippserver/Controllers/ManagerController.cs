using hippserver.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "Manager")]
public class ManagerController : ControllerBase
{
    private readonly IManagerService _managerService;

    public ManagerController(IManagerService managerService)
    {
        _managerService = managerService;
    }

    // Get Endpoints
    [HttpGet("employees")]
    public async Task<IActionResult> GetAllEmployees() => Ok(await _managerService.GetAllEmployeesAsync());

    [HttpGet("employees/{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await _managerService.GetEmployeeByIdAsync(id);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetAllProducts() => Ok(await _managerService.GetAllProductsAsync());

    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _managerService.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetAllOrders() => Ok(await _managerService.GetAllOrdersAsync());

    [HttpGet("orders/{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _managerService.GetOrderByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpGet("drivers")]
    public async Task<IActionResult> GetAllDrivers() => Ok(await _managerService.GetAllDriversAsync());

    [HttpGet("drivers/{id}")]
    public async Task<IActionResult> GetDriverById(int id)
    {
        var driver = await _managerService.GetDriverByIdAsync(id);
        if (driver == null) return NotFound();
        return Ok(driver);
    }

    [HttpGet("salespeople")]
    public async Task<IActionResult> GetAllSalesPeople() => Ok(await _managerService.GetAllSalesPeopleAsync());

    [HttpGet("salespeople/{id}")]
    public async Task<IActionResult> GetSalesPersonById(int id)
    {
        var salesPerson = await _managerService.GetSalesPersonByIdAsync(id);
        if (salesPerson == null) return NotFound();
        return Ok(salesPerson);
    }

    // Create Endpoints
    [HttpPost("employees")]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
    {
        var createdEmployee = await _managerService.CreateEmployeeAsync(employee);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
    }

    [HttpPost("drivers")]
    public async Task<IActionResult> CreateDriver([FromBody] Driver driver)
    {
        var createdDriver = await _managerService.CreateDriverAsync(driver);
        return CreatedAtAction(nameof(GetDriverById), new { id = createdDriver.Id }, createdDriver);
    }

    [HttpPost("salespeople")]
    public async Task<IActionResult> CreateSalesPerson([FromBody] SalesPerson salesPerson)
    {
        var createdSalesPerson = await _managerService.CreateSalesPersonAsync(salesPerson);
        return CreatedAtAction(nameof(GetSalesPersonById), new { id = createdSalesPerson.Id }, createdSalesPerson);
    }

    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        var createdProduct = await _managerService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }
}