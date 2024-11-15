namespace DownloadOptimize.Other;

public class FileManager
{
    public Settings settings;

    public FileManager(Settings settings)
    {
        this.settings = settings;
        
        FileSystemWatcher watcher = new FileSystemWatcher();
        watcher.Path = settings.pathToDownloads;
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                        | NotifyFilters.FileName | NotifyFilters.DirectoryName;

        watcher.Created += new FileSystemEventHandler(FileCreated);
    }

    private void FileCreated(object sender, FileSystemEventArgs e)
    {
        if (settings.SortEveryFile)
        {
            if (!Directory.Exists(settings.pathToDownloads + "\\" + e.FullPath.Split('.').Last()))
            {
                Directory.CreateDirectory(settings.pathToDownloads + "\\" + e.FullPath.Split('.').Last());
            }
            
            File.Move(e.FullPath, settings.pathToDownloads + "\\" + e.FullPath.Split('.').Last()+e.FullPath.Split('\\').Last());
        }
        else if (settings.WillWeSort)
        {
            if (!Directory.Exists(settings.pathToDownloads + "\\" + e.FullPath.Split('.').Last()))
            {
                Directory.CreateDirectory(settings.pathToDownloads + "\\" + e.FullPath.Split('.').Last());
            }
            
            File.Move(e.FullPath, settings.pathToDownloads + "\\" + e.FullPath.Split('.').Last()+e.FullPath.Split('\\').Last());
        }
    }
}