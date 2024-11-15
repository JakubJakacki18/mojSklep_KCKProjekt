using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository
{
	public class ShoppingCartRepository(ApplicationDbContext context)
	{
		private readonly ApplicationDbContext _context = context;

		public void AddProductToCart(int productId, int quantity)
		{
			// Add product to cart
		}

		public void RemoveProductFromCart(int productId)
		{
			// Remove product from cart
		}

		public void UpdateProductQuantity(int productId, int quantity)
		{
			// Update product quantity
		}

		public void ClearCart()
		{
			// Clear cart
		}

		public void Checkout()
		{
			// Checkout
		}

		public bool Add() 
		{
			throw new NotImplementedException();
		}
		public bool Remove()
		{
			throw new NotImplementedException();
		}
		public bool Update()
		{
			throw new NotImplementedException();
		}
		public bool Clear()
		{
			throw new NotImplementedException();
		}
		public bool SaveChanges()
			=> _context.SaveChanges() > 0;
		
	}
}
