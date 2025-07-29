using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace AMMasterProject.Models
{
    public class MessageMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MesageMasterId { get; set; }

      
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ChatId { get; set; }

        [Required]
        public int senderid { get; set; }

        [Required]
        public int receiverid { get; set; }

        //public MessageMaster()
        //{
        //    ChatId = Guid.NewGuid();
        //}
    }
}
