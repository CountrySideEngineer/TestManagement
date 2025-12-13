using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestManagement.Analyze.APP.Entities;

[Index("ResultId", Name = "IX_Requests_ResultId")]
[Index("StatusId", Name = "IX_Requests_StatusId")]
public partial class Request
{
    [Key]
    public int Id { get; set; }

    public string DirectoryPath { get; set; } = null!;

    public int StatusId { get; set; }

    public int ResultId { get; set; }

    public DateTime RegisteredAt { get; set; }

    public DateTime UpdateAt { get; set; }

    [ForeignKey("ResultId")]
    [InverseProperty("Requests")]
    public virtual ResultMaster Result { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Requests")]
    public virtual StatusMaster Status { get; set; } = null!;
}
