using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class CartProductModel
    {
        [Key]
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public int? UserId { get; set; }
        public UserModel User { get; set; }


        public int ProductId { get; set; }
        public ProductModel OriginalProduct { get; set; }

    }
}