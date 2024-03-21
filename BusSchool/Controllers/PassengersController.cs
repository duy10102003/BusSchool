using BusSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace BusSchool.Controllers
{
    public class PassengersController : Controller
    {
        BusSchoolContext context = new BusSchoolContext();
        public IActionResult Index()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user1 = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user1;

           

            //View Informations, View Tickets Details in table   
            PassengerUserTicketView PUTV = new PassengerUserTicketView
            {
                Passenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.ApplicationUser.Id == CurrentUser),
                Tickets = context.Tickets.Include(c => c.Payment).Include(c => c.Trip).ToList(),
                Lines = context.Lines.ToList(),
                Buses = context.Buses.ToList(),
                
            };

            var Feedbacks = context.Feedbacks.Include(c => c.PassengerId).Include(c => c.TripId).FirstOrDefault(c => c.PassengerId.Id == PUTV.Passenger.Id);

            ViewBag.Feedbacks = Feedbacks;

            if (PUTV.Passenger == null)
            {
                User user = context.Users.SingleOrDefault(c => c.Id == CurrentUser);
                if (user != null)
                {
                    return View(user);
                }

            }
            return View("PassengerProfile", PUTV);
        }

        public IActionResult Booking()
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            BookingViewModel BVM = GetBookingViewModel();
            

            return View(BVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Booking(BookingTicket ticket, int tripId)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (ModelState != null)
            {
                if (ticket.NumberOfChairs != 0)
                {
                    Trip trip = context.Trips.Include(c => c.Bus).Include(c => c.Bus.Seats).Include(c => c.Line).SingleOrDefault(c => c.Id == tripId);
                    if (trip != null)
                    {
                        var FreeSeats = trip.Bus.MaximumSeats;
                        if (trip.Bus.Seats.Count != 0)
                        {
                            FreeSeats = trip.Bus.MaximumSeats - trip.Bus.Seats.Count;
                        }

                        if (FreeSeats >= ticket.NumberOfChairs)
                        {
                            
                            if (user != null)
                            {
                                Passenger passenger = context.Passengers.SingleOrDefault(c => c.ApplicationUser.Id == user.Id);
                                if (passenger == null)
                                {
                                    //Creating Passenger and Adding it to DB
                                    passenger = new Passenger();
                                    passenger.ApplicationUser = user;
                                    passenger.Tickets = new List<Ticket>();
                                    context.Passengers.Add(passenger);
                                    context.SaveChanges();
                                }
                                else if (passenger != null && passenger.Blocked == false)
                                {
                                    if (passenger.Tickets == null)
                                    {
                                        passenger.Tickets = new List<Ticket>();
                                    }

                                    var BookedSeats = trip.Bus.Seats;
                                    List<int> SeatsNumbers = new List<int>();
                                    foreach (var Seat in BookedSeats)
                                    {
                                        if (Seat.IsAvailable == true)
                                            SeatsNumbers.Add(Seat.SeatNumber);
                                    }
                                    for (int i = 0; i < ticket.NumberOfChairs; i++)
                                    {
                                        for (int y = 0; y < trip.Bus.MaximumSeats; y++)
                                        {
                                            if (!SeatsNumbers.Contains(y))
                                            {
                                                //Initializate new Seat
                                                Seat one = new Seat
                                                {
                                                    IsAvailable = false,
                                                    PassengerId = passenger.Id,
                                                    SeatNumber = y + 1
                                                };
                                                //Save Seat in DB and add it in BookedSeats in Bus of the particilar Trip
                                                //Seat SeatId = db.Seats.Add(one);
                                                trip.Bus.Seats.Add(one);
                                                SeatsNumbers.Add(y);
                                                break;
                                            }
                                        }
                                    }

                                    Ticket Newticket = new Ticket
                                    {
                                        Trip = trip,
                                        PaymentId = 1,
                                        BookingTime = DateTime.Now.ToString(),
                                        IsBlocked = false
                                    };

                                    passenger.Tickets.Add(Newticket);
                                    //Save new Seats to DB
                                    context.SaveChanges();

                                }
                            }
                            else
                            {
                            
                            }
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        
                    }
                    return RedirectToAction("Index");//After Booking
                }
                else
                {
                    BookingViewModel BWM = GetBookingViewModel();
                    ViewBag.selectedId = tripId;
                    
                    var selectTrip = context.Trips.Include(c => c.Bus).Include(c => c.Bus.Seats).Include(c => c.Line).SingleOrDefault(c => c.Id == tripId);
                    ViewBag.selectedTrip = selectTrip;
                    return View(BWM);

                }
            }
            return RedirectToAction("Booking");
        }

        public BookingViewModel GetBookingViewModel()
        {
            BookingViewModel BVM = new BookingViewModel
            {
                Trips = context.Trips.Include(c => c.Bus).Include(c => c.Bus.Seats).Include(c => c.Line).Include(c => c.Bus.Driver).ToList()
            };
            //To return unexpired Trips
            var Trips = new List<Trip>();
            foreach (var Trip in BVM.Trips)
            {
                DateTime Time = DateTime.Parse(Trip.Time);
                String TimeString = Time.ToString("dd/MM/yyyy HH:mm");
                Time = DateTime.ParseExact(TimeString, "dd/MM/yyyy HH:mm", null);
                DateTime Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", null);
                if (DateTime.Compare(Time, Today) >= 0)
                {
                    Trips.Add(Trip);
                }
            }
            BVM.Trips = Trips;
            return BVM;
        }




        [Route("Passengers/Ticket/{pid}/{tid}/{trip}")]
        public ActionResult Ticket(int pid, int tid, int trip)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            //W ynf3 y3dl el feedback aw y3mlo Delete
            //Hyb2a Fe 3 Bottuns Add/Edit/Delete FeedBack, Add Seat and Delete if it not expired
            //Hyb2a Fe 3 Bottuns Add/Edit/Delete FeedBack if it expired
            if (pid == 0 || tid == 0 || trip == 0)
            {
                
            }
            PassengerTicketView PUTV = new PassengerTicketView
            {
                Passenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.Id == pid),
                Ticket = context.Tickets.Include(c => c.Payment).Include(c => c.Trip).Include(c => c.Trip.Bus).Include(c => c.Trip.Bus.Driver).Include(c => c.Trip.Line).SingleOrDefault(c => c.Id == tid),
                Seats = context.Seats.Include(cc => cc.Passenger).Where(cc => cc.PassengerId == pid),
                Feedbacks = context.Feedbacks.Include(c => c.PassengerId).Include(c => c.TripId).Where(c => c.PassengerId.Id == pid && c.TripId.Id == trip)
            };

            if (PUTV.Passenger == null || PUTV.Ticket == null)
            {
               /// not found
            }

            return View(PUTV);
        }


        [Route("Passengers/CancelBooking/{pid}/{tid}")]
        public ActionResult CancelBooking(int pid, int tid)
        {
            if (pid == 0 || tid == 0)
            {
                
            }

            PassengerTicketView PTV = new PassengerTicketView
            {
                Passenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.Id == pid),
                Ticket = context.Tickets.Include(c => c.Payment).Include(c => c.Trip).Include(c => c.Trip.Line).Include(c => c.Trip.Bus).Include(c => c.Trip.Bus.Seats).Single(c => c.Id == tid),
                Seats = context.Seats.Include(c => c.Passenger).Where(c => c.PassengerId == pid)
            };

            if (PTV.Passenger == null || PTV.Ticket == null || PTV.Seats == null)
            {
               
            }
            //Delete All Seats Before Ticket 
            foreach (var Seat in PTV.Seats)
            {
                if (PTV.Ticket.Trip.Bus.Seats.Contains(Seat))
                {
                    context.Seats.Remove(Seat);
                }
            }

            if (PTV.Passenger.Tickets.Count == 1)
            {        
                context.Tickets.Remove(PTV.Ticket);
                context.Passengers.Remove(PTV.Passenger);
            }
            else
            {
                context.Tickets.Remove(PTV.Ticket);
            }

            context.SaveChanges();

            return RedirectToAction("Index");
        }



        //Will Go to Add Feedback form
        [Route("Passengers/Feedback/{pid}/{trip}")]
        [HttpGet]
        public ActionResult Feedback(int pid, int trip)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
            if (pid == 0 || trip == 0)
            {
              /// Errr
            }
            
            Feedback feedback = new Feedback
            {
                PassengerId = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.Id == pid),
                TripId = context.Trips.Include(c => c.Line).Include(c => c.Bus).Include(c => c.Bus.Driver).Single(c => c.Id == trip)
            };
            return View(feedback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Passengers/Feedback/{pid}/{trip}")]
        public ActionResult Feedback(Feedback feedback, int mark)
        {
            var CurrentUser = HttpContext.Session.GetString("CurrentUser");
            User user = context.Users.FirstOrDefault(u => u.Id.Equals(CurrentUser));
            ViewBag.CurrentUser = user;
           

            Passenger SessionPassenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.ApplicationUser.Id == CurrentUser);
            Passenger passenger = context.Passengers.Include(c => c.ApplicationUser).Include(c => c.Tickets).SingleOrDefault(c => c.Id == feedback.PassengerId.Id);
            Trip trip = context.Trips.Single(c => c.Id == feedback.TripId.Id);
            feedback.PassengerId = passenger;

            feedback.TripId = trip;

            if (SessionPassenger != passenger)
            {
                //err
            }

            if (feedback.FeedbackMessage != "")
            {
                context.Feedbacks.Add(feedback);
                context.SaveChanges();
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }




    }
}
