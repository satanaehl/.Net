using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace TreeOfDir
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            do
            {
                Console.Write("Enter a path:  ");
                path = Console.ReadLine();

            } while (!Regex.IsMatch(path, @"([a-zA-Z]:)(\\(\w+\s*)*)+"));

            TreeOfDir.PrintDirRecConsole(path);
            TreeOfDir.PrintDirRecTextFile(path);
            TreeOfDir.PrintTextFile(path);
        }
    }

    public static class TreeOfDir
    {
        private static StringBuilder result = new StringBuilder(String.Empty);

        public static void PrintDirRecConsole(string path)
        {
            PrintConsole(path, 0);
        }

        private static void PrintConsole(string dir, int level)
        {
            for (int i = 0; i < level; i++) Console.Write("\t");
            string name = dir.Substring(1 + dir.LastIndexOf(Path.DirectorySeparatorChar));

            Console.WriteLine("{0}", name);

            try
            {
                foreach (var directory in Directory.EnumerateDirectories(dir))
                {
                    PrintConsole(directory, level + 1);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry but this  directory may contain inaccessible folders!"
                    + Environment.NewLine + "Try a different directory!" + Environment.NewLine);
            }
        }

        public static void PrintDirRecTextFile(string path)
        {

            string fileName = Path.Combine(path, "OUTPUT.txt");

            using (StreamWriter fileWriter = new StreamWriter(fileName))
            {
                PrintTextFile(path, 0);
                fileWriter.Write(result);
                fileWriter.Close();
            }
        }

        private static void PrintTextFile(string dir, int level)
        {
            result.Clear();  //
            for (int i = 0; i < level; i++) result.Append("\t");
            string name = dir.Substring(1 + dir.LastIndexOf(Path.DirectorySeparatorChar));

            result.AppendLine(name);

            try
            {
                foreach (var directory in Directory.EnumerateDirectories(dir))
                {
                    PrintTextFile(directory, level + 1);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry but this  directory may contain inaccessible folders!"
                    + Environment.NewLine + "Try a different directory!" + Environment.NewLine);
            }
        }

        public static void PrintTextFileConsole(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] dirs = dir.GetDirectories("*", SearchOption.AllDirectories);

            string[] names = new string[dirs.Length];

            for (int i = 0; i < dirs.Length; i++)
            {
                names[i] = dirs[i].FullName;
                Console.WriteLine(names[i]);
            }
            Array.Sort(names);
            foreach (var name in names)
            {
                string[] newName = name.Split(Path.DirectorySeparatorChar);
                for (int i = 0; i < newName.Length; i++) Console.Write("\t");
                Console.WriteLine(newName[newName.Length - 1]);
            }
        }

        public static void PrintTextFile(string path)
        {
            StringBuilder resultOfNonRec = new StringBuilder();

            resultOfNonRec.AppendLine(path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1));

            DirectoryInfo dir = new DirectoryInfo(path);

            try
            {
                DirectoryInfo[] dirs = dir.GetDirectories("*", SearchOption.AllDirectories);
                string[] names = new string[dirs.Length];

                for (int i = 0; i < dirs.Length; i++)
                {
                    names[i] = dirs[i].FullName;
                }
                Array.Sort(names);
                foreach (var name in names)
                {
                    string[] newName = name.Split(Path.DirectorySeparatorChar);
                    for (int i = 0; i < newName.Length - 2; i++) resultOfNonRec.Append("\t");
                    resultOfNonRec.AppendLine(newName[newName.Length - 1]);
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Sorry but this  directory may contain inaccessible folders!"
                    + Environment.NewLine + "Try a different directory!");
            }

            //Console.WriteLine(resultOfNonRec.ToString());
            string pathAndName = Path.Combine(path, "OUTPUTnotRec.txt");
            using (StreamWriter fileWriter = new StreamWriter(pathAndName))
            {
                fileWriter.Write(resultOfNonRec.ToString());
                fileWriter.Close();
            }
        }
    }
}