using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
	public class ShoppingCartHistoryModel
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public double TotalPrice { get; set; }

		public int UserId { get; set; }
		public UserModel User { get; set; }

		public int ShoppingCartId { get; set; }
		public ShoppingCartModel ShoppingCart { get; set; }

	}
}
