using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class ProductModel
    {
        [Key]
        [MinLength(8)]
        [MaxLength(8)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; } = 0;
        public int shelfRow { get; set; }
        public int shelfColumn { get; set; }


    }
}
