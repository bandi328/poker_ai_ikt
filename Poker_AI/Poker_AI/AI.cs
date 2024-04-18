using Poker_AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Poker_AI
{

    public class AI : Player
    {
        //kombinációk keresése, sorbaállítása, legjobb kiválasztása
        //1-10 pontozás, súlyozás (kivéve Royal Flush, ez esetén akár ALL IN) lap értékéből
        //       szín,szám (pontos egyezést néz)

        //10 5,   5 (AKQJ10)
        //9  5,   5 (szám egymást követi)
        //8  0,   4
        //7  0,   3+2
        //6  5,   0
        //5  0,   5 (szám egymást követi)
        //4  0,   3
        //3  0,   2+2
        //2  0,   2
        //1  0,   0 (magas lap)

        //esély számolása kiválasztott kombinációból

        //esély alapján tét rakása/dobás/emelés/skippelés


        public Dictionary<int, int> scoreBets = new Dictionary<int, int>();


    //tét
    public string FilePath()
    {
        string currentDir = Directory.GetCurrentDirectory().ToString();
        string toCut = "";
        for (int i = 0; i < 17; i++)
        {
            toCut += currentDir[currentDir.Length - i - 1];
        }
        return currentDir.Replace(ReverseString(toCut), "") + @"\data.txt";
    }
    public static string ReverseString(string str)
        {
            string output = "";
            for (int i = str.Length - 1; i >= 0; i--)
                output += str[i];
            return output;
        }
        public int GetRowNumber(List<int> scores, int score)
        {
            for (int i = 0; i < scores.Count; i++)
            {
                if (scores[i] == score)
                {
                    return i;
                }
            }
            return 0;
        }
        public string NewRowContent(int rowNumber, string[] keysValues, int score, bool newCase, int randomBet, int delta)
        {
            string output = score + ",";

            foreach (string keyValue in keysValues)
            {
                if(keyValue.Split(':')[0] == randomBet.ToString())
                    output += keyValue.Split(':')[0] + ":" + (int.Parse(keyValue.Split(':')[1]) + delta) + ".";
                else
                    output += keyValue.Split(':')[0] + ":" + (int.Parse(keyValue.Split(':')[1]) + delta) + ".";
            }
            if(newCase)
                output += randomBet + ":1" + ".";
            string actualOutput = "";
            for (int i = 0; i<output.Length; i++)
			{
                if(i + 1 != output.Length)
                    actualOutput += output[i];
            }
            return actualOutput;
        }
        public int Bet(int Money, int humanBet, Cards cards, Game game, List<string> tableCards, AI ai, List<int> scores)
        {
            List<string> avaliableCards = new List<string>();
            foreach (var card in cards.BaseCard)
            {
                avaliableCards.Add(card);
            }
            int score = 0;
            foreach (var card in cards.BaseCard)
            {
                if(!game.dealtCards.Contains(card))
                {
                    avaliableCards.Add(card);
                }
            }
            List<string> handCheckList = new List<string>() { ai.Hand[0], ai.Hand[1]};
            foreach (var card in tableCards)
            {
                if(card != "?")
                    handCheckList.Add(card);
            }
            foreach (var card in avaliableCards)
            {
                handCheckList.Add(card);
                score += new HandCheck().CheckHand(handCheckList);
                handCheckList.Remove(card);
            }
            Random rand = new Random();
            int randomBet;
            int randomGen = rand.Next(1, 5);
            //foreach (var oneScore in scores)
            //{
            //    if(oneScore == score)
            //    {
            //        string[] keysValues = line.Split(',')[1].Split('.');
            //        List<string> keys = new List<string>();
            //        List<string> values = new List<string>();
            //    }
            //}
            StreamReader sr = new StreamReader(FilePath());
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (int.Parse(line.Split(',')[0]) == score)
                {
                    string[] keyValues = line.Split(',')[1].Split('.');
                    //string[] values = new string[keyValues.Length];
                    //foreach(string keyValue in keyValues)
                    //{
                    //    keyValue.Split(':')[1]
                    //}
                    int maxValue = 0;
                    foreach(string keyValue in keyValues)
                    {
                        if (int.Parse(keyValue.Split(':')[1]) > maxValue)
                        {
                            maxValue = int.Parse(keyValue.Split(':')[1]);
                        }
                    }
                    int maxKey = 0;
                    foreach (string keyValue in keyValues)
                    {
                        if(int.Parse(keyValue.Split(':')[0]) == maxValue) 
                        {
                            maxKey = int.Parse(keyValue.Split(':')[0]);
                        }
                    }
                   
                    scoreBets[score] = maxKey;
                    return maxKey;

                    sr.Close();
                }
                sr.Close();
            }
            sr.Close();
            if (randomGen == 1)
            {
                randomBet = -1;
            }
            else if(randomGen == 2)
            {
                randomBet = 0;
            }
            else if(randomGen == 3)
            {
                do
                {
                    randomBet = rand.Next(humanBet, humanBet * 2);
                } while (Money - randomBet < 0);
            }
            else
            {
                do
                {
                    randomBet = rand.Next(humanBet * 2, 201);
                } while (Money - randomBet < 0);
            }

            scoreBets[score] = randomBet;
            return randomBet;

        }
        public void Training(List<int> scores, int score, int randomBet, int delta)
        {
            string line;
            string lineScore;

            if (scores.Contains(score))
            {
                StreamReader sr = new StreamReader(FilePath());
                do
                {
                    line = sr.ReadLine();
                    lineScore = line.Split(',')[0];

                } while (lineScore != score.ToString());
                sr.Close();

                string[] keysValues = line.Split(',')[1].Split('.');
                List<string> keys = new List<string>();
                List<string> values = new List<string>();
                int rowNumber = GetRowNumber(scores, score);
                foreach (string keyValue in keysValues)
                {
                    if (keysValues[0] != "")
                    {
                        keys.Add(keyValue.Split(':')[0]);
                        values.Add(keyValue.Split(':')[1]);
                    }
                }
                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] == randomBet.ToString())
                    {
                        OverwriteRow(FilePath(), rowNumber, NewRowContent(rowNumber, keysValues, score, false, randomBet, delta));
                        return;
                    }
                }
                if (keysValues[0] != "")
                {
                    OverwriteRow(FilePath(), rowNumber, NewRowContent(rowNumber, keysValues, score, true, randomBet, delta));
                }
                else
                {
                    OverwriteRow(FilePath(), rowNumber, score + "," + randomBet + ":1");
                }


            }

            else
            {
                scores.Add(score);

                StreamWriter sw = new StreamWriter(FilePath(), true);
                sw.WriteLine(score + ",");
                sw.Close();
            }

        }
        static void OverwriteRow(string filePath, int rowNumber, string newRowContent)
        {
            string[] lines = File.ReadAllLines(filePath);

            if (rowNumber >= 0 && rowNumber < lines.Length)
            {
                lines[rowNumber] = newRowContent;
                File.WriteAllLines(filePath, lines);
            }
        }
    }
}
