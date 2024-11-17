using Library.Data;

namespace Library.Models
{
    public class ShoppingCartModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }

        public PayementMethodEnum? PaymentMethod { get; set; }
        public bool IsPaid { get; set; } = false;
    }
}
