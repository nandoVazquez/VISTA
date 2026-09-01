using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VISTA.Models;

namespace VISTA.Web.Pages
{
    public class LoginModel : PageModel
    {

        public string? ErrorMessage { get; set; }

        public void OnGet()
        { }

        public IActionResult OnPost(string aNumber) 
        {
            string aUser = "0001";
            string aAdmin = "0002";

            if (aNumber == aUser)
            {
                HttpContext.Session.SetString("Role", "User");
                HttpContext.Session.SetString("ANumber", aNumber);
                return RedirectToPage("/Index");
            }
            else if (aNumber == aAdmin)
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("ANumber", aNumber);
                return RedirectToPage("/Index");
            }
            else 
            {
                ErrorMessage = "Invalid A#. Please Try Again.";
                return Page();
            }

        }

        
    }
}
