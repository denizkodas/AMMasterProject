using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;



namespace AMMasterProject
{

    [Table("CareerCategory")]
    public class CareerCategory
    {

        [Key]
        [Column("CareerCategoryId")]
        public int CareerCategoryId { get; set; }


        [Column("CareerCategoryName")]
        [StringLength(100)]
        [DisplayName("Category")]
        [Required(ErrorMessage = "Category Is Required")]
        public string CareerCategoryName { get; set; }


        public bool IsPublish { get; set; }

        [Column("ProfileId")]
        public int ProfileId { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime? InsertDate { get; set; }
    }
}