using System.Globalization;
using System.Text.Json;

namespace DownloadOptimize.Other;

public class Settings
{
    public string langName { get; set; }
    public string pathToDownloads { get; set; }
    public bool WillWeSort { get; set; }
    public bool SortEveryFile { get; set; }
    public Dictionary<string, string> whatFilesInWhatFolders { get; set; }
    public ZipType zipType { get; set; }
    public bool isMakeOldFiles { get; set; }
    public int howManySecondsIsOld { get; set; }
    public int howManySecondsToCheck { get; set; }
    public enum ZipType
    {
        ZIP_MAX,
        ZIP_LOW,
        ZIP_NON
    }
}

public static class Configurator
{
    static Settings settings = new Settings();
    static Dictionary<string, string> lang = new Dictionary<string, string>();
    public static void RunConsoleConfigurator()
    {
        Console.Clear();
        CultureInfo ci = CultureInfo.InstalledUICulture;
        if (ci.Name == "ru-RU")
        {
            lang = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(@".\Languages\ru-RU.json"))!;
            settings.langName = "ru-RU";
        }
        else
        {
            lang = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(@".\Languages\en-US.json"))!;
            settings.langName = "en-US";
        }

        Console.WriteLine(lang["Welcome"]);
        Console.ReadLine();
        
        Console.Clear();
        Console.WriteLine(lang["DownloadsPath"]);
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        bool isGoodPath = false;
        string pathToDownloads = "";
        while (!isGoodPath)
        {
            string answer = Console.ReadLine() ?? "null";
            if (!Directory.Exists(answer))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(lang["InvalidAnswer"]);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            else
            {
                pathToDownloads = answer;
                isGoodPath = true;
            }
        }

        settings.pathToDownloads = pathToDownloads;


        int answer1 = Question(1, 3); //Распределение файлов
        
        switch (answer1)
        {
            case 1:
                settings.WillWeSort = true; 
                settings.SortEveryFile = true; 
                break;
            case 2:
                settings.WillWeSort = true; 
                settings.SortEveryFile = false; 
                settings.whatFilesInWhatFolders = new Dictionary<string, string>
                {
                    {".png","Images"},
                    {".jpg","Images"},
                    {".webm","Images"},
                    {".jpeg","Images"},
                    {".bmp","Images"},
                    {".gif","Images"},
                    
                    {".exe","Executables"},
                    {".bat","Executables"},
                    
                    {".msi","Setups"},
                    
                    {".mp4","Videos"},
                    {".avi","Videos"},
                    {".mpeg","Videos"},
                    
                    {".zip","Archives"},
                    {".7z","Archives"},
                    {".rar","Archives"},
                    
                    {".wav","Audio"},
                    {".mp3","Audio"},
                    {".midi","Audio"},
                    {".kar","Audio"},
                    {".ogg","Audio"},
                    
                    {".txt","Text"},
                    {".rtf","Text"},
                    {".doc","Text"},
                    {".docx","Text"},
                    {".odt","Text"},
                };
                break;
            case 3:
                settings.WillWeSort = false; 
                break;
        }
        
        int answer2 = Question(2, 3); //Сжатие файлов
        
        switch (answer2)
        {
            case 1:
                settings.zipType = Settings.ZipType.ZIP_MAX;
                break;
            case 2:
                settings.zipType = Settings.ZipType.ZIP_LOW;
                break;
            case 3:
                settings.zipType = Settings.ZipType.ZIP_NON;
                break;
        }

        int answer3 = Question(3, 3); //Время, когда файлы будут старыми
        
        switch (answer3)
        {
            case 1:
                settings.isMakeOldFiles = true;
                settings.howManySecondsIsOld = 1209600;
                break;
            case 2:
                settings.isMakeOldFiles = true;
                Console.Clear();
                bool isValidAnswer = false;
                while (!isValidAnswer)
                {
                    try
                    {
                        Console.WriteLine(lang["WriteHowManySeconds"]);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        settings.howManySecondsIsOld = int.Parse(Console.ReadLine()??"0");
                        Console.ResetColor();
                        isValidAnswer = true;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(lang["InvalidAnswer"]);
                        Console.ResetColor();
                    }
                }
                Console.ResetColor();
                break;
            case 3:
                settings.isMakeOldFiles = false;
                break;
        }

        int answer4 = Question(4, 2);
        
        switch (answer4)
        {
            case 1:
                settings.howManySecondsToCheck = 86400;
                break;
            case 2:
                Console.Clear();
                bool isValidAnswer = false;
                while (!isValidAnswer)
                {
                    try
                    {
                        Console.WriteLine(lang["WriteHowManySecondsToCheck"]);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        settings.howManySecondsToCheck = int.Parse(Console.ReadLine()??"0");
                        Console.ResetColor();
                        isValidAnswer = true;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(lang["InvalidAnswer"]);
                        Console.ResetColor();
                    }
                }
                Console.ResetColor();
                break;
            
        }
        
        File.WriteAllText("settings.json", JsonSerializer.Serialize(settings));
        Console.Clear();
        Console.WriteLine(lang[$"ThanksForAnswering"]);
    }

    static int Question(int questionId, int howManyQuestions)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(lang[$"Q{questionId}"]+"\n");

        for (int i = 1; i < howManyQuestions+1; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(i);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("]");
            Console.ResetColor();
            Console.WriteLine(": "+lang[$"A{questionId}{i}"]);
        }

        int answer = int.Parse(Console.ReadKey().KeyChar.ToString());

        if (0 > answer || answer > howManyQuestions)
        {
            Console.WriteLine(lang[$"InputError"]);
            Thread.Sleep(3000);
            Question(questionId, howManyQuestions);
        }
        
        return answer;
    }
}