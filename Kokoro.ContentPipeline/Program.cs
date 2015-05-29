using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace Kokoro.ContentPipeline
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO Add support for the XML based format

#if !DEBUG
            try
            {
#endif
            if (args.Length < 2 || args[0] == "-h" || args[0] == "--h")
            {
                Console.WriteLine("Kokoro Content Processor\nUsage:\n\tKokoro.ContentPipeline [COMMAND] [ARCHIVE] [FILE/DIRECTORY PATH]\nCommands:\n  -c\tCreate Archive from Directory\n  -a\tAppend file to archive\n  -3dm\tConvert file to engine format and add to archive\n  -3dc\tConvert file to engine format and save to disk\n  -b\tBatch mode - Read commands from supplied XML file");
            }
            else
            {
                ProcessFS2(args);
            }
#if !DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed");
                Console.WriteLine(e);
                return;
            }   //Ignore all errors if release build
#endif
            Console.WriteLine("Done");
        }

        static string baseDir = "";
        static void ProcessFS2(string[] args)
        {
            Console.WriteLine(args[0]);
            Console.WriteLine(args[1]);
            if (args[0] != "-b")
            {
                ProcessFS(args[0], args[1], args[2]);
            }
            else
            {
                string[] commands = File.ReadAllLines(args[1]);

                for (int a = 0; a < commands.Length; a++)
                {
                    string cmd = commands[a].Split(' ')[0];
                    if (cmd != "-b") ProcessFS2(new string[] { cmd, commands[a].Replace(cmd, "").Split(',')[0].Trim(), baseDir + commands[a].Replace(cmd, "").Split(',')[1].Trim() });       //Allow recursive commands in a command list
                    else
                    {
                        baseDir = Path.GetDirectoryName(commands[a].Replace(cmd, "").Trim()) + "/";
                        ProcessFS2(new string[] { cmd, commands[a].Replace(cmd, "").Trim() });
                    }
                }
            }
        }

        static void ProcessFS(string cmd, string archiveName, string option)
        {
#if DEBUG
            Console.WriteLine("{CMD: " + cmd + ", ARCHIVE: " + archiveName + ", OPTION: " + option + ", PATH: " + Environment.CurrentDirectory + "}");
#endif

            if (cmd == "-c")
            {
                ZipFile.CreateFromDirectory(option, archiveName, CompressionLevel.Fastest, false);
            }
            else if (cmd == "-a")
            {
                ZipArchive archive = new ZipArchive(new System.IO.FileStream(archiveName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite), ZipArchiveMode.Update);

                if (archive.GetEntry(option) == null)
                {
                    archive.CreateEntryFromFile(option, Path.GetFileName(option));
                    archive.Dispose();
                }
                else
                {
                    archive.GetEntry(option).Delete();
                    archive.Dispose();
                    ProcessFS(cmd, archiveName, option);
                }
            }
            else if (cmd == "-3dm")
            {
                ZipArchive archive = new ZipArchive(new System.IO.FileStream(archiveName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite), ZipArchiveMode.Update);
                if (archive.GetEntry(option) == null)
                {
                    if (new string[] { ".obj", ".3ds", ".dae" }.Contains(Path.GetExtension(option))) File.WriteAllBytes(Path.GetFileNameWithoutExtension(option) + ".ko", ModelConvert.Process(option));
                    archive.CreateEntryFromFile(Path.GetFileNameWithoutExtension(option) + ".ko", Path.GetFileName(option));
                    archive.Dispose();
                    File.Delete(Path.GetFileNameWithoutExtension(option) + ".ko");
                }
                else
                {
                    archive.GetEntry(option).Delete();
                    archive.Dispose();
                    ProcessFS(cmd, archiveName, option);
                }
            }
            else if (cmd == "-3dc")
            {
                if (new string[] { ".obj", ".3ds", ".dae" }.Contains(Path.GetExtension(option))) File.WriteAllBytes(Path.GetFileNameWithoutExtension(option) + ".ko", ModelConvert.Process(option));
            }
        }

    }
}
