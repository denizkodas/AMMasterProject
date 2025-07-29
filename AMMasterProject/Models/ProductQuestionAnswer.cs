using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_Question_Answer")]
public partial class ProductQuestionAnswer
{
    [Key]
    public int ProductAnswerId { get; set; }

    [Column("ProductQAId")]
    public int? ProductQaid { get; set; }

    [Column("QAnswer")]
    [StringLength(1000)]
    public string? Qanswer { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    public int? ProfileId { get; set; }

    public bool? IsActive { get; set; }
}
