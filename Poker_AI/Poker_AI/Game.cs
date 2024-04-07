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
        public int Pot { get { return 400 - Human.Money - AI.Money; } }
        
        public int bet = 5;
        public bool FirstBet()
        {

            int decision = 1;
            while(decision != 5 && decision != 0)
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

        public bool BiddingRound()
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
            int aiBet = AI.Bet(Human.Money, humanBet);
            AI.Money -= aiBet;

            return false;
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
            }
        }
    }
}
