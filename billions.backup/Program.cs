using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace billions.backup
{
    class Program
    {
        private static readonly String savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "my games\\They Are Billions\\Saves");
        private static readonly String backPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "my games\\They Are Billions\\Backups");
        private static FileSystemWatcher watcher;

        static void Main(string[] args)
        {
            watcher = new FileSystemWatcher(savePath);
            watcher.Changed += FileChanged;
            watcher.Created += FileChanged;
            watcher.Filter = "*.zxsav";
            watcher.EnableRaisingEvents = true;

            System.Console.WriteLine("Watching for Backups. \r\n\r\nPress any key to exit.");
            System.Console.ReadKey();
        }

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted) return;
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            var zxcheckName = Path.GetFileNameWithoutExtension(e.FullPath) + ".zxcheck";
            var zxsav = new FileInfo(e.FullPath);
            var zxcheck = new FileInfo(Path.Combine(zxsav.DirectoryName, zxcheckName));
            SaveFiles(zxsav, zxcheck);
        }

        static DirectoryInfo GetLatestBackupDirectory(FileInfo file)
        {
            var name = Path.GetFileNameWithoutExtension(file.Name);
            var backup = "_backup";
            if (name.ToLower().EndsWith(backup)) name = name.Substring(0, name.Length - backup.Length);

            var subDir = new DirectoryInfo(Path.Combine(backPath, name + " " + DateTime.Now.ToString("MM-dd-yyyy HH.mm")));
            if (!subDir.Exists) subDir.Create();
            return subDir;
        }
    
        static void SaveFiles(FileInfo zxsav, FileInfo zxcheck)
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            var backupDirectory = GetLatestBackupDirectory(zxsav);
            Console.WriteLine("Backup saved to: {0}", Path.Combine(backupDirectory.Parent.Name, backupDirectory.Name, zxsav.Name));
            try
            {
                // copy .zxsav file
                zxsav.CopyTo(Path.Combine(backupDirectory.FullName, cleanBackupFromName(zxsav.Name)), true);

                // copy .zxcheck file
                zxcheck.CopyTo(Path.Combine(backupDirectory.FullName, cleanBackupFromName(zxcheck.Name)), true);

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static String cleanBackupFromName(String filePath)
        {
            var extension = Path.GetExtension(filePath);
            var name = Path.GetFileNameWithoutExtension(filePath);
            var backup = "_backup";
            if (name.ToLower().EndsWith(backup)) name = name.Substring(0, name.Length - backup.Length);
            return String.Format(name + extension);
        }
    }
}
