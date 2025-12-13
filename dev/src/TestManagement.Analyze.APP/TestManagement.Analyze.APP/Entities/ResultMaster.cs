using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestManagement.Analyze.APP.Entities;

enum RESULT {
    UNKNOWN = 1,
    PASSED = 2,
    FAILED = 3
};

public partial class ResultMaster
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [InverseProperty("Result")]
    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
