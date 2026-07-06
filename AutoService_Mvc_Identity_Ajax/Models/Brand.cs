using System.ComponentModel.DataAnnotations;

namespace AutoService_Mvc_Identity_Ajax.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka adı zorunludur.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public ICollection<CarModel>? CarModels { get; set; }
    }
}
