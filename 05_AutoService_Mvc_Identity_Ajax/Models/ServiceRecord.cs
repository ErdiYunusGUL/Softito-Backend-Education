using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoService_Mvc_Identity_Ajax.Models
{
    public class ServiceRecord
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Araç modeli seçilmelidir.")]
        public int CarModelId { get; set; }

        [ForeignKey("CarModelId")]
        public CarModel? CarModel { get; set; }

        [Required(ErrorMessage = "Plaka zorunludur.")]
        [StringLength(20)]
        [Display(Name = "Araç Plakası")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arıza açıklaması zorunludur.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Arıza Açıklaması")]
        public string FaultDescription { get; set; } = string.Empty;

        public DateTime EntryDate { get; set; } = DateTime.Now;

        // Navigation Property
        public ICollection<ServiceAction> ServiceActions { get; set; } = new List<ServiceAction>();
        public ICollection<ServicePart> ServiceParts { get; set; } = new List<ServicePart>();
    }
}
