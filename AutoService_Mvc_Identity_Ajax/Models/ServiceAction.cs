using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoService_Mvc_Identity_Ajax.Models
{
    public class ServiceAction
    {
        public int Id { get; set; }

        [Required]
        public int ServiceRecordId { get; set; }

        [ForeignKey("ServiceRecordId")]
        public ServiceRecord? ServiceRecord { get; set; }

        [Required(ErrorMessage = "İşlem adı zorunludur.")]
        [StringLength(255)]
        [Display(Name = "Yapılan İşlem")]
        public string ActionName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Ücret (TL)")]
        public decimal Price { get; set; }
    }
}
