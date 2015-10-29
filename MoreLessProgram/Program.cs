using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreLessProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pathInPut = Path.Combine(path, "INPUT.txt");

            //using (StreamWriter writer = File.CreateText(pathInPut))
            //{
            //    writer.WriteLine(7);
            //    writer.WriteLine(5);
            //}

            string aStr = String.Empty;
            string bStr = String.Empty;

            using (StreamReader reader = File.OpenText(pathInPut))
            {
                aStr = reader.ReadLine();
                bStr = reader.ReadLine();

            }

            int a = int.Parse(aStr);
            int b = int.Parse(bStr);

            string pathOutPut = Path.Combine(path, "OUTPUT.txt");

            using (StreamWriter writer = File.CreateText(pathOutPut))
            {
                if (a < b)
                {
                    writer.WriteLine("<");
                }
                else if (a > b)
                {
                    writer.WriteLine(">");
                }
                else writer.WriteLine("=");
            }

        }
    }
}
