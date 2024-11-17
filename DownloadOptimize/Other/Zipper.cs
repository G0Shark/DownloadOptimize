using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using Microsoft.VisualBasic.Logging;

namespace DownloadOptimize.Other;

public class Zipper
{
    public Settings settings;

    public Zipper(Settings settings)
    {
        this.settings = settings;
        Logger.Log("Zipper created");
    }
    
    public void CheckForOldFiles()
    {
        Logger.Log("CheckForOldFiles() runned...");
        var files = Directory.GetFiles(settings.pathToDownloads, "*.*", SearchOption.AllDirectories);
        
        List<string> filesOlds = new List<string>();
        
        foreach (var file in files)
        {
            Logger.INFO($"Founded file: {file}");
            if (File.GetCreationTime(file).AddSeconds(settings.howManySecondsIsOld).ToLocalTime() > DateTime.Now.ToLocalTime())
            {
                continue;
            }

            if (file.EndsWith("oldFiles.zip"))
            {
                Logger.Error("Choosed a own zip, skipping...");
                continue;
            }

            if (settings.zipType != Settings.ZipType.ZIP_NON)
            {
                filesOlds.Add(file);
            }
        }

        if (filesOlds.Count == 0)
        {
            return;
        }
        
        Logger.Log("File old, checking...");
        string pathToZip = settings.pathToDownloads + "\\oldFiles.zip";

        if (!File.Exists(pathToZip))
        {
            using (FileStream zipFile = new FileStream(pathToZip, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipFile, ZipArchiveMode.Create, true))
                {
                    Logger.Log("Creating a zip archive...");
                }
            }
        }
        else
        {
            CompressionLevel compressionLevel = CompressionLevel.Optimal;
            if (settings.zipType == Settings.ZipType.ZIP_MAX)
            {
                compressionLevel = CompressionLevel.SmallestSize;
            }
            
            using (ZipArchive zipFrom = ZipFile.Open(pathToZip, ZipArchiveMode.Read))
            using (ZipArchive zipTo = ZipFile.Open(pathToZip + ".tmp", ZipArchiveMode.Create))
            {
                foreach (ZipArchiveEntry entryFrom in zipFrom.Entries)
                {
                    ZipArchiveEntry entryTo = zipTo.CreateEntry(entryFrom.FullName);

                    using (Stream streamFrom = entryFrom.Open())
                    using (Stream streamTo = entryTo.Open())
                    {
                        streamFrom.CopyTo(streamTo);
                    }
                }

                foreach (String filePath in filesOlds)
                {
                    string nm = Path.GetFileName(filePath);
                    zipTo.CreateEntryFromFile(filePath, nm, compressionLevel);
                    File.Delete(filePath);
                }
            }

            File.Delete(pathToZip);
            File.Move(pathToZip + ".tmp", pathToZip);
        }
        
        DeleteEmptySubdirectories(settings.pathToDownloads);
    }
    void DeleteEmptySubdirectories(string directory)
    {
        try
        {
            DirectoryInfo info = new DirectoryInfo(directory);
            if (info.GetFiles().Length == 0 && info.GetDirectories().Length == 0)
            {
                Directory.Delete(directory, true);
                Logger.Log($"Deleted empty directory: {directory}");
            }
            else
            {
                foreach (DirectoryInfo subDir in info.GetDirectories())
                {
                    DeleteEmptySubdirectories(subDir.FullName);
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            Logger.Error($"Access denied: {directory}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Error processing {directory}: {ex.Message}");
        }
    }
}