using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WindowsFormsCapitals
{
    class Capitals
    {
        public enum State
        {
            NotWinnerNotLoser,
            Winner,
            Loser
        }

        private readonly List<Capital> _countriesWithCapitals = new List<Capital>();

        private readonly string[] _fileNames = { @"Resources\Европа.txt",
                                        @"Resources\Африка.txt",
                                        @"Resources\Америка.txt",
                                        @"Resources\Азия.txt",
                                        @"Resources\Австралия и Океания.txt" };

        private readonly Random _rnd1 = new Random();
        int _indexAnswers;
        int _indexTries;
        private int _rnd;
        public State StateOfPlayer { get; private set; }
        public int NumberAnswers => _indexAnswers;
        public int NumberTries => _indexTries;

        private void SetRandom()
        {
            _rnd = _rnd1.Next(0, _countriesWithCapitals.Count);
        }

        public string[] GetOneCountry()
        {
            string[] results = { _countriesWithCapitals[_rnd].NameCountry, _countriesWithCapitals[_rnd].NameCapital };

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
                        if (readText != null)
                        {
                            string[] cuntryAndCapital = readText.Split(';');
                            cuntryAndCapital[0] = cuntryAndCapital[0].Trim();
                            cuntryAndCapital[1] = cuntryAndCapital[1].Trim();
                            Capital newOne = new Capital { NameCountry = cuntryAndCapital[0], NameCapital = cuntryAndCapital[1] };
                            _countriesWithCapitals.Add(newOne);
                        }
                    }
                }
            }
        }

        public void StartGame()
        {
            _indexAnswers = 7;
            _indexTries = 5;
            StateOfPlayer = State.NotWinnerNotLoser;
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

                StateOfPlayer = State.Winner;
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
                StateOfPlayer = State.Loser;
                return false;
            }

            tryStr = tryStr.Trim();

            string rightStr = _countriesWithCapitals[_rnd].NameCapital;

            bool successfulTry = String.Equals(rightStr, tryStr, StringComparison.CurrentCultureIgnoreCase);
            if (successfulTry)
            {
                NextQuest();
                _indexTries = 5;
            }
            return successfulTry;
        }

        public class Capital
        {
            public string NameCountry { get; set; }
            public string NameCapital { get; set; }
        }
    }
}



