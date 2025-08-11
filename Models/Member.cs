using System.ComponentModel.DataAnnotations;

namespace GymCollab.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress, StringLength(200)]
        public string? Email { get; set; }

        [Phone, StringLength(30)]
        public string? Phone { get; set; }

        public DateTime JoinedOn { get; set; } = DateTime.UtcNow;

        [StringLength(200)]
        public string? PreferredTrainer { get; set; }

        public ICollection<ClassEnrollment> Enrollments { get; set; } = new List<ClassEnrollment>();
    }
}


