using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using VISTA.Models;

namespace VISTA.DataAccess
{
    public class VarDataAccess
    {
        // Connection string used to connect to the SQL server
        private readonly string conString;

        public VarDataAccess()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<VarDataAccess>()
                .Build();

            conString = config.GetConnectionString("VistaDb");
        }
        // Retrieves every VAR from the database
        public List<VisitorAccessRequest> GetAll()
        {
            var list = new List<VisitorAccessRequest>();

            using (SqlConnection con = new SqlConnection(conString))                            // Creates a database connection
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM VisitorAccessRequest", con);     // SQL query to retreieve all requests

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();                                        // Execute the query and read the returned rows
                
                // Convert each database row into a VAR object
                while (rdr.Read())
                {
                    var v = new VisitorAccessRequest();


                    v.ID = rdr.GetInt32(0);
                    v.Status = Enum.Parse<RequestStatus>(rdr["Status"].ToString());
                    v.VisitorName = rdr["VisitorName"].ToString();
                    v.VisitorOrganization = rdr["VisitorOrganization"].ToString();
                    v.SponsorName = rdr["SponsorName"].ToString();
                    v.SponsorEmail = rdr["SponsorEmail"].ToString();
                    v.VisitPurpose = rdr["VisitPurpose"].ToString();
                    v.VisitStartDate = Convert.ToDateTime(rdr["VisitStartDate"]);
                    v.VisitEndDate = Convert.ToDateTime(rdr["VisitEndDate"]);
                    v.CreatedDate = Convert.ToDateTime(rdr["CreatedDate"]);
                    v.LastUpdatedDate = Convert.ToDateTime(rdr["LastUpdatedDate"]);
                    v.SponsorANumber = rdr["SponsorANumber"].ToString();

                    list.Add(v);
                }
            }
            return list;
        }

        // Retrieves a single VAR using its ID
        public VisitorAccessRequest GetById(int id)
        {
            VisitorAccessRequest v = new VisitorAccessRequest();

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM VisitorAccessRequest WHERE ID = @id", con); // SQL query to find one by its ID
                cmd.Parameters.AddWithValue("@id", id); // Safe ID placeholder parameter
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                // Read the single matched row, if found
                while (rdr.Read())
                {
                    v.ID = rdr.GetInt32(0);
                    v.Status = Enum.Parse<RequestStatus>(rdr["Status"].ToString());
                    v.VisitorName = rdr["VisitorName"].ToString();
                    v.VisitorOrganization = rdr["VisitorOrganization"].ToString();
                    v.SponsorName = rdr["SponsorName"].ToString();
                    v.SponsorEmail = rdr["SponsorEmail"].ToString();
                    v.VisitPurpose = rdr["VisitPurpose"].ToString();
                    v.VisitStartDate = Convert.ToDateTime(rdr["VisitStartDate"]);
                    v.VisitEndDate = Convert.ToDateTime(rdr["VisitEndDate"]);
                    v.CreatedDate = Convert.ToDateTime(rdr["CreatedDate"]);
                    v.LastUpdatedDate = Convert.ToDateTime(rdr["LastUpdatedDate"]);
                    v.SponsorANumber = rdr["SponsorANumber"].ToString();

                }
            }
            return v;
        }

        // Inserts a new VAR into the database
        public void Create(VisitorAccessRequest v)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO VisitorAccessRequest (Status, VisitorName, VisitorOrganization, SponsorName, SponsorEmail, VisitPurpose, VisitStartDate, VisitEndDate, CreatedDate, LastUpdatedDate, SponsorANumber) VALUES (@Status, @VisitorName, @VisitorOrganization, @SponsorName, @SponsorEmail, @VisitPurpose, @VisitStartDate, @VisitEndDate, @CreatedDate, @LastUpdatedDate, @SponsorANumber)", con);

                // Map object properties to the SQL paramters
                cmd.Parameters.AddWithValue("@Status", v.Status.ToString());
                cmd.Parameters.AddWithValue("@VisitorName", v.VisitorName);
                cmd.Parameters.AddWithValue("@VisitorOrganization", v.VisitorOrganization);
                cmd.Parameters.AddWithValue("@SponsorName", v.SponsorName);
                cmd.Parameters.AddWithValue("@SponsorEmail", v.SponsorEmail);
                cmd.Parameters.AddWithValue("@VisitPurpose", v.VisitPurpose);
                cmd.Parameters.AddWithValue("@VisitStartDate", v.VisitStartDate);
                cmd.Parameters.AddWithValue("@VisitEndDate", v.VisitEndDate);
                cmd.Parameters.AddWithValue("@CreatedDate", v.CreatedDate);
                cmd.Parameters.AddWithValue("@LastUpdatedDate", v.LastUpdatedDate);
                // Add SponsorANumber to the INSERT SQL columns and VALUES
                cmd.Parameters.AddWithValue("@SponsorANumber", v.SponsorANumber);

                con.Open();

                cmd.ExecuteNonQuery(); // Execute the insert order immediately
            }
        }

        // Updates and existing VAR
        public void Update(VisitorAccessRequest v)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("Update VisitorAccessRequest SET Status = @Status, VisitorName = @VisitorName, VisitorOrganization = @VisitorOrganization, SponsorName = @SponsorName, SponsorEmail = @SponsorEmail, VisitPurpose = @VisitPurpose, VisitStartDate = @VisitStartDate, VisitEndDate = @VisitEndDate, CreatedDate = @CreatedDate, LastUpdatedDate = @LastUpdatedDate, SponsorANumber = @SponsorANumber WHERE ID = @id", con);

                // Safely pass properties, targeting the unique row by ID
                cmd.Parameters.AddWithValue("@ID", v.ID);
                cmd.Parameters.AddWithValue("@Status", v.Status.ToString());
                cmd.Parameters.AddWithValue("@VisitorName", v.VisitorName);
                cmd.Parameters.AddWithValue("@VisitorOrganization", v.VisitorOrganization);
                cmd.Parameters.AddWithValue("@SponsorName", v.SponsorName);
                cmd.Parameters.AddWithValue("@SponsorEmail", v.SponsorEmail);
                cmd.Parameters.AddWithValue("@VisitPurpose", v.VisitPurpose);
                cmd.Parameters.AddWithValue("@VisitStartDate", v.VisitStartDate);
                cmd.Parameters.AddWithValue("@VisitEndDate", v.VisitEndDate);
                cmd.Parameters.AddWithValue("@CreatedDate", v.CreatedDate);
                cmd.Parameters.AddWithValue("@LastUpdatedDate", v.LastUpdatedDate);
                cmd.Parameters.AddWithValue("@SponsorANumber", v.SponsorANumber);


                con.Open();
                cmd.ExecuteNonQuery(); // Execute the update order immediately
            }
        }

        // Deletes the VAR using its ID
        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM VisitorAccessRequest WHERE ID = @id", con);

                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery(); // Execute the deletion order immediately
            }
        }

        // Updates only the status and last updated date
        public void UpdateStatus(int id, RequestStatus status)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE VisitorAccessRequest SET STATUS = @Status, LastUpdatedDate = @LastUpdatedDate WHERE ID = @id", con);

                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Status", status.ToString());
                cmd.Parameters.AddWithValue("@LastUpdatedDate", DateTime.Now); // Automatically apply current time

                con.Open();
                cmd.ExecuteNonQuery(); // Execute the state modification order immediately
            }
        }
    }
}