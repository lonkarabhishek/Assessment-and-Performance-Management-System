using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
namespace Mini_Tekstac_Question_Paper_Generation.Models
{
    [Table("FeedbackTb")]
    public class FeedbackModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackId { get; set; }

        public int User_Id { get; set; }

        [ForeignKey("User_Id")]
        public UserModel UserModel { get; set; }

        public int AssessmentCode { get; set; }
        [ForeignKey("AssessmentCode")]
        public AssessmentModel AssessmentModel { get; set; }

        public int QuestionNo { get; set; }
        [ForeignKey("QuestionNo")]
        
        public FBQuestions FBQuestions { get; set; }

        public string Feedback { get; set; }

        public DateTime feedbackGivenDate { get; set; }
    }
}