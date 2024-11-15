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
			modelBuilder.HasSequence<int>("ProductIds", schema: "shared")
				.StartsAt(10000000)
				.IncrementsBy(1);

			modelBuilder.Entity<ProductModel>()
				.Property(p => p.Id)
				.HasDefaultValueSql("NEXT VALUE FOR shared.ProductIds");
			modelBuilder.Entity<UserModel>().HasData(
				new UserModel
				{
					UserId = 1,
					Login = "admin",
					Password = "admin",
					Role = RolesEnum.PermissionAdmin
				},
				new UserModel
				{
					UserId = 2,
					Login = "user",
					Password = "user",
					Role = RolesEnum.PermissionBuyer
				}
			);


			modelBuilder.Entity<CartProductModel>()
		.HasIndex(cp => new { cp.ProductId, cp.ShoppingCartId })
		.IsUnique();

			// ProductModel <-> CartProductModel: One-to-Many (każdy produkt może mieć wiele kopii w różnych koszykach)
			modelBuilder.Entity<CartProductModel>()
				.HasOne(cp => cp.OriginalProduct)
				.WithMany(p => p.CartProducts)
				.HasForeignKey(cp => cp.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			// ShoppingCartModel <-> CartProductModel: One-to-Many (koszyk może mieć wiele kopii różnych produktów)
			modelBuilder.Entity<CartProductModel>()
				.HasOne(cp => cp.ShoppingCart)
				.WithMany(sc => sc.ProductsInCart)
				.HasForeignKey(cp => cp.ShoppingCartId)
				.OnDelete(DeleteBehavior.Cascade);

			// UserModel <-> ShoppingCartModel: One-to-One
			modelBuilder.Entity<ShoppingCartModel>()
				.HasOne(sc => sc.User)
				.WithOne(u => u.ShoppingCart)
				.HasForeignKey<ShoppingCartModel>(sc => sc.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// ShoppingCartModel <-> ShoppingCartHistoryModel: One-to-Many
			modelBuilder.Entity<ShoppingCartHistoryModel>()
				.HasOne(sh => sh.ShoppingCart)
				.WithMany()
				.HasForeignKey(sh => sh.ShoppingCartId)
				.OnDelete(DeleteBehavior.Restrict);

			// UserModel <-> ShoppingCartHistoryModel: One-to-Many
			modelBuilder.Entity<ShoppingCartHistoryModel>()
				.HasOne(sh => sh.User)
				.WithMany(u => u.ShoppingCartHistories)
				.HasForeignKey(sh => sh.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
