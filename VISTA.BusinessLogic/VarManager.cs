using System;
using System.Collections.Generic;
using System.Linq;
using VISTA.DataAccess;
using VISTA.Models;

namespace VISTA.BusinessLogic
{
    public class VarManager
    {
        // DAL dependency used to communicate with the database
        private VarDataAccess dal = new VarDataAccess();

        // Retrieves all VARS for admin users but drafts
        public List<VisitorAccessRequest> GetAllForAdmin()
        {
            var allVars = dal.GetAll(); // Gets all the VARs
            var adminVars = allVars.Where(v => v.Status != RequestStatus.Draft).ToList(); //checks that the VAR isn't a draft and converts back to list
            
            // Checks for only approved VAR's and if its expired then itll set as expired
            foreach(var v in adminVars)
            {
                if(v.Status == RequestStatus.Approved && v.VisitEndDate < DateTime.Today)
                {
                    v.Status = RequestStatus.Expired;
                    dal.UpdateStatus(v.ID, RequestStatus.Expired); // also updates the Data base
                }
            }

            return adminVars;
        }

        // Retrieves all requests belonging to a specific user based on ANumber
        public List<VisitorAccessRequest> GetAllForUser(string aNumber)
        {
            var allvars = dal.GetAll(); // Gets all the VARs
            var userVars = allvars.Where(v => v.SponsorANumber == aNumber).ToList();    // filter to where its only their VARs

            // Checks for each VAR and if its expired then itll set as expired
            foreach (var v in userVars)
            {
                if(v.Status == RequestStatus.Approved && v.VisitEndDate < DateTime.Today)
                {
                    v.Status = RequestStatus.Expired;
                    dal.UpdateStatus(v.ID, RequestStatus.Expired); // also updates the Data base
                }
            }

            return userVars;
        }

        // returns the certain VAR by its ID
        public VisitorAccessRequest GetVarById(int id)
        {
            var certainVar = dal.GetById(id);

            return certainVar; 
        }

        // Creates new VAR and sets default values
        public void CreateVar(VisitorAccessRequest v, string aNumber)
        {
            v.SponsorANumber = aNumber;     // Assigns logged in user Anumber as sponsor

            // prevent null values from breaking data base entry
            v.VisitorName = v.VisitorName ?? string.Empty;
            v.VisitorOrganization = v.VisitorOrganization ?? string.Empty;
            v.SponsorName = v.SponsorName ?? string.Empty;
            v.SponsorEmail = v.SponsorEmail ?? string.Empty;
            v.VisitPurpose = v.VisitPurpose ?? string.Empty;

            if (v.VisitStartDate == DateTime.MinValue)
                v.VisitStartDate = DateTime.Today;
            if(v.VisitEndDate == DateTime.MinValue)
                v.VisitEndDate = DateTime.Today.AddDays(1);

            v.CreatedDate = DateTime.Now;
            v.LastUpdatedDate = DateTime.Now;
            dal.Create(v);
        }
    
        // Sets the last updated date to now and then goes to the specific VAR in the DAL and updates it
        public void UpdateVar(VisitorAccessRequest v)
        {
            v.VisitorName = v.VisitorName ?? string.Empty;
            v.VisitorOrganization = v.VisitorOrganization ?? string.Empty;
            v.SponsorName = v.SponsorName ?? string.Empty;
            v.SponsorEmail = v.SponsorEmail ?? string.Empty;
            v.VisitPurpose = v.VisitPurpose ?? string.Empty;
            v.LastUpdatedDate = DateTime.Now;
            dal.Update(v);
        }

        // Gets the certain VAR by the ID and then if the 
        public void DeleteVar(int id)
        {
            var certainVar = GetVarById(id);
            if(certainVar.Status == RequestStatus.Draft)
            {
                dal.Delete(id);
            }

        }
        
        // Sets the VAR as submitted
        public void SubmitVar(int id)
        {
            dal.UpdateStatus(id, RequestStatus.Submitted);
        }

        // Approves the VAR
        public void ApproveVar(int id)
        {
            var certainVar = GetVarById(id);
            if(certainVar.Status == RequestStatus.Submitted)
            {
                dal.UpdateStatus(id, RequestStatus.Approved);
            }
        }

        // Denys the VAR
        public void DenyVar(int id)
        {
            var certainVar = GetVarById(id);
            if (certainVar.Status == RequestStatus.Submitted)
            {
                dal.UpdateStatus(id, RequestStatus.Denied);
            }
        }

    }
}
