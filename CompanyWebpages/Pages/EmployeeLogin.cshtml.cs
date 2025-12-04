using gategourmetLibrary.Models;
using gategourmetLibrary.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyWebpages.Pages
{
    public class EmployeeLoginModel : PageModel
    {
        readonly EmployeeService _cs;
        [BindProperty]
        public string UserID { get; set; }

        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string ErrorMessage { get; set; }


        public IActionResult OnGet()
        {
            // når man logger ud og ikke mere har adgang til siden (Ordre)
            HttpContext.Session.Remove("IsLoggedIn");
            return Page();

        }
        public EmployeeLoginModel(EmployeeService cs)
        {
            _cs = cs;

        }

        public IActionResult OnPost()
        {
            Employee employ = _cs.Get(Convert.ToInt32(UserID));
            if (Password == employ.Password && UserID != null)
            {
                HttpContext.Session.SetString("IsLoggedIn", "true"); // Gem i session
                HttpContext.Session.SetString("username", $"{employ.Name}"); // Gem i session
                HttpContext.Session.SetString("userid", $"{employ.Id}");



                return RedirectToPage("/Dashboard");
                //return RedirectToPage("/NewOrder");

            }

            // Hvis password IKKE er korrekt
            ErrorMessage = "Incorrect password, please try again.";
            return Page();

        }




    }
    
}
