### Steg 1: Skapa en MVC-projekt med Identity

1. **Skapa ett nytt ASP.NET Core MVC-projekt** som inkluderar Identity:
   ```bash
   dotnet new mvc --auth Individual
   ```

### Steg 2: Ny klass: `ApplicationUser`
Skapa en egen klass som ärver från `IdentityUser` om du vill lägga till ytterligare egenskaper till användaren.

1. **Skapa en ny klass `ApplicationUser`** i mappen `Models/`:
   ```csharp
   public class ApplicationUser : IdentityUser
   {
       // Här kan du lägga till ytterligare fält om du vill
   }
   ```

### Steg 2: Anpassa `ApplicationDbContext`
Anpassa den att använda vår ny användarklass. Om du vill göra ändringar i databasen eller hantera fler tabeller, behöver du också anpassa `ApplicationDbContext`.

1. **Skapa en anpassad `ApplicationDbContext`-klass**:
   ```csharp
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
   {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
       {
       }

       // Lägg till dina egna DBSet för andra tabeller här om du har några
   }
   ```

2. **Registrera den anpassade kontexten** i `Program.cs`:
   ```csharp
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlite(connectionString));  // Använder SQLite här
   ```

2. **Registrera `ApplicationUser` i DI-kontainern** i `Program.cs`:
   ```csharp
   builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
       .AddRoles<IdentityRole>()  // Stöd för roller, med strings som nycklar
       .AddEntityFrameworkStores<ApplicationDbContext>();
   ```
### Steg 3: Anpassa vyer och _LoginPartial
Om du har anpassat `ApplicationUser`, se till att uppdatera vyerna så att de speglar dessa ändringar.

1. **Uppdatera `_LoginPartial.cshtml`** i `Views/Shared`:
   ```csharp
   @using Microsoft.AspNetCore.Identity
   @inject SignInManager<ApplicationUser> SignInManager
   @inject UserManager<ApplicationUser> UserManager
   ```

### Steg 4: Lägg till autentisering och auktorisering
Lägg till autentisering och auktorisering i din middleware.

1. **Uppdatera `Program.cs`** för att använda autentisering och auktorisering:
   ```csharp
   app.UseAuthentication(); 
   app.UseAuthorization();  // Lägg till auktorisering, viktigt att det är denna ordning på dessa två
   ```

### Steg 5: Skapa roller och tilldela dem till en användare (demonstration)
Det här steget visar hur du kan skapa roller och tilldela dem till en användare, men i en produktionsapp kanske du vill hantera detta via ett admin-interface eller en seeder-metod.

1. **Skapa roller och tilldela dem till en användare** i `Program.cs` (för demonstrationssyfte):
   ```csharp
   bool JagHarEnAnvändareMedRättEpost = false; // Sätt till true om du har skapat en användare
   string epost = "example@example.com";  // Ersätt med din användares e-postadress

   if (JagHarEnAnvändareMedRättEpost)
   {
       using (var scope = app.Services.CreateScope())
       {
           var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
           string[] roleNames = { "Leader", "Parent", "Scout" }; //strings som nycklar

           foreach (var roleName in roleNames)
           {
               if (!await roleManager.RoleExistsAsync(roleName))
               {
                   await roleManager.CreateAsync(new IdentityRole(roleName));
               }
           }

           var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
           var user = await userManager.FindByEmailAsync(epost);
           await userManager.AddToRoleAsync(user!, "Leader");

           var roles = await userManager.GetRolesAsync(user!);
           Console.WriteLine(string.Join(", ", roles));
       }
   }
   ```

**Notera:** Denna kod är endast för demonstration. I en riktig app kan du hantera detta via ett administrationsgränssnitt eller en seeder-metod som körs vid start.

### Steg 6: Lägg till Authorize-attributet för roller
Nu kan du använda roller för att skydda olika delar av din applikation.

1. **Använd `Authorize`-attributet** i dina controllers, till exempel:
   ```csharp
   [Authorize(Roles = "Leader")]
   public IActionResult Privacy()
   {
       return View();
   }
   ```

### Kör applikationen
Nu är du redo att köra applikationen med roller.
