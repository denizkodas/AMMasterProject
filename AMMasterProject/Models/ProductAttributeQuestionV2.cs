using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_AttributeQuestion_V2")]
public partial class ProductAttributeQuestionV2
{
    [Key]
    public int ProductAttributeId { get; set; }

    [Column("ProductAttributeGUID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? ProductAttributeGuid { get; set; }

    [Column("ProductGUID")]
    public Guid? ProductGuid { get; set; }

    [StringLength(100)]
    [DisplayName("Question")]
    [Required(ErrorMessage = "Question Is Required")]
    public string Question { get; set; }

    [StringLength(20)]
    [DisplayName("Control Type")]
    [Required(ErrorMessage = "Control Type Is Required")]
    public string Type { get; set; }


    [DisplayName("Sort Order")]
    [Required(ErrorMessage = "Sort Order Is Required")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Sort Order must be a number")]
    public int SortNumber { get; set; }


    [DefaultValue(true)]
    [Required(ErrorMessage = "Is Publish Required")]
    public bool IsPublish { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    public int ProfileId { get; set; }
}
