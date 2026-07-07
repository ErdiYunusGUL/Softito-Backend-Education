using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoService_Mvc_Identity_Ajax.Models
{
    public class ServicePart
    {
        public int Id { get; set; }

        [Required]
        public int ServiceRecordId { get; set; }

        [ForeignKey("ServiceRecordId")]
        public ServiceRecord? ServiceRecord { get; set; }

        [Required]
        public int PartId { get; set; }

        [ForeignKey("PartId")]
        public Part? Part { get; set; }

        [Required(ErrorMessage = "Miktar zorunludur.")]
        [Range(1, 100, ErrorMessage = "Geçerli bir miktar giriniz.")]
        [Display(Name = "Kullanılan Miktar")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Satış Fiyatı (TL)")]
        public decimal UnitPrice { get; set; } // Fiyat zamanla değişebileceği için satış anındaki fiyatı tutuyoruz.
    }
}
