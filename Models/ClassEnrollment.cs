namespace GymCollab.Models
{
    public class ClassEnrollment
    {
        public int ClassEnrollmentId { get; set; }

        public int GymClassId { get; set; }
        public GymClass? GymClass { get; set; }

        public int MemberId { get; set; }
        public Member? Member { get; set; }

        public DateTime EnrolledOn { get; set; } = DateTime.UtcNow;
    }
}


