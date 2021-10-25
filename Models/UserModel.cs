using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
namespace Mini_Tekstac_Question_Paper_Generation.Models
{
    [Table("UserDetailsTb")]
    public class UserModel
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your name")]
        public string First_Name { get; set; }

        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DoB { get; set; }

        public string Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile number is required")]
        [Display(Name = "Contact Number")]
        public long Contact_Number { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User id is required")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "User Id")]
        public int User_Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password should be minimum of 8 characters")]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { set; get; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password should be minimum of 8 characters")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Security Question1")]
        [Display(Name = "What is your Nickname?")]
        public string SecurityQuestionNickName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Security Question2")]
        [Display(Name = "What is your Best Friend's name?")]
        public string SecurityQuestionFriendName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Security Question3")]
        [Display(Name = "What was your first School name?")]
        public string SecurityQuestionSchoolName { get; set; }

        public ICollection<HelpTickets> IssueTickets { get; set; }

        public ICollection<FeedbackModel>Feedbacks { get; set; }

        public ICollection<AssessmentModel> AssessmentGivenCode { get; set; }


    }
}