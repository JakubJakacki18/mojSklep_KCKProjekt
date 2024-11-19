using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
	public class PurchasedProductModel
	{
		[Key]
		public int Id { get; set; }
		public int Quantity { get; set; }
		public int ProductId { get; set; }
		public int HistoryId { get; set; }
		[Precision(18,2)]
		public decimal Price { get; set; }
		public string? Description { get; set; }
		public required string Name { get; set; }
		public int? UserId { get; set; }
		public UserModel User { get; set; }
	}
}
