using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CrmApp.Pages.Customers
{
    public class Create : PageModel
    {
        [BindProperty, Required(ErrorMessage = "The FirstName is required")]
        public string FirstName {get; set;} = "";
        

        [BindProperty, Required(ErrorMessage = "The LastName is required")]
        public string LastName {get; set;} = "";

        [BindProperty, Required, EmailAddress]
        public string Email {get; set;} = "";
        [BindProperty, Phone]
        public string Phone {get; set;} ="";
        [BindProperty]
        public string? Address {get; set;} 

        [BindProperty, Required]
        public string Company {get; set;} = " ";
        [BindProperty]
        public string? Notes {get; set;} 
        


        public void OnGet()
        {
        }
        public string ErrorMessage {get; set;} = " ";
        public void OnPost()
        {
            if(!ModelState.IsValid){
                return;
            }

            if (Phone == null) Phone = " ";
            if (Address == null) Address = " ";
            if (Notes == null) Notes = " ";

            try{
                string connectionString = "Server=tcp:crmapp2.database.windows.net,1433;Initial Catalog=crm;Persist Security Info=False;User ID=deployingcrm;Password=Password@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString)){
                    connection.Open();
                    string sql = "INSERT into customers " +
                    "(firstname, lastname, email, phone, address, company, notes) VALUES" +
                    "(@firstname, @lastname, @email, @phone, @address, @company, @notes);";

                    using (SqlCommand command = new SqlCommand(sql,connection)){
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@lastname", LastName);
                        command.Parameters.AddWithValue("@email", Email);
                        command.Parameters.AddWithValue("@phone", Phone);
                        command.Parameters.AddWithValue("@address", Address);
                        command.Parameters.AddWithValue("@company", Company);
                        command.Parameters.AddWithValue("@notes", Notes);

                        command.ExecuteNonQuery();
                    }
                }
                    

                    }
                

            catch(Exception ex){
                ErrorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Customers/Index");


        }
    }
}