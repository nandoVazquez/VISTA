using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VISTA.BusinessLogic;
using VISTA.Models;


namespace VISTA.Web.Pages
{
    public class CreateModel : PageModel
    {

        [BindProperty]
        public VisitorAccessRequest VAR { get; set; } = new();
        public void OnGet()
        {}

        // Runs when Submit button is clicked
        public IActionResult OnPostSubmit()
        {
            string aNumber = HttpContext.Session.GetString("ANumber");
            
            VAR.Status = RequestStatus.Submitted;
            var bll = new VarManager();
            bll.CreateVar(VAR, aNumber);
            return RedirectToPage("/Index");
        }

        // Runs when Draft button is clicked 
        public IActionResult OnPostDraft()
        {

            if ((VAR.VisitorName?.Length ?? 0) > 100 ||
            (VAR.VisitorOrganization?.Length ?? 0) > 100 ||
            (VAR.SponsorName?.Length ?? 0) > 100 ||
            (VAR.SponsorEmail?.Length ?? 0) > 100 ||
            (VAR.VisitPurpose?.Length ?? 0) > 500)
            {
                return Page();
            }

            string aNumber = HttpContext.Session.GetString("ANumber");
            VAR.Status = RequestStatus.Draft;
            var bll = new VarManager();
            bll.CreateVar(VAR, aNumber);
            return RedirectToPage("/Index");
        }
    }
}