using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
namespace Mini_Tekstac_Question_Paper_Generation.Models
{
    [Table("QuestionAndAnswersTb")]
    public class QuestionsAndAnswersSheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssessmentQuestionNo { get; set; }

        public int AssessmentCode { get; set; }
        public string AssessmentName { get; set; }
        [ForeignKey("AssessmentCode")]
        public AssessmentModel AssessmentModel { get; set; }

        [Required(ErrorMessage ="* question box cannot be empty")]
        public string Question { get; set; }

        [Required(ErrorMessage = "* option 1 cannot be empty")]
        [Display(Name ="Answer option 1")]
        public string Answer1 { get; set; }

        [Required(ErrorMessage = "* option 2 cannot be empty")]
        [Display(Name = "Answer option 2")]
        public string Answer2 { get; set; }

        [Required(ErrorMessage = "* option 3 cannot be empty ")]
        [Display(Name = "Answer option 3")]
        public string Answer3 { get; set; }

        [Required(ErrorMessage = "* option 4 cannot be empty ")]
        [Display(Name = "Answer option 4")]
        public string Answer4 { get; set; }

        [Required(ErrorMessage = "* correct answer cannot be empty ")]
        [Display(Name = "Correct answer")]
        public string RightAnswer { get; set; }

        [Required(ErrorMessage ="* competency required")]
        [Display(Name ="Competency")]
        public string Competency { get; set; }
    }
}