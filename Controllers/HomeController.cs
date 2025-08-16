using Microsoft.AspNetCore.Mvc;

namespace GymCollab.Controllers
{
    /// <summary>
    /// Controller for handling the home page and dashboard view.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly Data.AppDbContext _db;
        public HomeController(Data.AppDbContext db) { _db = db; }

        /// <summary>
        /// Displays the dashboard with stats, upcoming classes, popular equipment, and recent members.
        /// </summary>
        public IActionResult Index()
        {
            var vm = new ViewModels.DashboardVM
            {
                TotalClasses = _db.GymClasses.Count(),
                TotalEquipment = _db.Equipment.Count(),
                TotalMembers = _db.Members.Count(),
                TotalEnrollments = _db.ClassEnrollments.Count(),
                UpcomingClasses = _db.GymClasses.OrderBy(c => c.DayOfWeek).ThenBy(c => c.Time).Take(5).ToList(),
                PopularEquipment = _db.Equipment.OrderByDescending(e => e.Quantity).Take(5).ToList(),
                RecentMembers = _db.Members.OrderByDescending(m => m.JoinedOn).Take(5).ToList()
            };
            return View(vm);
        }
    }
}
