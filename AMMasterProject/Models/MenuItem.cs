using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMasterProject.Models
{
    public class MenuItem
    {
        [Key]
        [Column("MenuId")]
        public int MenuId { get; set; }


        [Column("MenuFor")]
        [StringLength(200)]
        [DisplayName("Menu For")]
      
        [Required(ErrorMessage = "Menu For Is Required")]
        public string MenuFor { get; set; }   ///for admin, customer or seller


        [Column("MenuName")]
        [StringLength(200)]
        [DisplayName("Menu Name")]
        [RegularExpression("^[A-Za-z0-9 ]*$", ErrorMessage = "Only characters, numbers, and spaces are allowed.")]
        [Required(ErrorMessage = "Menu Name Is Required")]
        public string MenuName { get; set; }

        [DisplayName("IsURL")]
        [Required(ErrorMessage = "Is URL Is Required")]
        [DefaultValue(true)]
        public bool IsURL { get; set; }


        [Column("URLPath")]
        [StringLength(500)]
        [DisplayName("URL Path")]


        public string? Urlpath { get; set; }

        [Column("parent_category_id")]
        [DisplayName("Parent Category")]
        [Required(ErrorMessage = "Parent Category Is Required")]
        public int ParentCategoryId { get; set; }

        [Column("Description")]
        [StringLength(200)]
        [DisplayName("Description")]
        public string? Description { get; set; }

        [StringLength(1000)]
        [DisplayName("CssClass")]

        public string? CssClass { get; set; }

    

        [DisplayName("Is Published")]
        [Required(ErrorMessage = "Is Published Is Required")]
        [DefaultValue(true)]
        public bool IsPublished { get; set; }


        [DisplayName("Is Show Home Page")]
        [Required(ErrorMessage = "Is Show Home Page Is Required")]
        [DefaultValue(false)]
        public bool IsShowHomePage { get; set; }


 

       

        [Column("Sortnumber")]
        [DisplayName("Sort Order")]
        [Required(ErrorMessage = "Sort Order Is Required")]
        public int Sortnumber { get; set; }



    }
}
