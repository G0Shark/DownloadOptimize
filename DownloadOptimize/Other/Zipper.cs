using System.IO.Compression;

namespace DownloadOptimize.Other;

public class Zipper
{
    public Settings settings;

    public Zipper(Settings settings)
    {
        this.settings = settings;
        Console.WriteLine("Ziper created");
    }

    public void CheckForOldFiles()
    {
        Console.WriteLine("Checking for old files");
        var files = Directory.GetFiles(settings.pathToDownloads, "*.*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            Console.WriteLine($"Found file: {file}");
            if (File.GetCreationTime(file).AddSeconds(5).ToLocalTime() > DateTime.Now.ToLocalTime())
            {
                Console.WriteLine("file dont old");
                Console.WriteLine(File.GetCreationTime(file));
                continue;
            }

            if (file.EndsWith("oldFiles.zip"))
            {
                Console.WriteLine("Choosed a own zip");
                continue;
            }

            if (settings.zipType != Settings.ZipType.ZIP_NON)
            {
                Console.WriteLine("file old, checking...");
                string pathToZip = settings.pathToDownloads + "\\oldFiles.zip";

                if (!File.Exists(pathToZip))
                {
                    using (FileStream zipFile = new FileStream(pathToZip, FileMode.Create))
                    {
                        using (ZipArchive archive = new ZipArchive(zipFile, ZipArchiveMode.Create, true))
                        {
                            // At this point, we have an empty zip file
                            Console.WriteLine("Empty zip file created successfully.");
                        }
                    }
                }
                else
                {
                    try
                    {
                        Console.WriteLine("zip file already exists");
                        using (var zip = ZipFile.Open(pathToZip, ZipArchiveMode.Update))
                        {
                            var FileInfo = new FileInfo(file);
                            zip.CreateEntryFromFile(FileInfo.FullName, FileInfo.Name);
                            File.Delete(file);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("some problems... try in another time");
                    }
                }
            }
        }
        
        DeleteEmptySubdirectories(settings.pathToDownloads);
    }
    void DeleteEmptySubdirectories(string directory)
    {
        try
        {
            DirectoryInfo info = new DirectoryInfo(directory);
            
            // Check if the directory itself is empty
            if (info.GetFiles().Length == 0 && info.GetDirectories().Length == 0)
            {
                Directory.Delete(directory, true);
                Console.WriteLine($"Deleted empty directory: {directory}");
            }
            else
            {
                // Recursively process subdirectories
                foreach (DirectoryInfo subDir in info.GetDirectories())
                {
                    DeleteEmptySubdirectories(subDir.FullName);
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Access denied: {directory}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing {directory}: {ex.Message}");
        }
    }
}