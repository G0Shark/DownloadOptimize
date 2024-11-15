namespace DownloadOptimize.Other;

public class Zipper
{
    public Settings settings;

    public Zipper(Settings settings)
    {
        this.settings = settings;
    }

    public void CheckForOldFiles()
    {
        var files = Directory.GetFiles(settings.pathToDownloads, "*.*", SearchOption.AllDirectories);
    }
}