using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;




using System.ComponentModel.DataAnnotations.Schema;

namespace AMMasterProject
{

    [Table("Websitesetup_Team")]
    public class WebsitesetupTeam
    {

        [Key]
        [Column("TeamID")]
        public int TeamId { get; set; }


        [Column("Image")]
        [StringLength(1000)]
        [DisplayName("Image")]
        [Required(ErrorMessage = "Image Is Required")]
        public string Image { get; set; }


        [Column("Name")]
        [StringLength(200)]
        [DisplayName("Parner Name")]
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }



        [Column("Designation")]
        [StringLength(200)]
        [DisplayName("Designation")]
        [Required(ErrorMessage = "Designation Is Required")]
        public string Designation { get; set; }




        [Column("Summary")]
        [StringLength(1000)]
        [DisplayName("Summary")]
        [Required(ErrorMessage = "Summary Is Required")]
        public string Summary { get; set; }


        [Column("Description")]

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description Is Required")]

        public string Description { get; set; }


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