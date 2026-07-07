using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoService_Mvc_Identity_Ajax.Models
{
    public class Part
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Parça adı zorunludur.")]
        [StringLength(100)]
        [Display(Name = "Parça Adı")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parça kodu zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Parça Kodu")]
        public string PartCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stok miktarı zorunludur.")]
        [Range(0, 10000, ErrorMessage = "Stok miktarı 0 ile 10000 arasında olmalıdır.")]
        [Display(Name = "Stok Miktarı")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Birim fiyat zorunludur.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Birim Fiyat (TL)")]
        public decimal UnitPrice { get; set; }
    }
}
