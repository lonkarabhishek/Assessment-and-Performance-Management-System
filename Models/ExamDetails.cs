using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mini_Tekstac_Question_Paper_Generation.Models
{
    [Table("ExamDetailsTb")]
    public class ExamDetails
    {
        [Key]
        public int ExamId { get; set; }
        public int AssessmentCode { get; set; }
        [ForeignKey("AssessmentCode")]
        public AssessmentModel AssessmentModel { get; set; }

        [Display(Name = "Exam Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Exam Name")]
        public string ExamName { get; set; }

        [Display(Name = "Date of Exam")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateOfExam { get; set; }

        [Required(ErrorMessage = "* competency required")]
        [Display(Name = "Competency")]
        public string Competency { get; set; }

    }
}