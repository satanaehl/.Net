using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace WindowsFormsCapitals
{
    /// <summary>
    /// Describes a state of the player.
    /// </summary>
    public enum State
    {
        None,
        Winner,
        Loser
    }

    class Capitals
    {
        public Capitals()
        {
            foreach (var item in _fileNames)
            {
                using (StreamReader reader = new StreamReader(item, Encoding.GetEncoding("windows-1251")))
                {
                    while (reader.Peek() > -1)
                    {
                        Task<string> readText = reader.ReadLineAsync();
                        if (readText != null)
                        {
                            string[] cuntryAndCapital = readText.Result.Split(';');
                            cuntryAndCapital[0] = cuntryAndCapital[0].Trim();
                            cuntryAndCapital[1] = cuntryAndCapital[1].Trim();
                            Capital newOne = new Capital
                            {
                                NameCountry = cuntryAndCapital[0],
                                NameCapital = cuntryAndCapital[1]
                            };
                            _countriesWithCapitals.Add(newOne);
                        }
                    }
                }
            }
        }

        private readonly List<Capital> _countriesWithCapitals = new List<Capital>();

        private readonly string[] _fileNames =
        {
            @"Resources\Европа.txt",
            @"Resources\Африка.txt",
            @"Resources\Америка.txt",
            @"Resources\Азия.txt",
            @"Resources\Австралия и Океания.txt"
        };

        private readonly TRandom _rnd1 = new TRandom(new ALFGenerator());

        private int _rnd;
        public State StateOfPlayer { get; private set; } //= State.None;
        public int NumberAnswers { get; private set; }

        public int NumberTries { get; private set; }

        private void SetRandom() => _rnd = _rnd1.Next(_countriesWithCapitals.Count);

        public string[] OneCountry
        {
            get
            {
                string[] results = { _countriesWithCapitals[_rnd].NameCountry, _countriesWithCapitals[_rnd].NameCapital };
                return results;
            }
        }

        public void StartNewGame()
        {
            NumberAnswers = 7;
            NumberTries = 5;
            StateOfPlayer = State.None;
            SetRandom();
        }

        public void NextQuestion()
        {
            if (NumberAnswers > 0)
            {
                NumberAnswers--;
            }
            if (NumberAnswers == 0)
            {
                StateOfPlayer = State.Winner;
            }
            SetRandom();
        }

        public bool Try(string tryStr)
        {
            if (NumberTries > 0)
            {
                NumberTries--;
            }

            if (NumberTries == 0)
            {
                StateOfPlayer = State.Loser;
                return false;
            }

            tryStr = tryStr.Trim();

            string rightStr = _countriesWithCapitals[_rnd].NameCapital;

            bool successfulTry = String.Equals(rightStr, tryStr, StringComparison.CurrentCultureIgnoreCase);
            if (successfulTry)
            {
                NextQuestion();
                NumberTries = 5;
            }
            return successfulTry;
        }

        private class Capital
        {
            public string NameCountry { get; set; }
            public string NameCapital { get; set; }
        }
    }
}