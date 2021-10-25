using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Mini_Tekstac_Question_Paper_Generation.Models
{

    [Table("Fb_Questions")]
    public class FBQuestions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionNo { get; set; }

        [Display(Name = "Add Question")]
        [Required(ErrorMessage ="Please type in a question")]
        public string Question { get; set; }
    }

}