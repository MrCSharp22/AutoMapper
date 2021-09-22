using AutoMapper.Common;

namespace AutoMapper.Console.Models;

[MapsTo(typeof(TrackDto))]
public class Track
{
    public Guid Id {  get; set; }

    public string? Name { get; set; }

    public string? Album { get; set; }

    public string? Artist { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public bool IsExplicit { get; set; }
}