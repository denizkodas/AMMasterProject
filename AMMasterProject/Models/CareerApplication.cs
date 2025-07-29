using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;


namespace AMMasterProject
{
    [Table("CareerApplication")]
    public class CareerApplication
    {

        [Key]
        [Column("CareerApplicationId")]
        public int CareerApplicationId { get; set; }


        [Column("CareerGuid")]
        [Required(ErrorMessage = "CareerGuid Is Required")]
        public Guid CareerGuid { get; set; }

        [Column("ProfileId")]
        public int ProfileId { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime? InsertDate { get; set; }

        public bool IsDeleted { get; set; }


        [Column("ApplicationStatusId")]
        
        public int? ApplicationStatus { get; set; }  //0= applied  1= shortlist 2= hired 3=reject 4= pending

    }
}