using BusSchool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BusSchool.Controllers
{
    public class AccountController : Controller
    {
        
       
        private BusSchoolContext context = new BusSchoolContext();
        public readonly IWebHostEnvironment _webHost;

        public AccountController(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public IActionResult Login()
        {
            
            string EmailCookie = Request.Cookies["MyCookie"];
            bool check = false;
            if (EmailCookie != null)
            {
                check = true;
                var userCookie = context.Users.SingleOrDefault(c => c.Email == EmailCookie);               
            ViewBag.EmailCookie = userCookie;
            ViewBag.check = check;
           
               
            }
           
            return View();
            
                    
        }

        [HttpPost]
        public IActionResult Login(string email, string pass,bool checkRemember)
        {
            if (checkRemember)
            {
                var cookieOptions1 = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(1)
                };
                Response.Cookies.Append("MyCookie", email, cookieOptions1);
            } else
            {
                HttpContext.Response.Cookies.Delete("MyCookie");
            }
            
            var authenticatedUser = context.Users.FirstOrDefault(u => u.Email == email && u.Password == pass);

            if (authenticatedUser != null )
            {
               
                HttpContext.Session.SetString("CurrentUser", authenticatedUser.Id);

                
                return RedirectToAction("Index", "Home");
            }
           // else if(authenticatedUser != null && authenticatedUser.Role == 1)
           // {
            //    HttpContext.Session.SetString("CurrentUser", authenticatedUser.Id);


              //  return RedirectToAction("Index", "Admin");
           // } else
            {              
                ViewBag.Message = "Wrong username or password";
                return View("Login");
            }
        }

        public IActionResult Register()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(User user, string confirm,IFormFile profilepicture)
        {
            string message = "";
            string lastId = context.Users.OrderByDescending(u => u.Id).FirstOrDefault()?.Id;
            int newId = int.Parse(lastId.Substring(1)) + 1;
            string newUserId = "U" + newId;

            if (ModelState.IsValid)
            {
                string picture = "";

                if (profilepicture != null)
                {
                    string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image","picture_user");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string fileName = Path.GetFileName(profilepicture.FileName);
                    string fileSavePath = Path.Combine(uploadsFolder, fileName);
                    using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                    {
                        await profilepicture.CopyToAsync(stream);
                    }
                    picture = fileName;
                }
                string password = user.Password;
                var getUser = context.Users.FirstOrDefault(u => u.Email == user.Email);

                if (!password.Equals(confirm) || getUser != null)
                {

                    message = "Confirm and Password are not matched or Email used!!";
                    ViewBag.Error = message;
                    return View(user);
                }
                else
                {
                    string email = user.Email;
                    string fullname = user.UserName;
                    int role = 0; // User
                    
                    try
                    {
                        User newUser = new User()
                        {
                            Id = newUserId,
                            UserName = fullname,
                            Ssn = user.Ssn,
                            PhoneNumber = user.PhoneNumber,
                            Email = email,
                            Password = password,
                            Role = role,
                            ProfilePicture = picture
                        };

                        context.Users.Add(newUser);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        ViewBag.Error = message;
                        return View(user);
                    }
                }
            }
            else
            {
                ViewBag.Error = message;
                return View(user);
            }

            TempData["SignUp"] = "Sign up successfully! You can login now!";
            return RedirectToAction("Login", "Account");

        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("CurrentUser");
            return RedirectToAction("Index", "Home");

        }

    }
}

