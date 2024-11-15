using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using DownloadOptimize.Other;

namespace DownloadOptimize;

static class Program
{
    static NotifyIcon notifyIcon = new NotifyIcon();
    static bool Visible = false;
    static void Main(string[] args)
    {
        AllocConsole();
        IntPtr handle = GetConsoleWindow();
        
        //Проверка на наличие конфига, если его нет, или программа запущенна впервые, то он запустит процесс настройки
        if (!File.Exists("settings.json"))
        {
            ShowWindow(handle, 5);
            Configurator.RunConsoleConfigurator();
        }
        Settings settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("settings.json"))!;
        Sorter sorter = new Sorter(settings);
        
        IconInit(handle);
        Application.Run(); 

        notifyIcon.Visible = false;
    }
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool AllocConsole();

    //=======================================
    static void IconInit(IntPtr handle)
    {
        notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        notifyIcon.Visible = true;
        notifyIcon.Text = Application.ProductName;

        notifyIcon.DoubleClick += (s, e) =>
        {
            Visible = !Visible;
            if (Visible)
            {
                ShowWindow(handle, 5);
            }
            else
            {
                ShowWindow(handle, 0);
            }
        };

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Exit", null, (s, e) => { Application.Exit(); });
        notifyIcon.ContextMenuStrip = contextMenu;
    }
}