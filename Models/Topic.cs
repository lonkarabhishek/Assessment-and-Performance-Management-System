using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_Tekstac_Question_Paper_Generation.Models
{
    [Table("TopicsTb")]
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TopicName { get; set; }
    }
}