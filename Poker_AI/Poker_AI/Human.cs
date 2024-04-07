using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_AI
{
    public class Human : Player
    {
        public int Bet(int money, int minToBet, bool mustCall)
        {
            // [összeg, ami egyenlő vagy nagyobb mint a tét] - tartás/emelés
            // 0 - passz (ha lehet)
            // [üresen hagyás] - eldobás


            int intBet = 1;
            string[] bet;

            while ((intBet >= 0 && intBet <= money) == false || (intBet < minToBet && (intBet != 0 || mustCall)))
            {
                Console.Write("\nLicitösszeg: ");
                bet = Console.ReadLine().Split("$");
                if(int.TryParse(bet[0], out int result))
                    intBet = int.Parse(bet[0]);
                else
                {
                    if (bet[0] == "")
                        return -1;
                }

                if (intBet < 0)
                {
                    Console.WriteLine($"Negatív tét nem adható meg.");
                }
                else if (intBet < minToBet && intBet != 0)
                {
                    Console.WriteLine($"Rossz összeget adott meg. Adja meg legalább a minimum tétnek megfelelő összeget ({minToBet}$).");
                        if(!mustCall)
                            Console.WriteLine($"0-t is megadhat ha nem kíván kockáztatni");
                    Console.WriteLine("Eldobáshoz hagyja üresen a mezőt.");
                }
                else if(intBet > money)
                {
                    Console.WriteLine("Nincs elegendő tőke a tét megadásához. All-in? (y/n)");
                    char yn;
                    do
                    {
                        yn = char.Parse(Console.ReadLine());
                    } while (yn != 'y' && yn != 'n');
                    if(yn == 'y')
                        intBet = money;

                  
                }
                
            } 
            return intBet;
        }
    }
}
