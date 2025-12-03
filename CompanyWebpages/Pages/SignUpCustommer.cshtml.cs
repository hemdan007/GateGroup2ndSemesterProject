using gategourmetLibrary.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyWebpages.Pages
{
    public class SignUpCustommerModel : PageModel
    {
        private readonly CustomerService _customerService;

        [BindProperty(SupportsGet = true)]
        public int CustomerID { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public SignUpCustommerModel(CustomerService customerService)
        {
            _customerService = customerService;
        }


        public void OnGet()
        {

        }
        public IActionResult OnPost(int customerId, string password, string confirmPassword)
        {
            CustomerID = customerId;
            if (password != confirmPassword)
            {
                ErrorMessage = "Adgangskoderne matcher ikke";
                return Page();
            }

            // Hent kunden fra databasen
            var customer = _customerService.GetCustomer(customerId);
            if (customer == null)
            {
                ErrorMessage = $"Kunde med ID {customerId} findes ikke";
                return Page();
            }
          
            // Opdater kundens password
            customer.Password = password;
            _customerService.UpdateCustomer(customerId, customer);

            SuccessMessage = $"Adgangskode oprettet for kunde ID {customerId}";
            return Page();


        }
    }
}
