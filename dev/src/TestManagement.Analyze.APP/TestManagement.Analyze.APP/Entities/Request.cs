using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestManagement.Analyze.APP.Entities;

[Index("ResultId", Name = "IX_Requests_ResultId")]
[Index("StatusId", Name = "IX_Requests_StatusId")]
/// <summary>
/// Represents an analysis request which tracks a test directory, its execution level,
/// status and result along with timestamps for registration and updates.
/// </summary>
public partial class Request
{
    /// <summary>
    /// Primary key identifier for the request.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The filesystem path to the directory containing tests to be analyzed.
    /// </summary>
    [Required]
    public string DirectoryPath { get; set; } = null!;

    /// <summary>
    /// Identifier indicating the test level for this request.
    /// </summary>
    [Required]
    public int TestLevelId { get; set; } = 0;

    /// <summary>
    /// Foreign key referencing the current status of the request.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Foreign key referencing the result master entry for this request.
    /// </summary>
    public int ResultId { get; set; }

    /// <summary>
    /// UTC date and time when the request was registered.
    /// </summary>
    public DateTime RegisteredAt { get; set; }

    /// <summary>
    /// UTC date and time when the request was last updated.
    /// </summary>
    public DateTime UpdateAt { get; set; }

    /// <summary>
    /// Navigation property to the related result master entity.
    /// </summary>
    [ForeignKey("ResultId")]
    [InverseProperty("Requests")]
    public virtual ResultMaster Result { get; set; } = null!;

    /// <summary>
    /// Navigation property to the related status master entity.
    /// </summary>
    [ForeignKey("StatusId")]
    [InverseProperty("Requests")]
    public virtual StatusMaster Status { get; set; } = null!;
}
