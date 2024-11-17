using Library.Data;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public RolesEnum Role { get; set; }
        public virtual ICollection<CartProductModel> ProductsInCart { get; set; } = [];
        public ICollection<ShoppingCartHistoryModel> ShoppingCartHistories { get; set; } = [];

    }


}
