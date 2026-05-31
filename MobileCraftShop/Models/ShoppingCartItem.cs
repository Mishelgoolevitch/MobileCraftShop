using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileCraftShop.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CartId { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; } = 1;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Внешний ключ
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        // Свойства навигации
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        // Вычисляемые свойства
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice => Product != null ? Product.SalePrice * Quantity : 0;
    }
}
