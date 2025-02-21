using System;

namespace Dsm.Models;

public class MusicTrack
{
    public string FilePath { get; set; }
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }
    public string CoverArtPath { get; set; }
    public string Album { get; set; }
}