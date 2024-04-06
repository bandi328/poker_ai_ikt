using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_AI
{
    public class Game
    {
        public List<string> dealtCards = new List<string> { };
        private string dealtCard;
        public bool FirstBet(int decision, Human human, AI ai)
        {
            if(decision == 5)
            {
                human.Money -= 5;
                ai.Money -= 10;
                human.Money -= 5;
                Console.WriteLine($"Játékos alap téte: 5$");
                Console.WriteLine($"AI tét emelés: 10$");
                Console.WriteLine($"Játékos tét tartás: 5$");
                return true;
            }
            else
            {
                return false;
            }
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
    }
}
