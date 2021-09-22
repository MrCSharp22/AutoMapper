// See https://aka.ms/new-console-template for more information

using AutoMapper.Console.Models;
using AutoMapper.GeneratedMappingExtensions;

using BenchmarkDotNet.Running;

Console.WriteLine("One to one mapping: Track ==> TrackDto");

var track = new Track
{
    Id = Guid.NewGuid(),
    Name = "Lying in State",
    Album = "Dystopia",
    Artist = "Megadeth",
    IsExplicit = false,
    ReleaseDate = new DateOnly(2016, 01, 22)
};

//var trackDto = AutoMapper.GeneratedMappers.TrackToTrackDtoMapper.AsTrackDto(track);
var trackDto = track.AsTrackDto();

Console.WriteLine($"Mapping result: Now playing: '{trackDto.Name}' by '{trackDto.Artist}' from the album '{trackDto.Album}' released on '{trackDto.ReleaseDate}'");
Console.ReadKey();

Console.WriteLine("Benchmarking vs AutoMapper:");
BenchmarkRunner.Run<BenchmarkVsAutoMapper>();

Console.ReadLine();