namespace AutoMapper.Console.Models;

public class TrackDto
{
    public string? Name { get; set; }

    public string? Album { get; set; }

    public string? Artist { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public bool IsExplicit { get; set; }
}