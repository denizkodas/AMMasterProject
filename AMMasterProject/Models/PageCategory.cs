using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("PageCategory")]
public partial class PageCategory
{
    [Key]
    public int PageCategoryId { get; set; }

  
    [StringLength(50)]
    [Column("Category")]
    [DisplayName("Page Category")]
    [Required(ErrorMessage = "Page Category Is Required")]
    public string Category{ get; set; }

    
    [Column("IsPublish")]
    [DisplayName("Is Active")]
   
    [DefaultValue(true)]
    public bool IsPublish { get; set; }

    [Column("ProfileId")]
  
    public int ProfileId { get; set; }
}
