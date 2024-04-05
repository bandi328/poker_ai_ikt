using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_AI
{
    public class AI
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

        public int Hands()
        {

        }
    }
}
