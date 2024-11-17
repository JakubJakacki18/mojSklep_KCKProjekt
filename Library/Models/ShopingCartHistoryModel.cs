

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
