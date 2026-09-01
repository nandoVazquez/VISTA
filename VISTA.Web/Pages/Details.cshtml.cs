using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VISTA.BusinessLogic;
using VISTA.Models;

namespace VISTA.Web.Pages
{
    public class DetailsModel : PageModel
    {

        // Holds the specific VAR that will be displayed
        public VisitorAccessRequest VAR { get; set; } = new();

        // Runs when the Details page is first opened and gets the ID to create a BLL instance
        public void OnGet(int id)
        {
            var bll = new VarManager();
            VAR = bll.GetVarById(id);
        }

        // Runs when the Approve button is clicked
        public IActionResult OnPostApprove(int id)
        {
            var bll = new VarManager();
            bll.ApproveVar(id);
            return RedirectToPage("/Index");
        }

        // Runs when the Deny button is clicked
        public IActionResult OnPostDeny(int id)
        {
            var bll = new VarManager();
            bll.DenyVar(id);
            return RedirectToPage("/Index");
        }
    }
}