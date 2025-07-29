using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;


namespace AMMasterProject
{
    [Table("EventCategory")]
    public class EventCategory
    {
        [Key]
        [Column("EventCategoryId")]
        public int EventCategoryId { get; set; }


        [Column("EventCategoryName")]
        [StringLength(100)]
        [DisplayName("Category")]
        [Required(ErrorMessage = "Category Is Required")]
        public string EventCategoryName { get; set; }


        public bool IsPublish { get; set; }

        [Column("ProfileId")]
        public int ProfileId { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime? InsertDate { get; set; }
    }
}