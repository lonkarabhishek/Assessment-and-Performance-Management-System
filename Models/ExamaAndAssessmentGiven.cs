using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
namespace Mini_Tekstac_Question_Paper_Generation.Models
{
    [Table("ExamsAndAssessmentGivenTb")]
    public class ExamaAndAssessmentGiven
    {
        [Key]
        public int ExamsGivenid { get; set; }
        
        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public UserModel UserModel { get; set; }

        public int AssessmentCode { get; set; }
        [ForeignKey("AssessmentCode")]
        public AssessmentModel AssessmentModel { get; set; }

        public int ExamId { get; set; }
        [ForeignKey("ExamId")]
        public ExamDetails ExamDetails { get; set; }

        public bool FeedbackGive { get; set; }

        public DateTime AssessmentGiven { get; set; }

        public int Score { get; set; }

        public float percentageMarks { get; set; }
    }
}