using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Keyless]
[Table("Alphabet")]
public partial class Alphabet
{
    public int Alphabetid { get; set; }

    [Column("Alphabet")]
    [StringLength(5)]
    public string? Alphabet1 { get; set; }

    [StringLength(50)]
    public string? Image { get; set; }
}
