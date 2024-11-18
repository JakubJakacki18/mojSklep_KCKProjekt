

using Library.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
    public class ShoppingCartHistoryModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Precision(18,2)]
        public decimal TotalPrice { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
		public PaymentMethodEnum? PaymentMethod { get; set; }
		public virtual ICollection<CartProductModel> CartProducts { get; set; } = [];
	}
}
