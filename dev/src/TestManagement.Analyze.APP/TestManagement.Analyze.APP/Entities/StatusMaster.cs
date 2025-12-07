using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestManagement.Analyze.APP.Entities;

public enum STATUS
{
    NOT_STARTED = 1,
    RUNNING = 2,
    COMPLETED = 3
};

public partial class StatusMaster
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
