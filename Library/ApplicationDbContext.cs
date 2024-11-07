using Library.Data;
using Library.Model;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KCKDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"); // Dla SQL Server
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel
                {
                    Login = "admin",
                    Password = "admin",
                    Role = RolesEnum.PermissionAdmin
                },
                new UserModel
                {
                    Login = "user",
                    Password = "user",
                    Role = RolesEnum.PermissionBuyer
                }
            );


            modelBuilder.Entity<UserModel>().HasKey(u => u.Login);
            modelBuilder.Entity<ProductModel>().HasKey(p => p.Id);
        }
    }
}
