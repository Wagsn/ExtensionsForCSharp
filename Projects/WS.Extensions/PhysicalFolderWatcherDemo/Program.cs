using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WS.Extensions.FileProviders.Physical;

namespace PhysicalFolderWatcherDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{int.MaxValue} {int.MaxValue.ToChinese()}");
            //Console.WriteLine($"{long.MaxValue} {long.MaxValue.ToChinese()}");
            //Console.WriteLine($"{10} {10.ToChinese()}");
        }

        static void Watch()
        {
            var pluginDir = Path.Combine(AppContext.BaseDirectory, "Plugins");
            if (!Directory.Exists(pluginDir)) Directory.CreateDirectory(pluginDir);
            if (!Directory.Exists(Path.Combine(pluginDir, "WS.EmailSender"))) Directory.CreateDirectory(Path.Combine(pluginDir, "WS.EmailSender"));
            if (!Directory.Exists(Path.Combine(pluginDir, "WS.Player"))) Directory.CreateDirectory(Path.Combine(pluginDir, "WS.Player"));
            // 监听子文件夹内容的变化
            using var watcher = SubfolderChangeWatcher.Watch(pluginDir, (modPluginDirs) =>
            {
                Console.WriteLine($"- {DateTime.Now.ToString("HH:mm:ss.FFFFFF")}\r\n{string.Join("\r\n", modPluginDirs.Select(a => $"{a} : Mod"))}");
            });
            while (true)
            {
                Task.Delay(5 * 1000).Wait();
            }
        }
    }
}
