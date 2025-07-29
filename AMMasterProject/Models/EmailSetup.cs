using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Email_Setup")]
public partial class EmailSetup
{
    [Key]
    public int EmailSettingId { get; set; }

    [StringLength(256)]
    public string? EmailFrom { get; set; }

    [StringLength(300)]
    public string? Password { get; set; }

    [Column("SMTPPort")]
    public int? Smtpport { get; set; }

    [StringLength(500)]
    public string? Host { get; set; }

    [Column("BCC")]
    [StringLength(256)]
    public string? Bcc { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    public bool? Enablessl { get; set; }
}
