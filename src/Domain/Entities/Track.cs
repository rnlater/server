using System;
using Domain.Base;

namespace Domain.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

public class Track : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public ICollection<Subject> Subjects { get; set; } = [];

    public static Track ForTestPurposeOnly()
    {
        return new Track
        {
            Id = Guid.NewGuid(),
            Name = "Test Track",
            Description = "Test Description",
        };
    }
}
