using System.ComponentModel.DataAnnotations;

namespace GymCollab.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [StringLength(50)]
        public string? Category { get; set; }

        public int Quantity { get; set; } = 0;

        [StringLength(100)]
        public string? Supplier { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<ClassEquipment> ClassEquipments { get; set; } = new List<ClassEquipment>();
    }
}
