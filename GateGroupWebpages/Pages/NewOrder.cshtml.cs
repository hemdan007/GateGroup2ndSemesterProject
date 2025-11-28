using gategourmetLibrary.Models;
using gategourmetLibrary.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GateGroupWebpages.Pages
{
    public class NewOrderModel : PageModel
    {
        readonly OrderService _os;
        public Order newOrder { get; set; }
        public RecipePart RecipePar { get; set; }

        public List<RecipePart> recipeParts { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        

        public void OnGet()
        {

          
        }
        public NewOrderModel(OrderService os)
        {
            
            _os = os;
            newOrder = new Order();
            RecipePar = new RecipePart();
            recipeParts = new List<RecipePart>();
            Ingredients = _os.GetAllIngredients();
        }

        //public IActionResult OnGet()
        //{
        //    // Tjek om brugeren er logget ind før den giver adgang til siden 
        //    if (HttpContext.Session.GetString("IsLoggedIn") != "true")
        //    {
        //        // Hvis IKKE logget ind - send til login siden
        //        return RedirectToPage("/Login");
        //    }

        //    // Hvis logget ind - vis siden som normalt
        //    return Page();

        //}


        public IActionResult OnPost()
        {
            _os.AddOrder(newOrder);
            return Page();
        }

    }
}
