using hippserver.Models.Entities;
using Microsoft.AspNetCore.Identity;

public class DataSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ILogger<DataSeeder> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Starting data seeding...");

        try
        {
            _logger.LogInformation("Seeding roles...");
            await SeedRoles();

            _logger.LogInformation("Seeding admin user...");
            await SeedAdminUser();

            _logger.LogInformation("Data seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding data");
            throw;
        }
    }

    private async Task SeedRoles()
    {
        var roles = new[] { "Admin", "Menaxher", "Komercialist", "Shofer", "Etiketues" };

        foreach (var roleName in roles)
        {


            if (!await _roleManager.RoleExistsAsync(roleName))
            {

                var role = new ApplicationRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);

            }
        }
    }

    private async Task SeedAdminUser()
    {
        var adminUsername = "HippAdmin";
        var adminPassword = "HippAdmin123!";

       

        try
        {
            var adminUser = await _userManager.FindByNameAsync(adminUsername);

            if (adminUser == null)
            {
               

                adminUser = new ApplicationUser
                {
                    UserName = adminUsername,
                    Email = "hippadmin@hipp.com",
                    EmailConfirmed = true,
                    FirstName = "Hipp",
                    LastName = "Admin"
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {


                    // Make sure the Admin role exists before assigning
                    if (await _roleManager.RoleExistsAsync("Admin"))
                    {
                        result = await _userManager.AddToRoleAsync(adminUser, "Admin");


                    }
                }
            }
            else
            {
                

                // Check if user is in Admin role
                if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                   
                    var result = await _userManager.AddToRoleAsync(adminUser, "Admin");

                    
                }
                
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding admin user");
            throw;
        }
    }
}