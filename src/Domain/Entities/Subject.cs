using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities;

public class Subject : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public ICollection<Track> Tracks { get; set; } = [];

    public ICollection<Knowledge> Knowledges { get; set; } = [];

    public static Subject ForTestPurposeOnly()
    {
        return new Subject
        {
            Id = Guid.NewGuid(),
            Name = "Test Subject",
            Description = "Test Description",
        };
    }
}
