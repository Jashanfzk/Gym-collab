using System.ComponentModel.DataAnnotations;

namespace GymCollab.Models
{
    public class GymClass
    {
        public int GymClassId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [StringLength(100)]
        public string? Trainer { get; set; }

        [StringLength(50)]
        public string? DayOfWeek { get; set; } // e.g., Monday

        [StringLength(20)]
        public string? Time { get; set; } // e.g., 10:00 AM

        public int Capacity { get; set; } = 20;

        public ICollection<ClassEquipment> ClassEquipments { get; set; } = new List<ClassEquipment>();
        public ICollection<ClassEnrollment> Enrollments { get; set; } = new List<ClassEnrollment>();
    }
}
