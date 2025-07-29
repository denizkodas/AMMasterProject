using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject
{

    [Table("WebsiteSetup_FAQ")]
    public class FAQ
    {


        [Key]
        [Column("FAQId")]
        public int FAQId { get; set; }



        [Column("Question")]
        [StringLength(200)]
        [DisplayName("Question")]
        [Required(ErrorMessage = "Question Is Required")]
        public string Question { get; set; }


        [Column("Answer")]
        [StringLength(1000)]
        [DisplayName("Answer")]
        [Required(ErrorMessage = "Answer Is Required")]
        public string Answer { get; set; }


        [Column(TypeName = "date")]
        public DateTime? Insertdate { get; set; }

        public bool IsPublish { get; set; }

        [Column("ProfileID")]
        public int ProfileId { get; set; }

        [Column("sortorder")]
        [DisplayName("Sort Order")]
        [Required(ErrorMessage = "Sort Order Is Required")]
        public int Sortorder { get; set; }

    }
}