using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FilenameExtension
{
    class Program
    {

        /// <summary>
        /// Add an extension to files .png, .gif, .jpg ib the directory /My Documents/guess-extension without any extensions in names of files. 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            byte[] pngBytes =
            {
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52
            };
            byte[] jpgBytes = { 0xFF, 0xD8, 0xFF };
            byte[] gif01Bytes = { 0x47, 0x49, 0x46, 0x38 };
            byte[] gif02Bytes = { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };
            byte[] gif03Bytes = { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };


            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pathDir = Path.Combine(path, "guess-extension");

            DirectoryInfo dirInfo = new DirectoryInfo(pathDir);

            IEnumerable<FileInfo> files = dirInfo.EnumerateFiles();

            foreach (FileInfo file in files)
            {
                if (Path.GetExtension(file.Name) == ".bin")
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(file.Name);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(file.Name);
                }
            }

            Extension.ChangeExtensionFromBin(pathDir, pngBytes, ".png");
            Extension.ChangeExtensionFromBin(pathDir, jpgBytes, ".jpg");
            Extension.ChangeExtensionFromBin(pathDir, gif01Bytes, ".gif");
            Extension.ChangeExtensionFromBin(pathDir, gif02Bytes, ".gif");
            Extension.ChangeExtensionFromBin(pathDir, gif03Bytes, ".gif");

            Console.WriteLine(Environment.NewLine);

            foreach (FileInfo file in files)
            {
                if (Path.GetExtension(file.Name) == ".bin")
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(file.Name);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(file.Name);
                }
            }

            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        public static class Extension
        {
            public static void ChangeExtensionFromBin(string pathDir, byte[] example, string extensionName)
            {
                foreach (string pathFile in Directory.EnumerateFiles(pathDir, "*.bin"))
                {
                    byte[] example1;
                    using (BinaryReader reader = new BinaryReader(File.OpenRead(pathFile)))
                    {
                        example1 = reader.ReadBytes(example.Length);
                    }
                    if (example.SequenceEqual(example1))
                    {
                        string destFileName = Path.ChangeExtension(pathFile, extensionName);
                        if (!File.Exists(destFileName))
                        {
                            File.Copy(pathFile, destFileName);
                            File.Delete(pathFile);
                        }
                        else
                        {
                            File.Delete(destFileName);
                            File.Copy(pathFile, destFileName);
                            File.Delete(pathFile);
                        }
                    }
                }
            }


        }
    }
}
