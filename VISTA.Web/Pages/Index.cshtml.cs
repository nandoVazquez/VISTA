using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VISTA.BusinessLogic;
using VISTA.Models;

namespace VISTA.Web.Pages;

public class IndexModel : PageModel
{

    // Stores all VARS that will be displayed on the dashboard
    public List<VisitorAccessRequest> VarList { get; set; } = new();

    // Stores current selected status filter
    public string SelectedStatus { get; set; } = "";
    public string SearchTerm { get; set; } = "";
    public int DraftCount { get; set; }
    public int SubmittedCount { get; set; }
    public int ApprovedCount { get; set; }
    public int DeniedCount { get; set; }
    public int ExpiredCount { get; set; }

    // Runs when the Dashboard page is first loaded
    public IActionResult OnGet(string status, string search)
    {

        SelectedStatus = status ?? "";
        SearchTerm = search ?? "";

        if (HttpContext.Session.GetString("Role") == null)
            return RedirectToPage("/Login");

        string role = HttpContext.Session.GetString("Role");
        string aNumber = HttpContext.Session.GetString("ANumber");

        var bll = new VarManager();
        List<VisitorAccessRequest> rawList;

        if (role == "User")
            rawList = bll.GetAllForUser(aNumber);
        
        else
            rawList =  bll.GetAllForAdmin();

        DraftCount = 0;
        SubmittedCount = 0;
        ApprovedCount = 0;
        DeniedCount = 0;
        ExpiredCount = 0;

        foreach (var v in rawList)
        {
            if (v.Status == RequestStatus.Draft) DraftCount++;
            else if (v.Status == RequestStatus.Submitted) SubmittedCount++;
            else if (v.Status == RequestStatus.Approved) ApprovedCount++;
            else if (v.Status == RequestStatus.Denied) DeniedCount++;
            else if (v.Status == RequestStatus.Expired) ExpiredCount++;
        }

        VarList = new List<VisitorAccessRequest>();

        foreach (var v in rawList)
        {
            if (!string.IsNullOrEmpty(status) && v.Status.ToString() != status)
            {
                continue; 
            }

            if (!string.IsNullOrEmpty(search))
            {
                bool matchesVisitor = v.VisitorName != null && v.VisitorName.Contains(search, StringComparison.OrdinalIgnoreCase);
                bool matchesSponsor = v.SponsorName != null && v.SponsorName.Contains(search, StringComparison.OrdinalIgnoreCase);
                bool matchesStatus = v.Status.ToString().Contains(search, StringComparison.OrdinalIgnoreCase);

                if (!matchesVisitor && !matchesSponsor && !matchesStatus)
                {
                    continue; 
                }
            }

            VarList.Add(v);
        }

        return Page();
    }


    
    // runs when Delete button clicked
    public IActionResult OnPostDelete(int id)
    {
        var bll = new VarManager();
        bll.DeleteVar(id);
        return RedirectToPage("/Index");
    }


}