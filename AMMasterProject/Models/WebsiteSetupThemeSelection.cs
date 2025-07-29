using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Website_Setup_ThemeSelection")]
public partial class WebsiteSetupThemeSelection
{
    [Key]
    public int WebsiteThemeSelectionId { get; set; }

    [StringLength(50)]
    public string? ThemeName { get; set; }

    [Column("ThemeCSS")]
    [StringLength(50)]
    public string? ThemeCss { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }
}
