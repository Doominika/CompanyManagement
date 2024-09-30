using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection.Emit;
using System.Threading.Tasks;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Database
{
    public class MyDbConnection : DbContext
    {
       
        public MyDbConnection() : base("name=MyDbContext")
        {
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Installation> Installations { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Installations)
                .WithMany(i => i.Employees)
                .Map(m =>
                {
                    m.ToTable("EmployeeInstallations"); // Nazwa tabeli pośredniczącej
                    m.MapLeftKey("Employee_id"); // Klucz obcy dla tabeli Employee
                    m.MapRightKey("Installation_id"); // Klucz obcy dla tabeli Installation
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}






















/*
 [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MyDbConnection : System.Data.Entity.DbContext
    {
        
        public MyDbConnection() : base("name=MyDbContext")
        {
        }
        

        //public DbSet<Person> People { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Installation> Installations { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }

        
    }
 */


