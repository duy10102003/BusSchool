using BusSchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusSchool.Models;

namespace ProjectPrn.Controllers
{
    public class HomeController : Controller
    {
        BusSchoolContext context = new BusSchoolContext();
        public ActionResult Index()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            LinesBusesDriversView LBDV = new LinesBusesDriversView
            {
                Trips = context.Trips.Include(c => c.Bus).Include(c => c.Bus.Driver).Include(c => c.Bus.Seats).Include(c => c.Line).OrderByDescending(c => c.Id).Take(3),
                Buses = context.Buses.Include(c => c.Seats).Include(c => c.Driver).OrderByDescending(c => c.Id).Take(3),
                Drivers = context.Drivers.OrderByDescending(c => c.Id).Take(3)
            };
            //Hngeb top 3 inserted Line, Top 3 Buses, Top 3 Drivers           

            return View(LBDV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(string ContactName, string ContactEmail, string ContactPhone, string ContactMessage)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            LinesBusesDriversView LBDV = new LinesBusesDriversView
            {
                Trips = context.Trips.Include(c => c.Bus).Include(c => c.Bus.Driver).Include(c => c.Bus.Seats).Include(c => c.Line).OrderByDescending(c => c.Id).Take(3),
                Buses = context.Buses.Include(c => c.Seats).Include(c => c.Driver).OrderByDescending(c => c.Id).Take(3),
                Drivers = context.Drivers.OrderByDescending(c => c.Id).Take(3)
            };
            DateTime Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", null);

            if (ContactMessage!=null)
            {
                ContactUsForm contactUs = new ContactUsForm()
                {
                    Name = ContactName,
                    EmailAddress = ContactEmail,
                    PhoneNumber = ContactPhone,
                    Message = ContactMessage,
                    TimeStamp = Today.ToString(),
                };
                context.ContactUsForms.Add(contactUs);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index",LBDV);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
