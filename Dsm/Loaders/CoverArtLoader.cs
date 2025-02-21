using System.IO;
using Dsm.Models;

namespace Dsm.Loaders;

public class CoverArtLoader
{
    private readonly PlayerConfig _config;

    public CoverArtLoader(PlayerConfig config) => _config = config;
    
    public string FindCoverArt(string audioPath)
    {
        var directory = Path.GetDirectoryName(audioPath)!;
        var fileName = Path.GetFileNameWithoutExtension(audioPath);

        foreach (var pattern in _config.MatchPatterns)
        {
            var path = pattern switch
            {
                "{filename}" => SearchByFileName(directory, fileName),
                "{cover}" => SearchCustomCovers(directory),
                "folder" => SearchFolderCover(directory),
                _ => null
            };

            if (!string.IsNullOrEmpty(path)) return path;
        }
        
        return _config.DefaultCoverArt;
    }
    
    private string SearchByFileName(string dir, string baseName)
    {
        foreach (var ext in _config.SupportedImageFormats)
        {
            var path = Path.Combine(dir, $"{baseName}{ext}");
            if (File.Exists(path)) return path;
        }
        return null;
    }
    
    private string SearchCustomCovers(string dir)
    {
        foreach (var name in _config.CustomCoverNames)
        foreach (var ext in _config.SupportedImageFormats)
        {
            var path = Path.Combine(dir, $"{name}{ext}");
            if (File.Exists(path)) return path;
        }
        return null;
    }

    private string SearchFolderCover(string dir)
    {
        foreach (var ext in _config.SupportedImageFormats)
        {
            var path = Path.Combine(dir, $"folder{ext}");
            if (File.Exists(path)) return path;
        }
        return null;
    }
}