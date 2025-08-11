using System.ComponentModel.DataAnnotations;
using GymCollab.Models;

namespace GymCollab.ViewModels
{
    public class ClassDetailsVM
    {
        public GymClass? GymClass { get; set; }
        public List<ClassEquipment> Assigned { get; set; } = new();
        public List<Equipment> Available { get; set; } = new();

        [Display(Name="Equipment")]
        public int EquipmentId { get; set; }
        [Display(Name="Quantity")]
        public int Quantity { get; set; } = 1;
    }

    public class DashboardVM
    {
        public int TotalClasses { get; set; }
        public int TotalEquipment { get; set; }
        public int TotalMembers { get; set; }
        public int TotalEnrollments { get; set; }

        public List<GymClass> UpcomingClasses { get; set; } = new();
        public List<Equipment> PopularEquipment { get; set; } = new();
        public List<Member> RecentMembers { get; set; } = new();
    }
}
