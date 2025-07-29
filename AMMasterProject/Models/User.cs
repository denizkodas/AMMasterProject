using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(256)]
    public string Username { get; set; } = null!;

    [StringLength(2000)]
    public string Password { get; set; } = null!;

    [StringLength(256)]
    public string Email { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLoginDate { get; set; }

    [Column("GUID")]
    public Guid Guid { get; set; }

    [Column("registertype")]
    [StringLength(50)]
    public string? Registertype { get; set; }

    [Column("logintype")]
    [StringLength(50)]
    public string? Logintype { get; set; }

    [Column("loginchannel")]
    [StringLength(50)]
    public string? Loginchannel { get; set; }
}
