// See https://aka.ms/new-console-template for more information

using AutoMapper;
using AutoMapper.Console.Models;
using AutoMapper.GeneratedMappingExtensions;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

using Bogus;

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class BenchmarkVsAutoMapper
{
    private List<Track> tracks;
    private MapperConfiguration automapperConfig;

    public BenchmarkVsAutoMapper()
    {
        this.tracks = new List<Track>();
        this.automapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Track, TrackDto>());
    }

    [GlobalSetup]
    public void Setup()
    {
        var testTracks = new Faker<Track>()
            .StrictMode(true)
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Name, f => f.Music.Random.Words(Random.Shared.Next(1, 4)))
            .RuleFor(x => x.Artist, f => f.Name.FullName())
            .RuleFor(x => x.Album, f => f.Lorem.Word())
            .RuleFor(x => x.ReleaseDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .RuleFor(x => x.IsExplicit, true);

        tracks = testTracks.Generate(1000000);
    }

    [Benchmark(Baseline = true)]
    public void MapWithSourceGenerator()
    {
        foreach (var track in this.tracks)
        {
            var mappedTrack = track.AsTrackDto();
        }
    }

    [Benchmark]
    public void MapWithAutoMapper()
    {
        var mapper = this.automapperConfig.CreateMapper();
        foreach (var track in this.tracks)
        {
            var mappedTrack = mapper.Map<TrackDto>(track);
        }
    }
}