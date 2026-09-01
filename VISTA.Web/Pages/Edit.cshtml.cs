using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VISTA.BusinessLogic;
using VISTA.Models;

namespace VISTA.Web.Pages
{
    public class EditModel : PageModel
    {

        // Binds the form data to the VAR object, VAR holds the request, when submitted ASP.net automatically fills
        [BindProperty]
        public VisitorAccessRequest VAR { get; set; } = new();
        

        // Runs when the Edit page is first opened and the id comes frmo the URL and creates an instance of the BLL
        public void OnGet(int id)
        {
            var bll = new VarManager();
            VAR = bll.GetVarById(id);
        }

        // Runs when submit request button clicked
        public IActionResult OnPostSubmit()
        {
            VAR.Status = RequestStatus.Submitted;
            var bll = new VarManager();
            bll.UpdateVar(VAR);
            return RedirectToPage("/Index");
        }

        // Runs when the save changes button clicked
        public IActionResult OnPostSaveChanges()
        {
            VAR.Status = RequestStatus.Draft;
            var bll = new VarManager();
            bll.UpdateVar(VAR);
            return RedirectToPage("/Index");
        }

    }
}