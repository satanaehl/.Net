using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WindowsFormsCapitals
{
    class Capitals
    {
        private readonly string[] _fileNames = { @"Resources\Европа.txt",
                                        @"Resources\Африка.txt",
                                        @"Resources\Америка.txt",
                                        @"Resources\Азия.txt",
                                        @"Resources\Австралия и Океания.txt" };

        int _indexAnswers;
        int _indexTries;
        private string _info = String.Empty;
        private int _rnd;
        List<Capital> CuntriesWithCapitals = new List<Capital>();
        Random rnd1 = new Random();

        public int NumberAnswers
        {
            get { return _indexAnswers; }
            private set { }
        }

        public int NumberTries
        {
            get { return _indexTries; }
            private set { }
        }

        public string InfoString
        {
            get { return _info; }
            private set { }
        }

        private void SetRandom()
        {
            _rnd = rnd1.Next(0, CuntriesWithCapitals.Count);
        }

        public string[] GetOneCountry()
        {
            string[] results = { CuntriesWithCapitals[_rnd].NameCountry, CuntriesWithCapitals[_rnd].NameCapital };

            return results;
        }

        public void GetCountries()
        {


            foreach (var item in _fileNames)
            {
                using (StreamReader reader = new StreamReader(item, Encoding.GetEncoding("windows-1251")))
                {

                    while (reader.Peek() > -1)
                    {

                        string readText = reader.ReadLine();
                        string[] CuntryAndCapital = readText.Split(';');
                        //CuntryAndCapital = readText.Split(';');
                        CuntryAndCapital[0] = CuntryAndCapital[0].Trim();
                        CuntryAndCapital[1] = CuntryAndCapital[1].Trim();
                        Capital NewOne = new Capital { NameCountry = CuntryAndCapital[0], NameCapital = CuntryAndCapital[1] };
                        CuntriesWithCapitals.Add(NewOne);
                    }
                }
            }
        }

        public void StartGame()
        {
            _indexAnswers = 7;
            _indexTries = 5;
            _info = String.Empty;

            SetRandom();
        }

        public void NextQuest()
        {
            if (_indexAnswers > 0)
            {
                _indexAnswers--;
            }
            if (_indexAnswers == 0)
            {
                _info = "Ура! Вы победили!!!";
            }
            SetRandom();
        }

        public bool Try(string tryStr)
        {

            if (_indexTries > 0)
            {
                _indexTries--;
            }

            if (_indexTries == 0)
            {
                _info = "К сожалению, Вы исчерпали все пять попыток!";
                return false;
            }

            tryStr = tryStr.Trim();


            string rightStr = CuntriesWithCapitals[_rnd].NameCapital;


            bool bool01 = String.Equals(rightStr, tryStr, StringComparison.CurrentCultureIgnoreCase);
            if (bool01)
            {
                NextQuest();

                _indexTries = 5;
            }

            return bool01;
        }

        public class Capital
        {
            public string NameCountry { get; set; }
            public string NameCapital { get; set; }
        }
    }
}



