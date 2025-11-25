using gategourmetLibrary.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GateGroupWebpages.Pages
{
    public class NewOrderModel : PageModel
    {
        readonly OrderService _os;
        public void OnGet()
        {
        }
    }
}
