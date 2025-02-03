using Library.Data;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Converter;

namespace WpfApp.ViewModel
{
	public class ShoppingCartHistoryViewModel
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public decimal TotalPrice { get; set; }
		public string PaymentMethod { get; set; } 
		public int ProductCount { get; set; }
		public List<PurchasedProductModel> PurchasedProducts { get; set; }

		public ShoppingCartHistoryViewModel(ShoppingCartHistoryModel model)
		{
			Id = model.Id;
			Date = model.Date;
			TotalPrice = model.TotalPrice;
			PaymentMethod = (model.PaymentMethod != null) ? EnumConverter.GetEnumDescription((PaymentMethodEnum)model.PaymentMethod) : "";
			ProductCount = model.PurchasedProducts?.Count ?? 0;
			PurchasedProducts = model.PurchasedProducts?.ToList() ?? [];
		}
	}

}
