using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

public partial class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int MesageId { get; set; }

    [Column("ChatID")]
    public Guid ChatId { get; set; }

    [Column("senddate", TypeName = "datetime")]
    public DateTime? Senddate { get; set; }

    [Column("recieveddate", TypeName = "datetime")]
    public DateTime? Recieveddate { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("Message")]
    [StringLength(2000)]
    public string? Message1 { get; set; }

    [StringLength(1000)]
    public string? Attachment { get; set; }

    [Column("senderid")]
    public int Senderid { get; set; }

    [Column("receiverid")]
    public int Receiverid { get; set; }

    [StringLength(200)]
    public string? FileName { get; set; }

    [Column("sample")]
    public string? Sample { get; set; }
}
