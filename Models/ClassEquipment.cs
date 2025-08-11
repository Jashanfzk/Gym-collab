namespace GymCollab.Models
{
    public class ClassEquipment
    {
        public int ClassEquipmentId { get; set; }
        public int GymClassId { get; set; }
        public int EquipmentId { get; set; }
        public int QuantityUsed { get; set; } = 1;

        public GymClass? GymClass { get; set; }
        public Equipment? Equipment { get; set; }
    }
}
