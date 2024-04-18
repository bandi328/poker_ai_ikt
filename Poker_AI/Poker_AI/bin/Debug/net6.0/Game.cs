using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_AI
{
    public class Game
    {
        private Human Human;
        private AI AI;
        public Game(Human human, AI ai)
        {
            Human = human;
            AI = ai;
        }
        public List<string> dealtCards = new List<string> { };
        private string? dealtCard;
        List<int> scores = new List<int>();
        public int Pot { get { return 400 - Human.Money - AI.Money; } }
        
        public int bet = 5;

        public void ReadScores()
        {
            StreamReader sr = new StreamReader(FilePath());
            while (!sr.EndOfStream)
            {
                scores.Add(int.Parse(sr.ReadLine().Split(',')[0]));
            }
            sr.Close();
        }
        public static string ReverseString(string str)
        {
            string output = "";
            for (int i = str.Length - 1; i >= 0; i--)
                output += str[i];
            return output;
        }
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
        public bool FirstBet()
        {
            int decision = 1;
            while(decision != 5 || decision != 0)
            {
                Console.Write("Kezdőlicit: ");
                decision = int.Parse(Console.ReadLine());
                if (decision == 5)
                {
                    Human.Money -= 5;
                    AI.Money -= 10;
                    Human.Money -= 5;

                    Console.WriteLine($"Játékos alap téte: 5$");
                    Console.WriteLine($"AI tét emelés: 10$");
                    Console.WriteLine($"Játékos tét tartás: 5$");
                    return true;
                }
                else if(decision == 0)
                {
                    return false;
                }
            }
            return true;

        }
        public void Burning(Cards cards)
        {
            do
            {
                dealtCard = cards.Card[new Random().Next(52)];
            } while (dealtCards.Contains(dealtCard) == true);
            dealtCards.Add(dealtCard);
        }
        public string Dealing(Cards cards)
        {

            do
            {
                dealtCard = cards.Card[new Random().Next(52)];
            } while (dealtCards.Contains(dealtCard) == true);
            dealtCards.Add(dealtCard);
            return dealtCard;
        }
        public void HandListing(Player player)
        {
            foreach (var item in player.Hand)
            {
                Console.WriteLine(item);
            }
        }

        public bool BiddingRound(Cards cards, Game game, List<string> tableCards, AI ai)
        {
            bool mustCall = (200 - AI.Money - (200 - Human.Money) > 0) ? true : false;
            int humanBet = Human.Bet(Human.Money, bet, mustCall);
            if (humanBet > bet)
            {
                bet = humanBet;
            }
            if (humanBet == -1)
            {
                RoundOver(AI);
                return true;
            }
            Human.Money -= humanBet;
            int aiBet = AI.Bet(AI.Money, humanBet, cards, game, tableCards, ai, scores);
            AI.Money -= aiBet;

            return false;
        }
        public Player DecideWinner(int humanCheck, int aiCheck, List<string> tableCards)
        {
            if (humanCheck > aiCheck)
                return Human;
            else if (aiCheck > humanCheck)
                return AI;
            else
            {
                List<string> humanHandCombo = GetHandCombo(Human, tableCards, humanCheck);
                List<string> aiHandCombo = GetHandCombo(AI, tableCards, aiCheck);

                Console.WriteLine("Az Ön sorozata: ");
                foreach (var hand in humanHandCombo)
                {
                    Console.Write($"{hand} ");
                }
                Console.Write('\n');
                Console.WriteLine("AI sorozata: ");
                foreach (var hand in aiHandCombo)
                {
                    Console.Write($"{hand} ");
                }
                Console.Write('\n');
                Thread.Sleep(1800);
                bool humanVSai = IsBetterHand(humanHandCombo, aiHandCombo, humanCheck);
                if(humanVSai)
                    return Human;
                else
                    return AI;
            }
        }
        
        public List<string> GetHandCombo(Player player, List<string> tableCards,int handRank)
        {
            List<string> bestHand = new List<string>(); 
            List<string> fullHand = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                for (int j = i + 1; j < 5; j++)
                {
                    fullHand.Clear();
                    foreach (var item in player.Hand)
                    {
                        fullHand.Add(item);
                    }
                    for (int k = 0; k < 5; k++)
                    { 
                        if (k != i && k != j)
                        {
                            fullHand.Add(tableCards[k]);
                        }
                    }

                    if (IsBetterHand(fullHand, bestHand, handRank))
                    {
                        bestHand = fullHand;
                    }
                }
            }
            return bestHand;
        }
        public List<int> FixCardFormat(List<string> inputCards)
        {
            List<int> outputCards = new List<int>();
            foreach (var card in inputCards)
            {
                if (!int.TryParse(card[1].ToString(), out int num))
                {
                    switch (card[1])
                    {
                        case 'J':
                            outputCards.Add(11);
                            break;
                        case 'Q':
                            outputCards.Add(12);
                            break;
                        case 'K':
                            outputCards.Add(13);
                            break;
                        case 'A':
                            outputCards.Add(14);
                            break;
                    }
                }
                else
                    outputCards.Add(int.Parse(card[1].ToString()));
            }
            return outputCards;
        }
        public bool IsBetterHand(List<string> handToCompare, List<string> bestHandSoFar, int handRank)
        {
            if (bestHandSoFar.Count == 0)
            {
                return true;
            }
            List<int> fixedHandToCompare = FixCardFormat(handToCompare);
            List<int> fixedBestHandSoFar = FixCardFormat(bestHandSoFar);

            if (handRank == 10 || handRank == 9 || handRank == 8 || handRank == 5)
            {
                int handToCompareSum = fixedHandToCompare.Sum();
                int bestToCompareSum = fixedBestHandSoFar.Sum();
                if (handToCompareSum > bestToCompareSum)
                    return true;
                else if(handToCompareSum < bestToCompareSum)
                    return false;
                else
                    return false; // döntetlen
            }
            else if (handRank == 7 || handRank == 6 || handRank == 4 || handRank == 3 || handRank == 2 || handRank == 1)
            {
                List<int> sectionsPattern = new List<int>();
                switch (handRank)
                {
                    case 7:
                        sectionsPattern = new List<int>() { 3, 2 };
                        break;
                    case 6:
                        sectionsPattern = new List<int>() { 1, 1, 1, 1, 1 };
                        break;
                    case 4:
                        sectionsPattern = new List<int>() { 3, 1, 1 };
                        break;
                    case 3:
                        sectionsPattern = new List<int>() { 2, 2, 1 };
                        break;
                    case 2:
                        sectionsPattern = new List<int>() { 2, 1, 1, 1 };
                        break;
                    case 1:
                        sectionsPattern = new List<int>() { 1, 1, 1, 1, 1 };
                        break;
                }
                Dictionary<int, int> rankCountsToCompare = GetRanks(fixedHandToCompare);
                Dictionary<int, int> rankCountsBest = GetRanks(fixedBestHandSoFar);

                List<int> rankingsToCompare = DividedSections(fixedHandToCompare, rankCountsToCompare, sectionsPattern);
                List<int> rankingsBest = DividedSections(fixedBestHandSoFar, rankCountsBest, sectionsPattern);
                for (int i = 0; i < sectionsPattern.Count; i++)
                {
                    if (rankingsToCompare[i] > rankingsBest[i])
                        return true;
                    else if(rankingsToCompare[i] < rankingsBest[i])
                        return false;
                }
                return false; // döntetlen
            }
            return true; // ez elvileg lehetetlen
        }
        public Dictionary<int, int> GetRanks(List<int> cards)
        {
            Dictionary<int, int> rankCounts = new Dictionary<int, int>();
            foreach (var card in cards)
            {
                if (rankCounts.ContainsKey(card))
                    rankCounts[card]++;
                else
                    rankCounts[card] = 1;
            }
            return rankCounts;
        }
        public List<int> DividedSections(List<int> cards, Dictionary<int, int> rankCounts, List<int> sections)
        {

            List<int> rankings = new List<int>();
            int sectionSizeSum = 0;
            for (int i = 0; i < sections.Count; i++)
            {
                foreach (KeyValuePair<int, int> item in rankCounts)
                {
                    if(item.Value == sections[i])
                        rankings.Add(item.Key);
                }
                sectionSizeSum += sections[i];
            }
            return rankings;
        }
        public void RoundOver(Player winner)
        {
            if(winner.GetType() == typeof(Human)) 
            {
                Console.WriteLine($"Ön nyert!\nNyereménye: {Pot}$");
            }
            else
            {
                Console.WriteLine($"AI nyert: {AI.Hand[0]}{AI.Hand[1]}\nNyereménye: {Pot}$");
                foreach (KeyValuePair<int, int> item in AI.scoreBets)
                {
                    AI.Training(scores, item.Key, item.Value, 1);
                }
            }
            winner.Money += Pot;
            Thread.Sleep(2500);
        }
    }
}
