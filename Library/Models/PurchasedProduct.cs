using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
	public class PurchasedProduct
	{
		[Key]
		public int Id { get; set; }
		public int Quantity { get; set; }
		public int ProductId { get; set; }
		[Precision(18,2)]
		public decimal Price { get; set; }
		public string? Description { get; set; }
		public required string Name { get; set; }
	}
}
