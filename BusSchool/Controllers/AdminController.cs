
using BusSchool.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Web;

namespace BusSchool
{

    public class AdminController : Controller
    {
        private BusSchoolContext context = new BusSchoolContext();
        public readonly IWebHostEnvironment _webHost;

        public AdminController(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        // GET: Admin
        public ActionResult Index()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            DashboardViewModel DBVM = new DashboardViewModel
            {
                Trips = context.Trips.Include(c => c.Bus).Include(c => c.Line).ToList(),
                Feedbacks = context.Feedbacks.Include(c => c.PassengerId).Include(c => c.PassengerId.ApplicationUser).Include(c => c.TripId).ToList(),
                Buses = context.Buses.Count(),
                Passengers = context.Passengers.Count(),
                Tickets = context.Tickets.Count(),
                ContactUsForms = context.ContactUsForms.ToList()
            };
            return View(DBVM);
        }


        public ActionResult AdminProfile()
        {
            
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            var AdminId = CurrentUser;
            if (AdminId != null)
            {
                User admin = context.Users.SingleOrDefault(c => c.Id == AdminId);
                return View(admin);
            }
            return View("Error");
        }

        

        //-----------------------------------Ticket--------------------------------------------
        public ActionResult Tickets()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            PassengersTicketsView PTV = new PassengersTicketsView
            {
                Passengers = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).ToList(),
                Tickets = context.Tickets.Include(c => c.Trip.Bus).Include(c => c.Payment).Include(c => c.Trip).Include(c => c.Trip.Line).ToList(),
                Buses = context.Buses.Include(c => c.Driver).Include(c => c.Seats).ToList()
            };
            return View(PTV);
        }

        [Route("Admin/ViewTicket/{pid}/{tid}")]
        public ActionResult ViewTicket(int? pid, int tid)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (pid == null)
            {
                return View("Error");
            }
            PassengerTicketView PTV = new PassengerTicketView
            {
                Passenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.Id == pid),
                Ticket = context.Tickets.Include(c => c.Payment).Include(c => c.Trip).Include(c => c.Trip.Bus).Include(c => c.Trip.Line).SingleOrDefault(c => c.Id == tid),
                Seats = context.Seats.Include(c => c.Passenger).Where(c => c.PassengerId == pid)
            };

            if (PTV == null)
            {
                return View("Error");
            }
            return View(PTV);
        }

        [Route("Admin/DeleteTicket/{pid}/{tid}")]
        public ActionResult DeleteTicket(int? pid, int tid)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (pid == null)
            {
                return View("Error");
            }
            PassengerTicketView PTV = new PassengerTicketView
            {
                Passenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.Id == pid),
                Ticket = context.Tickets.Include(c => c.Payment).Include(c => c.Trip).Include(c => c.Trip.Bus).Include(c => c.Trip.Line).SingleOrDefault(c => c.Id == tid),
                Seats = context.Seats.Include(c => c.Passenger).Where(c => c.PassengerId == pid)
            };

            if (PTV == null)
            {
                return View("Error");
            }

            foreach (var Seat in PTV.Seats)
            {
                context.Seats.Remove(Seat);
            }

            context.Tickets.Remove(PTV.Ticket);

            context.SaveChanges();

            

            return RedirectToAction("ViewPassenger", new { pid = pid });
        }


        [Route("Admin/BlockTicket/{pid}/{tid}")]
        public ActionResult BlockTicket(int? pid, int tid)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (pid == null)
            {
                return View("Error");
            }
            PassengerTicketView PTV = new PassengerTicketView
            {
                Passenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.Id == pid),
                Ticket = context.Tickets.Include(c => c.Payment).Include(c => c.Trip).Include(c => c.Trip.Bus).Include(c => c.Trip.Line).SingleOrDefault(c => c.Id == tid)
            };

            if (PTV == null)
            {
                return View("Error");
            }
            if (PTV.Ticket.IsBlocked)
            {
                PTV.Ticket.IsBlocked = false;
                IEnumerable<Seat> Seats = context.Seats.Where(c => c.PassengerId == pid);
                foreach (var Seat in Seats)
                {
                    Seat.IsAvailable = false;
                }

            }
            else
            {
                PTV.Ticket.IsBlocked = true;
                IEnumerable<Seat> Seats = context.Seats.Where(c => c.PassengerId == pid);
                foreach (var Seat in Seats)
                {
                    Seat.IsAvailable = true;
                }
            }
            context.SaveChanges();

            

            return RedirectToAction("ViewTicket", new { pid = pid, tid = tid });
        }

        //--------------------------------------------------Passenger-----------------------------------------
        public ActionResult Passengers()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            return View(context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).ToList());
        }

        public ActionResult ViewPassenger(int? id)
        {
            if (id == null)
            {
                var CurrentUser = HttpContext.Session.GetString("CurrentUser");
                User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
                ViewBag.CurrentUser = user;
                return View("Error");
            }
            PassengerUserTicketView PUTV = new PassengerUserTicketView
            {
                Passenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.Id == id),
                Tickets = context.Tickets.Include(c => c.Trip).Include(c => c.Trip.Line).Include(c => c.Payment).ToList()
            };

            if (PUTV.Passenger == null)
            {
                return View("Error");
            }
            return View(PUTV);
        }


        public ActionResult EditPassenger(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            Passenger passenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.Id == id);

            if (passenger == null)
            {
                return View("Error");
            }

            return View(passenger);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassenger(int Id, bool checkBlock)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            string mess = "";
            if (ModelState.IsValid)
            {

                if (Id != null && checkBlock != null)
                {
                    if (Id != 0)
                    {
                        Passenger passengerInDB = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).Single(c => c.Id == Id);
                        passengerInDB.Blocked = checkBlock;

                        var Seats = context.Seats.Where(c => c.PassengerId == Id);
                        foreach (var Seat in Seats)
                        {
                            Seat.IsAvailable = checkBlock;
                        }

                        context.SaveChanges();

                        return RedirectToAction("ViewPassenger", new { id = Id });
                    }
                    ///err
                    return View("Error", mess);
                }

                return View();
            }
            return View();
        }

        public ActionResult DeletePassenger(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            Passenger passenger = context.Passengers.Find(id);
            if (passenger == null)
            {
                return View("Error");
            }

            try
            {
                context.Passengers.Remove(passenger);
                context.SaveChanges();
                return RedirectToAction("Passengers");
            }
            catch
            {
                return View("Error");
            }


        }
        //--------------Start Trip---------------------//
        public ActionResult Trips()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            return View(context.Trips.Include(c => c.Line).Include(c => c.Bus).Include(c => c.Bus.Seats).Include(c => c.Bus.Driver).ToList());
        }

        public ActionResult AddTrip()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            TripLineBusView Trip = new TripLineBusView
            {
                Lines = context.Lines.ToList(),
                Buses = context.Buses.ToList()
            };

            return View("AddTrip", Trip);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTrip(int lid, int bid, DateTime TripTime, IFormFile TripPictureFile)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            TripLineBusView Trip = new TripLineBusView
            {
                Lines = context.Lines.ToList(),
                Buses = context.Buses.ToList()
            };
            string picture = "";
            if (ModelState.IsValid)
            {
                if (TripPictureFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image","trips");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string fileName = Path.GetFileName(TripPictureFile.FileName);
                    string fileSavePath = Path.Combine(uploadsFolder, fileName);
                    using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                    {
                        await TripPictureFile.CopyToAsync(stream);
                    }
                    picture = fileName;

                }
                Trip trip = new Trip()
                {
                    LineId = lid,
                    BusId = bid,
                    Time = TripTime.ToString(),
                    TripPicture = picture
                };
                context.Trips.Add(trip);
                context.SaveChanges();
                return RedirectToAction("Trips");
            }
            return View(Trip);

        }

        public ActionResult ViewTrip(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error", 1);
            }

            Trip trip = context.Trips.Include(c => c.Line).Include(c => c.Bus).Include(c => c.Bus.Driver).Include(c => c.Bus.Seats).SingleOrDefault(c => c.Id == id);

            if (trip == null)
            {
                return View("Error", 1);
            }
            return View(trip);
        }

        public ActionResult EditTrip(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error", 1);
            }
            TripLineBusView trip = new TripLineBusView
            {
                Trip = context.Trips.Find(id),
                Lines = context.Lines.ToList(),
                Buses = context.Buses.ToList()
            };

            var tripFind = context.Trips.Find(id);
            int busSelect = tripFind.Bus.Id;
            int lineSelect = tripFind.Line.Id;
            ViewBag.busSelect = busSelect;
            ViewBag.lineSelect = lineSelect;
            if (trip == null)
            {
                return View("Error", 1);
            }
            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTrip(int tid, int lid, int bid, DateTime TripTime, IFormFile TripPictureFile)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            TripLineBusView trip = new TripLineBusView
            {
                Trip = context.Trips.Find(tid),
                Lines = context.Lines.ToList(),
                Buses = context.Buses.ToList()
            };
            if (ModelState.IsValid)
            {
                if (tid != 0)
                {
                    Trip tripInDB = context.Trips.Single(c => c.Id == tid);
                    if (TripPictureFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image","trips");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        string fileName = Path.GetFileName(TripPictureFile.FileName);
                        string fileSavePath = Path.Combine(uploadsFolder, fileName);
                        using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                        {
                            await TripPictureFile.CopyToAsync(stream);
                        }
                        tripInDB.TripPicture = fileName;
                    }

                    tripInDB.LineId = lid;
                    tripInDB.BusId = bid;
                    tripInDB.Time = TripTime.ToString();

                    context.SaveChanges();
                    return RedirectToAction("ViewTrip", new { id = tid });
                }
                return View("Error", 1);
            }
            return View(trip);

        }


        public ActionResult DeleteTrip(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error", 1);
            }
            Trip trip = context.Trips.Find(id);
            if (trip == null)
            {
                return View("Error", 1);
            }

            try
            {
                context.Trips.Remove(trip);
                context.SaveChanges();
                return RedirectToAction("Trips");
            }
            catch
            {
                return View("Error", 1);
            }
        }

        //--------------------------Bus-----------------------------------

        public ActionResult Buses()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            return View(context.Buses.Include(c => c.Driver).Include(c => c.Seats).ToList());
        }

        public ActionResult AddBus()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            BusDriverView bus = new BusDriverView
            {
                Drivers = context.Drivers.ToList()
            };
            return View("AddBus", bus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddBus(string busColor, string busNumber, int maximumSeat, int dId, IFormFile BusPictureFile)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            BusDriverView bus = new BusDriverView
            {
                Drivers = context.Drivers.ToList()
            };
            string picture = "";
            if (ModelState.IsValid)
            {
                if (BusPictureFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image","buses");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string fileName = Path.GetFileName(BusPictureFile.FileName);
                    string fileSavePath = Path.Combine(uploadsFolder, fileName);
                    using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                    {
                        await BusPictureFile.CopyToAsync(stream);
                    }
                    picture = fileName;

                }
                bus Bus = new bus()
                {
                    BusNumber = busNumber,
                    Color = busColor,
                    DriverId = dId,
                    MaximumSeats = maximumSeat,
                    BusPicture = picture

                };
                context.Buses.Add(Bus);
                context.SaveChanges();
                return RedirectToAction("Buses");
            }
            return View(bus);

        }

        public ActionResult ViewBus(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }

            bus bus = context.Buses.Include(c => c.Driver).Include(c => c.Seats).SingleOrDefault(c => c.Id == id);

            if (bus == null)
            {
                return View("Error");
            }
            return View(bus);
        }

        public ActionResult EditBus(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            BusDriverView bus = new BusDriverView
            {
                Bus = context.Buses.Find(id),
                Drivers = context.Drivers.ToList()
            };

            if (bus == null)
            {
                return View("Error");
            }
            return View(bus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBus(int bid, string busColor, string busNumber, int maximumSeat, int dId, IFormFile BusPictureFile)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            BusDriverView bus = new BusDriverView
            {
                Bus = context.Buses.Find(bid),
                Drivers = context.Drivers.ToList()
            };
            if (ModelState.IsValid)
            {
                if (bid != 0)
                {
                    bus busInDB = context.Buses.Single(c => c.Id == bid);
                    if (BusPictureFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image","buses");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        string fileName = Path.GetFileName(BusPictureFile.FileName);
                        string fileSavePath = Path.Combine(uploadsFolder, fileName);
                        using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                        {
                            await BusPictureFile.CopyToAsync(stream);
                        }
                        busInDB.BusPicture = fileName;
                    }

                    busInDB.Color = busColor;
                    busInDB.DriverId = dId;
                    busInDB.BusNumber = busNumber;
                    busInDB.MaximumSeats = maximumSeat;

                    Driver driver = context.Drivers.Find(dId);

                    driver.IsAvailable = false;
                    context.SaveChanges();
                    return RedirectToAction("ViewBus", new { id = bid });
                }
                return View("Error");
            }
            return View(bus);
        }



        public ActionResult DeleteBus(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            bus bus = context.Buses.Find(id);
            if (bus == null)
            {
                View("Error");
            }

            try
            {
                context.Buses.Remove(bus);
                context.SaveChanges();
                return RedirectToAction("Buses");
            }
            catch
            {
                return View("Error");
            }
        }

        //-------------------------------------------------Drivers-------------------------------------------

        public ActionResult Drivers()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            return View(context.Drivers.ToList());
        }

        public ActionResult AddDriver()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            return View("AddDriver");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddDriver(string driverName, string phoneNumber, string Ssn, string emailAddress, string driverLicence, IFormFile ProfilePictureFile)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (ModelState.IsValid)
            {
                string picture = "";

                if (ProfilePictureFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image","drivers");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string fileName = Path.GetFileName(ProfilePictureFile.FileName);
                    string fileSavePath = Path.Combine(uploadsFolder, fileName);
                    using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                    {
                        await ProfilePictureFile.CopyToAsync(stream);
                    }
                    picture = fileName;
                }
                Driver driver = new Driver()
                {
                    Name = driverName,
                    PhoneNumber = phoneNumber,
                    EmailAddress = emailAddress,
                    DriverLicence = driverLicence,
                    Ssn = Ssn,
                    ProfilePicture = picture
                };
                context.Drivers.Add(driver);
                context.SaveChanges();
                return RedirectToAction("Drivers");
            }
            return View();

        }

        public ActionResult ViewDriver(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            Driver driver = context.Drivers.SingleOrDefault(c => c.Id == id);
            if (driver == null)
            {
                return View("Error");
            }
            return View(driver);
        }

        public ActionResult EditDriver(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            Driver driver = context.Drivers.Find(id);
            if (driver == null)
            {
                return View("Error");
            }
            return View(driver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDriver(int dId, string driverName, string phoneNumber, string Ssn, string emailAddress, string driverLicence, IFormFile ProfilePictureFile)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            Driver driver = context.Drivers.Find(dId);
            if (ModelState.IsValid)
            {
                if (dId != 0)
                {
                    Driver driverInDB = context.Drivers.Single(c => c.Id == dId);

                    if (ProfilePictureFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image","drivers");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        string fileName = Path.GetFileName(ProfilePictureFile.FileName);
                        string fileSavePath = Path.Combine(uploadsFolder, fileName);
                        using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                        {
                            await ProfilePictureFile.CopyToAsync(stream);
                        }
                        driverInDB.ProfilePicture = fileName;
                    }

                    driverInDB.Name = driverName;
                    driverInDB.PhoneNumber = phoneNumber;
                    driverInDB.Ssn = Ssn;
                    driverInDB.DriverLicence = driverLicence;
                    driverInDB.EmailAddress = emailAddress;

                    context.SaveChanges();
                    return RedirectToAction("ViewDriver", new { id = dId });
                }
                return View("Error");
            }
            return View(driver);

        }


        public ActionResult DeleteDriver(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            Driver driver = context.Drivers.Find(id);
            if (driver == null)
            {
                return View("Error");
            }

            try
            {
                context.Drivers.Remove(driver);
                context.SaveChanges();
                return RedirectToAction("Drivers");
            }
            catch
            {
                return View("Error");
            }


        }

        //////---------------------------Line---------------------------
        ///
        public ActionResult Lines()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            return View(context.Lines.ToList());
        }

        public ActionResult AddLine()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            return View("AddLine");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddLine(string from, string to, int price, IFormFile LinePictureFile)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            string picture = "";
            if (ModelState.IsValid)
            {
                if (LinePictureFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image","lines");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string fileName = Path.GetFileName(LinePictureFile.FileName);
                    string fileSavePath = Path.Combine(uploadsFolder, fileName);
                    using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                    {
                        await LinePictureFile.CopyToAsync(stream);
                    }
                    picture = fileName;
                }
                Line line = new Line()
                {
                    From = from,
                    To = to,
                    Price = price,
                    LinePicture = picture
                };

                context.Lines.Add(line);
                context.SaveChanges();
                return RedirectToAction("Lines");
            }
            return View();
        }

        public ActionResult ViewLine(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            Line line = context.Lines.SingleOrDefault(c => c.Id == id);
            if (line == null)
            {
                return View("Error");
            }
            return View(line);
        }

        public ActionResult EditLine(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            Line line = context.Lines.Find(id);
            if (line == null)
            {
                return View("Error");
            }
            return View(line);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditLine(int lId, string from, string to, int price, IFormFile LinePictureFile)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            Line line = context.Lines.SingleOrDefault(c => c.Id == lId);
            if (ModelState.IsValid)
            {
                if (lId != 0)
                {
                    Line lineInDB = context.Lines.Single(c => c.Id == lId);
                    if (LinePictureFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image","lines");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        string fileName = Path.GetFileName(LinePictureFile.FileName);
                        string fileSavePath = Path.Combine(uploadsFolder, fileName);
                        using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                        {
                            await LinePictureFile.CopyToAsync(stream);
                        }
                        lineInDB.LinePicture = fileName;
                    }

                    lineInDB.From = from;
                    lineInDB.To = to;
                    lineInDB.Price = price;

                    context.SaveChanges();
                    return RedirectToAction("ViewLine", new { id = lId });
                }
                return View("Error");
            }
            return View(line);


        }

        public ActionResult DeleteLine(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            Line line = context.Lines.Find(id);
            if (line == null)
            {
                return View("Error");
            }

            try
            {
                context.Lines.Remove(line);
                context.SaveChanges();
                return RedirectToAction("Lines");
            }
            catch
            {
                return View("Error");
            }


        }


        //-------------------------------------------------FeedBack-----------------------

        public ActionResult Feedbacks()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            return View(context.Feedbacks.Include(c => c.PassengerId).Include(c => c.PassengerId.ApplicationUser).Include(c => c.TripId).Include(c => c.TripId.Line).OrderByDescending(c => c.Id).ToList());
        }

        public ActionResult ViewFeedback(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            Feedback feedback = context.Feedbacks.Include(c => c.PassengerId).Include(c => c.PassengerId.ApplicationUser).Include(c => c.TripId).Include(c => c.TripId.Bus).Include(c => c.TripId.Line).SingleOrDefault(c => c.Id == id);
            if (feedback == null)
            {
                return View("Error");
            }
            return View(feedback);
        }

        public ActionResult ContactForms()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            return View(context.ContactUsForms.OrderByDescending(c => c.Id).ToList());
        }

        public ActionResult ViewContactForm(int? id)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (id == null)
            {
                return View("Error");
            }
            ContactUsForm contactUsForm = context.ContactUsForms.SingleOrDefault(c => c.Id == id);
            if (contactUsForm == null)
            {
                return View("Error");
            }
            return View(contactUsForm);
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
