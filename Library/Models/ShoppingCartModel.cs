using Library.Data;

namespace Library.Models
{
    public class ShoppingCartModel
    {
        public List<ProductQuantityModel> ProductsWithQuantity { get; set; } = [];
        public PayementMethodEnum PaymentMethod { get; set; }
        public bool IsPaid { get; set; } = false;


    }
}
