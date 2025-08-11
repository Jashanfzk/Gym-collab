using System.ComponentModel.DataAnnotations;

namespace GymCollab.Models
{
    /// <summary>
    /// Represents gym equipment available for use in classes
    /// </summary>
    public class Equipment
    {
        /// <summary>
        /// Unique identifier for the equipment
        /// </summary>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Name of the equipment item
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        /// <summary>
        /// Category or type of equipment (e.g., Cardio, Strength, Yoga)
        /// </summary>
        [StringLength(50)]
        public string? Category { get; set; }

        /// <summary>
        /// Available quantity of this equipment
        /// </summary>
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// Supplier or manufacturer of the equipment
        /// </summary>
        [StringLength(100)]
        public string? Supplier { get; set; }

        /// <summary>
        /// Path to the equipment image file
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Collection of gym classes that use this equipment
        /// </summary>
        public ICollection<ClassEquipment> ClassEquipments { get; set; } = new List<ClassEquipment>();
    }
}
