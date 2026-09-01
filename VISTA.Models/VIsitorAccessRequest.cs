using System.ComponentModel.DataAnnotations; // imports validation attributes
namespace VISTA.Models{

// Plain data definitions shared by every layer
// No logic only describes the shape of a vistor access request
// everything public not methods just data get and set
    public class VisitorAccessRequest
    {

        // [Required] used to tell ASP.NET that a value must be required, cannot be empty


        public int ID {get; set;}
        [Required]
        public string VisitorName {get; set;} = string.Empty;
        [Required]
        public string VisitorOrganization {get; set;} = string.Empty;
        [Required]
        public string SponsorName {get; set;} = string.Empty;
        [Required]
        public string SponsorEmail {get; set;} = string.Empty;  
        [Required]
        public string SponsorANumber { get; set; } = string.Empty;
        [Required]
        public string VisitPurpose {get; set;} = string.Empty;
        public System.DateTime VisitStartDate {get; set;}
        public System.DateTime VisitEndDate {get; set;}
        public RequestStatus Status {get; set;}
        public System.DateTime CreatedDate {get; set;}
        public System.DateTime LastUpdatedDate {get; set;}

        // displays the status badge in the ui with its color
        public string StatusBadgeClass => Status switch
        {
            RequestStatus.Draft     => "badge-draft",
            RequestStatus.Submitted => "badge-submitted",
            RequestStatus.Approved  => "badge-approved",
            RequestStatus.Denied    => "badge-denied",
            RequestStatus.Expired   => "badge-expired",
                _                       => "badge-secondary"
        };
    }
}
