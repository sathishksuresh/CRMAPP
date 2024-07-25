using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace CrmApp.Pages.Customers
{
    public class Edit : PageModel
    {

    
        public int Id {get; set;} 

      
    
        [BindProperty]
        public string FirstName {get; set;} 
        

       [BindProperty]
        public string LastName {get; set;} 


        
        public string Email {get; set;} = "";
        
        public string Phone {get; set;}
        
        public string? Address {get; set;} 

        
        public string Company {get; set;} = " ";
        
        public string? Notes {get; set;} 

        public string ErrorMessage {get;set;} = " ";

        public void OnGet(int id)
        {

            try{
                string connectionString = "Server=tcp:crmapp2.database.windows.net,1433;Initial Catalog=crm;Persist Security Info=False;User ID=deployingcrm;Password=Password@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString)){
                    connection.Open();
                    string sql = "SELECT * FROM customers WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection)){
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader()){
                            if(reader.Read())
                            {
                                Id = reader.GetInt32(0);
                                FirstName = reader.GetString(1);
                                LastName = reader.GetString(2);
                                Email = reader.GetString(3);
                                Phone = reader.GetString(4);
                                Address = reader.GetString(5);
                                Company = reader.GetString(6);
                                Notes = reader.GetString(7);
                            }
                            else{
                                Response.Redirect("/Customers/Index");
                            }
                        }
                    }
                }
            }

            catch(Exception ex){
                    ErrorMessage = ex.Message;
                    
            }
        }

        public void OnPost(){
            if(!ModelState.IsValid){
                return;
            }
            if (Phone == null) Phone = " ";
            if (Address == null) Address = " ";
            if (Notes == null) Notes = " ";

            //Update Customers Details

            try{
                string connectionString = "Server=tcp:crmapp2.database.windows.net,1433;Initial Catalog=crm;Persist Security Info=False;User ID=deployingcrm;Password=Password@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString)){
                     connection.Open();
                    string sql = "UPDATE customers SET firstname=@firstname, lastname=@lastname, email=@email, " +
                    "phone=@phone, address=@address, company=@company, notes=@notes WHERE id=@id;";

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