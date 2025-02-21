using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dsm.Models;

namespace Dsm.Loaders;

public class MusicLoader
{
    private readonly PlayerConfig _config;

    public MusicLoader(PlayerConfig config) => _config = config;

    public IEnumerable<string> ScanMusicFiles()
    {
        return Directory.EnumerateFiles(_config.MusicRootPath, "*.*",
                new EnumerationOptions
                {
                    RecurseSubdirectories = _config.MaxScanDepth > 0,
                    MaxRecursionDepth = _config.MaxScanDepth
                })
            .Where(IsValidAudioFile)
            .Where(IsIncludedFolder);
    }

    private bool IsValidAudioFile(string path)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return _config.SupportedAudioFormats.Contains(ext);
    }

    private bool IsIncludedFolder(string path)
    {
        var dir = Path.GetDirectoryName(path)!;
        return _config.IncludeFolders switch
        {
            null => true,
            _ => _config.IncludeFolders.Count() switch
            {
                0 => true,
                _ => _config.IncludeFolders.Any(pattern =>
                    WildcardMatcher.Match(dir, pattern))
            }
        };
    }
}