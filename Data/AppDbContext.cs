using GymCollab.Models;
using Microsoft.EntityFrameworkCore;

namespace GymCollab.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<GymClass> GymClasses => Set<GymClass>();
        public DbSet<Equipment> Equipment => Set<Equipment>();
        public DbSet<ClassEquipment> ClassEquipments => Set<ClassEquipment>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<ClassEnrollment> ClassEnrollments => Set<ClassEnrollment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClassEquipment>()
                .HasOne(ce => ce.GymClass)
                .WithMany(gc => gc.ClassEquipments)
                .HasForeignKey(ce => ce.GymClassId);

            modelBuilder.Entity<ClassEquipment>()
                .HasOne(ce => ce.Equipment)
                .WithMany(e => e.ClassEquipments)
                .HasForeignKey(ce => ce.EquipmentId);

            modelBuilder.Entity<ClassEnrollment>()
                .HasOne(e => e.GymClass)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.GymClassId);

            modelBuilder.Entity<ClassEnrollment>()
                .HasOne(e => e.Member)
                .WithMany(m => m.Enrollments)
                .HasForeignKey(e => e.MemberId);
        }
    }

    public static class SeedData
    {
        public static void Initialize(AppDbContext db)
        {
            if (db.GymClasses.Any()) return;

            var classes = new List<GymClass> {
                new GymClass { Name = "Zumba 101", Trainer = "Alice", DayOfWeek = "Monday", Time = "10:00 AM", Capacity = 20 },
                new GymClass { Name = "Yoga Power", Trainer = "Bob", DayOfWeek = "Wednesday", Time = "7:00 PM", Capacity = 15 }
            };
            db.GymClasses.AddRange(classes);

            var eq = new List<Equipment> {
                new Equipment { Name = "Yoga Mat", Category = "Cardio", Quantity = 30, Supplier = "FitCo" },
                new Equipment { Name = "Resistance Bands", Category = "Strength", Quantity = 20, Supplier = "Bandify" },
                new Equipment { Name = "Dumbbells", Category = "Strength", Quantity = 40, Supplier = "IronWorks" }
            };
            db.Equipment.AddRange(eq);
            db.SaveChanges();

            db.ClassEquipments.AddRange(
                new ClassEquipment { GymClassId = classes[0].GymClassId, EquipmentId = eq[0].EquipmentId, QuantityUsed = 10 },
                new ClassEquipment { GymClassId = classes[1].GymClassId, EquipmentId = eq[1].EquipmentId, QuantityUsed = 8 }
            );

            var members = new List<Member>
            {
                new Member { FullName = "John Doe", Email = "john@example.com", Phone = "+1-555-0100", PreferredTrainer = "Alice" },
                new Member { FullName = "Jane Smith", Email = "jane@example.com", Phone = "+1-555-0101", PreferredTrainer = "Bob" }
            };
            db.Members.AddRange(members);
            db.SaveChanges();

            db.ClassEnrollments.AddRange(
                new ClassEnrollment { GymClassId = classes[0].GymClassId, MemberId = members[0].MemberId },
                new ClassEnrollment { GymClassId = classes[1].GymClassId, MemberId = members[1].MemberId }
            );
            db.SaveChanges();
        }
    }
}
