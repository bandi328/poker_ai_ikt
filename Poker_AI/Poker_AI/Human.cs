using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_AI
{
    public class Human : Player
    {
        public int Bet(int Money)
        {
            string[] bet = Console.ReadLine().Split("$");
            while ((int.Parse(bet[0]) >= 0 && int.Parse(bet[0]) <= Money) == false)
            {
                Console.WriteLine("Rossz összeget adott meg. Tét: ");
                bet = Console.ReadLine().Split();
            }
            return int.Parse(bet[0]);
        }
    }
}
