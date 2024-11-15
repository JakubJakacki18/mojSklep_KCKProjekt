using Library.Data;
using Library.Models;
using System.ComponentModel.DataAnnotations;

namespace Library.Model
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
		public required string Login { get; set; }
        public required string Password { get; set; }
        public RolesEnum Role { get; set; }
        public ShoppingCartModel ShoppingCart { get; set; }
        public ICollection<ShoppingCartHistoryModel> ShoppingCartHistories { get; set; } = [];

	}


}
