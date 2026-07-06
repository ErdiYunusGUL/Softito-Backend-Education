using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoService_Mvc_Identity_Ajax.Models
{
    public class CarModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Model adı zorunludur.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand? Brand { get; set; }
    }
}
