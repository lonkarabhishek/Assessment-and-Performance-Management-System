using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
namespace Mini_Tekstac_Question_Paper_Generation.Models
{
    [Table("Admin_CredentialTb")]
    public class AdminModel
    {
        [Key]
        [Display(Name = "Admin Username")]
        [Required(ErrorMessage = "Enter admin username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Enter admin password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}