namespace DownloadOptimize.Other;

public class Sorter
{
    public Settings settings;

    public Sorter(Settings settings)
    {
        this.settings = settings;
        
        FileSystemWatcher watcher = new FileSystemWatcher();
        watcher.Path = settings.pathToDownloads;
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                        | NotifyFilters.FileName | NotifyFilters.DirectoryName;

        Console.WriteLine("created watcher");
        watcher.Created += FileCreated;
        watcher.Changed += FileCreated;
        
        watcher.EnableRaisingEvents = true;
    }

    private async void FileCreated(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine("File created: " + e.FullPath);
        
        if (e.FullPath.EndsWith(".~") || e.FullPath.EndsWith(".tmp"))
        {
            return;
        }

        if (Directory.Exists(e.FullPath) || !File.Exists(e.FullPath))
        {
            return;
        }
        
        if (settings.SortEveryFile && settings.WillWeSort)
        {
            if (!Directory.Exists(settings.pathToDownloads + "\\" + e.FullPath.Split('.').Last()))
            {
                Directory.CreateDirectory(settings.pathToDownloads + "\\" + e.FullPath.Split('.').Last());
            }

            string pathToFile = settings.pathToDownloads + "\\" + e.FullPath.Split('.').Last() + "\\" +
                                e.FullPath.Split('\\').Last();
            if (!File.Exists(pathToFile))
            {
                File.Move(e.FullPath, pathToFile);
            }
        }
        else if (settings.WillWeSort)
        {
            if (settings.whatFilesInWhatFolders.ContainsKey("." + e.Name.Split('.').Last()))
            {
                string pathToFolder = settings.pathToDownloads + "\\" +
                                      settings.whatFilesInWhatFolders["." + e.Name.Split('.').Last()];
                if (!Directory.Exists(pathToFolder))
                {
                    Directory.CreateDirectory(pathToFolder);
                }
                
                File.Move(e.FullPath, pathToFolder + "\\" + e.Name.Split('\\').Last());
            }
            else
            {
                string pathToFolder = settings.pathToDownloads + "\\Other";
                if (!Directory.Exists(pathToFolder))
                {
                    Directory.CreateDirectory(pathToFolder);
                }
                File.Move(e.FullPath, pathToFolder + "\\" + e.Name.Split('\\').Last());
            }
        }   
    }
}