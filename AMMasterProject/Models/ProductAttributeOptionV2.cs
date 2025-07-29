using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_AttributeOption_V2")]
public partial class ProductAttributeOptionV2
{
    [Key]
    public int ProductAttributeOptionId { get; set; }

    [Column("ProductAttributeGUID")]
    public Guid? ProductAttributeGuid { get; set; }

    [StringLength(1000)]
    [DisplayName("Option")]
    [Required(ErrorMessage = "Option Is Required")]
    public string OptionText { get; set; }

    public bool? IsCorrect { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    [Column("attributeprice", TypeName = "decimal(18, 2)")]
    [DisplayName("Price")]
    [Required(ErrorMessage = "Price Is Required")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Price must be a number")]
    public decimal Attributeprice { get; set; }

    [Column("attributeimage")]
    [StringLength(1000)]
    public string? Attributeimage { get; set; }

    [Column("Sort")]
    
    public int Sort { get; set; }

}
