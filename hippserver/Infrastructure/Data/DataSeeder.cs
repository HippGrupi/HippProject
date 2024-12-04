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
            _logger.LogInformation("Checking role: {RoleName}", roleName);

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                _logger.LogInformation("Creating role: {RoleName}", roleName);
                var role = new ApplicationRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                    _logger.LogInformation("Successfully created role: {RoleName}", roleName);
                else
                    _logger.LogError("Failed to create role: {RoleName}. Errors: {Errors}",
                        roleName,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            else
            {
                _logger.LogInformation("Role already exists: {RoleName}", roleName);
            }
        }
    }

    private async Task SeedAdminUser()
    {
        var adminUsername = "HippAdmin";
        var adminPassword = "HippAdmin123!";

        _logger.LogInformation("Starting admin user creation process...");

        try
        {
            var adminUser = await _userManager.FindByNameAsync(adminUsername);

            if (adminUser == null)
            {
                _logger.LogInformation("Admin user does not exist. Creating new admin user...");

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
                    _logger.LogInformation("Admin user created successfully. Adding to Admin role...");

                    // Make sure the Admin role exists before assigning
                    if (await _roleManager.RoleExistsAsync("Admin"))
                    {
                        result = await _userManager.AddToRoleAsync(adminUser, "Admin");

                        if (result.Succeeded)
                        {
                            _logger.LogInformation("Admin role assigned successfully");
                        }
                        else
                        {
                            _logger.LogError("Failed to assign Admin role. Errors: {Errors}",
                                string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }
                    else
                    {
                        _logger.LogError("Admin role does not exist. Cannot assign role to user.");
                    }
                }
                else
                {
                    _logger.LogError("Failed to create admin user. Errors: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                _logger.LogInformation("Admin user already exists. Checking role assignment...");

                // Check if user is in Admin role
                if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    _logger.LogInformation("Adding existing admin user to Admin role...");
                    var result = await _userManager.AddToRoleAsync(adminUser, "Admin");

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Admin role assigned successfully to existing user");
                    }
                    else
                    {
                        _logger.LogError("Failed to assign Admin role to existing user. Errors: {Errors}",
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    _logger.LogInformation("Admin user already has Admin role assigned");
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