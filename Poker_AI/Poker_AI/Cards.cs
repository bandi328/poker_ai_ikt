using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_AI
{
    public class Cards
    {
        private List<string> BaseCard = new List<string> { "♠2", "♠3", "♠4", "♠5", "♠6", "♠7", "♠8", "♠9", "♠10", "♠J", "♠Q", "♠K", "♠A", "♣2", "♣3", "♣4", "♣5", "♣6", "♣7", "♣8", "♣9", "♣10", "♣J", "♣Q", "♣K", "♣A", "♥2", "♥3", "♥4", "♥5", "♥6", "♥7", "♥8", "♥9", "♥10", "♥J", "♥Q", "♥K", "♥A", "♦2", "♦3", "♦4", "♦5", "♦6", "♦7", "♦8", "♦9", "♦10", "♦J", "♦Q", "♦K", "♦A" };
        public List<string> Card = new List<string> { };
        public Cards()
        {
            Shuffle();
        }
        private void Shuffle()
        {

            List<int> ints = new List<int>();
            while(Card.Count != BaseCard.Count)
            {
                int random = new Random().Next(52);
                while (ints.Contains(random) == false)
                {
                    ints.Add(random);
                    Card.Add(BaseCard[random]);
                }
            }
        }
    }
}