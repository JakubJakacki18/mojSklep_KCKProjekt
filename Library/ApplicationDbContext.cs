using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<UserModel> Users { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<ShoppingCartHistoryModel> ShoppingCartHistories { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KCKDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"); // Dla SQL Server
			optionsBuilder.EnableSensitiveDataLogging();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasSequence<int>("ProductIds", schema: "shared")
				.StartsAt(10000000)
				.IncrementsBy(1);

			modelBuilder.Entity<ProductModel>()
				.Property(p => p.Id)
				.HasDefaultValueSql("NEXT VALUE FOR shared.ProductIds");


			modelBuilder.Entity<CartProductModel>()
				.HasOne(cp => cp.OriginalProduct)
				.WithMany(p => p.CartProducts)
				.HasForeignKey(cp => cp.ProductId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<CartProductModel>()
				.HasOne(cp => cp.User)  
				.WithMany(u => u.ProductsInCart) 
				.HasForeignKey(cp => cp.UserId)  
				.OnDelete(DeleteBehavior.SetNull);  


			modelBuilder.Entity<ShoppingCartHistoryModel>()
				.HasOne(h => h.User)
				.WithMany(u => u.ShoppingCartHistories)
				.HasForeignKey(h => h.UserId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<PurchasedProductModel>()
				.HasOne<ShoppingCartHistoryModel>()
				.WithMany(h => h.PurchasedProducts)
				.HasForeignKey(p => p.HistoryId)
				.OnDelete(DeleteBehavior.Cascade);




			modelBuilder.Entity<UserModel>().HasData(
					new UserModel
					{
						UserId = 1,
						Login = "admin",
						Password = "admin",
						Role = RolesEnum.PermissionAdmin,

					},
					new UserModel
					{
						UserId = 2,
						Login = "user",
						Password = "user",
						Role = RolesEnum.PermissionBuyer
					}
				);
			//modelBuilder.Entity<ShoppingCartModel>().HasData(
			//    new ShoppingCartModel
			//    {
			//        Id = 1,
			//        UserId = 1  // Przypisanie UserId do ShoppingCart, by utworzyć powiązanie z UserModel
			//    },

			//    new ShoppingCartModel
			//    {
			//        Id = 2,
			//        UserId = 2 // Przypisanie UserId do ShoppingCart, by utworzyć powiązanie z UserModel
			//    }
			//);

		}


	}
}
