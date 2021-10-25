using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Mini_Tekstac_Question_Paper_Generation.Models
{
    
    [Table("Help_Tickets")]
    public class HelpTickets
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set;  }

        [DataType(DataType.Date)]
        public DateTime Date_of_Ticket { get; set; }

        public string TicketStatus { get; set; }

        [Required(ErrorMessage = "Please give title to your issue")]
        public string Issue { get; set; }

        [Required(ErrorMessage ="Please write description of your issue")]
        public string Description { set; get; }

        public int User_Id { get; set; }
        [ForeignKey("User_Id")]

        public UserModel UserModel { get; set; }
        
      
        public string Resolve { get; set; }

    }
}