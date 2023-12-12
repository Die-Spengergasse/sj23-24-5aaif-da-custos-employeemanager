using Bogus;
using EmployeeManager.Application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Infrastructure
{
    public class EmployeemanagerContext : DbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Job> Jobs => Set<Job>();
        public EmployeemanagerContext(DbContextOptions<EmployeemanagerContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Generic config for all entities
                // ON DELETE RESTRICT instead of ON DELETE CASCADE
                foreach (var key in entityType.GetForeignKeys())
                    key.DeleteBehavior = DeleteBehavior.Restrict;

                foreach (var prop in entityType.GetDeclaredProperties())
                {
                    // Define Guid as alternate key. The database can create a guid fou you.
                    if (prop.Name == "Guid")
                    {
                        modelBuilder.Entity(entityType.ClrType).HasAlternateKey("Guid");
                        prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                    }
                    // Default MaxLength of string Properties is 255.
                    if (prop.ClrType == typeof(string) && prop.GetMaxLength() is null) prop.SetMaxLength(255);
                    // Seconds with 3 fractional digits.
                    if (prop.ClrType == typeof(DateTime)) prop.SetPrecision(3);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetPrecision(3);
                }
            }
        }

        /// <summary>
        /// Initialize the database with some values (holidays, ...).
        /// Unlike Seed, this method is also called in production.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void Initialize()
        {
        }

        /// <summary>
        /// Generates random values for testing the application. This method is only called in development mode.
        /// </summary>
        private async Task SeedAsync()
        {
            Randomizer.Seed = new Random(1213);
            var faker = new Faker("de");

            var employees = new Faker<Employee>("de").CustomInstantiator(f =>
            {
                var lastname = f.Name.LastName();
                return new Employee(
                    username: lastname.ToLower(),
                    firstname: f.Name.FirstName(), lastname: lastname,
                    birth: f.Date.Between(new DateTime(1990, 1, 1), new DateTime(2005, 1, 1)).Date)
                { Guid = f.Random.Guid() };
            })
            .Generate(10)
            .ToList();
            Employees.AddRange(employees);

            var customers = new Faker<Customer>("de").CustomInstantiator(f =>
            {
                return new Customer(
                    f.Company.CompanyName(), f.Random.Int(1000, 9999).ToString(),
                    f.Address.City(), f.Address.StreetAddress())
                { Guid = f.Random.Guid() };
            })
            .Generate(10)
            .ToList();
            Customers.AddRange(customers);

            var jobs = new Faker<Job>("de").CustomInstantiator(f =>
            {
                var date = f.Date.Between(new DateTime(2023, 1, 1), new DateTime(2024, 6, 1)).Date.AddHours(f.Random.Int(18, 22));
                var employee = date < new DateTime(2024, 1, 1) ? f.Random.ListItem(employees) : null;
                return new Job(date, f.Random.ListItem(customers), employee)
                { Guid = f.Random.Guid() };
            })
            .Generate(20)
            .ToList();
            Jobs.AddRange(jobs);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Creates the database. Called once at application startup.
        /// </summary>
        public async Task CreateDatabaseAsync(bool isDevelopment)
        {
            if (isDevelopment) { Database.EnsureDeleted(); }
            // EnsureCreated only creates the model if the database does not exist or it has no
            // tables. Returns true if the schema was created.  Returns false if there are
            // existing tables in the database. This avoids initializing multiple times.
            if (Database.EnsureCreated()) { Initialize(); }
            if (isDevelopment) await SeedAsync();
        }

    }
}
