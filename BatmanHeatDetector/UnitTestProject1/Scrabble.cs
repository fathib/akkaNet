using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestProject1
{
    [TestClass]
    public class Scrabble
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<string> input = new List<string>
            {
                "because",
                "first",
                "these",
                "could",
                "which",
                "hicquwh"
            };
            ScrabbleGame sg = new ScrabbleGame();

            int nbElement = 5;

            List<string> words = new List<string>();
            for (int i = 0; i < nbElement; i++)
            {
                words.Add(input[i]);
            }
            sg.LoadDictionary(words);

            var result = sg.WordWithHghScore( input[5]);

            Assert.AreEqual("which", result);

        }
    }



    public class ScrabbleGame
    {
        
        public ScrabbleGame()
        {
            WordsDictionary= new List<Word>();

            LetterScore = new Dictionary<char, int>();
            LetterScore.Add('a',1);
            LetterScore.Add('e', 1);
            LetterScore.Add('i', 1);
            LetterScore.Add('o', 1);
            LetterScore.Add('n', 1);
            LetterScore.Add('r', 1);
            LetterScore.Add('t', 1);
            LetterScore.Add('l', 1);
            LetterScore.Add('s', 1);
            LetterScore.Add('u', 1);

            LetterScore.Add('d', 2);
            LetterScore.Add('g', 2);

            LetterScore.Add('b', 3);
            LetterScore.Add('c', 3);
            LetterScore.Add('m', 3);
            LetterScore.Add('p', 3);

            LetterScore.Add('f', 4);
            LetterScore.Add('h', 4);
            LetterScore.Add('v', 4);
            LetterScore.Add('w', 4);
            LetterScore.Add('y', 4);

            LetterScore.Add('k', 5);

            LetterScore.Add('j', 8);
            LetterScore.Add('x', 8);

            LetterScore.Add('q', 10);
            LetterScore.Add('z', 10);



        }
        public Dictionary<char, int> LetterScore;


        public List<Word> WordsDictionary { get; private set; }

        public void LoadDictionary(List<string> words)
        {
            WordsDictionary = words.Select(w => new Word(w, LetterScore)).OrderByDescending(s=> s.Score).ToList();
        }
        public string WordWithHghScore(string letters)
        {
            var orderedLetter= letters.GroupBy(c => c).Select(group => new Letter(){ Character= group.Key, Count= group.Count() });

            var exitWord = string.Empty;
            foreach (var word in WordsDictionary)
            {
                bool WordIsOk = true;
                foreach (var letter in word.letters)
                {
                    if (!orderedLetter.Any(o => o.Character == letter.Character && o.Count >= letter.Count))
                    {
                        WordIsOk = false;
                        break;
                    }
                }

                if (WordIsOk)
                {
                    exitWord = word.InitialWord;
                    break;
                }
            }


            return exitWord;
        }


    }


    public class Letter
    {
        public char Character { get; set; }
        public int Count { get; set; }
    }



    public class Word
    {

        public Word(string word, Dictionary<char, int> LetterScore)
        {
            InitialWord = word;
            Score = word.Select(c => LetterScore[c]).Sum();
            letters = word.GroupBy(c => c).Select(group => new Letter { Character = group.Key, Count= group.Count() }).ToList();
        }
        public string InitialWord { get; private set; }
        public int Score { get; private set; }

        public List<Letter> letters { get; private set; }
    }


}


