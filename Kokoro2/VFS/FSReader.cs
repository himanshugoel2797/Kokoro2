using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace Kokoro2.VFS
{
    public static class FSReader
    {
        static Dictionary<string, ZipArchive> archives;

        //The FileSystem implements its own asynchronous resource loader, this thread however is not frame limited

        static FSReader()
        {
            archives = new Dictionary<string, ZipArchive>();
        }

        public static void LoadFileSystem(string archive, string directoryMap)
        {
            FileStream arch = File.OpenRead(archive);
            archives[directoryMap] = new ZipArchive(arch, ZipArchiveMode.Read);
        }

        public static void UnloadFileSystem(string directoryMap)
        {
            archives.Remove(directoryMap);
        }

        /// <summary>
        /// Check if an item (file or Directory) exists
        /// </summary>
        /// <param name="file">The path to check</param>
        /// <returns>True if the item exists</returns>
        public static bool ItemExists(string file)
        {
            string baseDir = file.Split('/')[0];
            file = file.Replace(baseDir + "/", "");


            for (int i = 0; i < archives[baseDir].Entries.Count; i++)
            {
                if (archives[baseDir].Entries[i].FullName == file)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Open a file
        /// </summary>
        /// <param name="file">The path to the file to open</param>
        /// <returns>The File Stream</returns>
        public static Stream OpenFile(string file, bool decompress = false)
        {
            string baseDir = file.Split('/')[0];
            file = file.Replace(baseDir + "/", "");

            return archives[baseDir].GetEntry(file).Open();
        }

        public static void PerformOperationAsync(Func<object> op, Action<object> Callback)
        {
            ThreadPool.QueueUserWorkItem((o) => { Callback(op()); });
        }

    }
}