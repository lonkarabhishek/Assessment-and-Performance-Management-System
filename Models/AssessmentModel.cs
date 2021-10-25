using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
namespace Mini_Tekstac_Question_Paper_Generation.Models
{ 
    [Table("AssessmentTb")]
    public class AssessmentModel
    {
        [Key]
        public int AssessmentCode { get; set; }

        public string AssessmentName { get; set; }

        /*public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public UserModel UserModel { get; set; }*/

        [DataType(DataType.Date)]
        public DateTime AssessmentDate { get; set; }

        public IEnumerable<FeedbackModel>FeedbackOfAssessment { get; set; }

        public IEnumerable<QuestionsAndAnswersSheet> QuestionsAndAnswersSheet { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Assessment Name")]
        [Display(Name = "Skill set")]
        public string SkillSet { get; set; }
    }
}