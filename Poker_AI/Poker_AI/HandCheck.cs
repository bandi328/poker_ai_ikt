using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_AI
{
    public enum HandEnum
    {
        HighCard = 1,
        OnePair = 2,
        TwoPairs = 3,
        ThreeOfAKind = 4,
        Straight = 5,
        Flush = 6,
        FullHouse = 7,
        FourOfAKind = 8,
        StraightFlush = 9,
        RoyalFlush = 10
    }
    public class HandCheck
    {
        private  List<Card> Hands = new List<Card> { };
        public HandEnum CheckHand(List<string> tableAndPlayerHand)
        {
            for (int i = 0; i < tableAndPlayerHand.Count; i++)
            {
                Hands.Add(new Card(tableAndPlayerHand[i][0], ConvertCharToInt(tableAndPlayerHand[i][1])));
            };
            if (IsRoyalFlush())
                return HandEnum.RoyalFlush;
            if (IsStraightFlush())
                return HandEnum.StraightFlush;
            if (IsFourOfAKind())
                return HandEnum.FourOfAKind;
            if (IsFullHouse())
                return HandEnum.FullHouse;
            if (IsFlush())
                return HandEnum.Flush;
            if (IsStraight())
                return HandEnum.Straight;
            if (IsThreeOfAKind())
                return HandEnum.ThreeOfAKind;
            if (IsTwoPairs())
                return HandEnum.TwoPairs;
            if (IsOnePair())
                return HandEnum.OnePair;
            return HandEnum.HighCard;
            //return (int)HandEnum.HighCard;
        }

        private  bool IsRoyalFlush()
        {
            return IsStraightFlush() && Hands.Any(card => card.Value == 14);
        }
        private  bool IsStraightFlush()
        {
            return IsFlush() && IsStraight();
        }
        private  bool IsFourOfAKind()
        {
            return Hands.GroupBy(card => card.Value).Any(group => group.Count() == 4);
        }
        private  bool IsFullHouse()
        {
            return IsThreeOfAKind() && IsOnePair();
        }
        private  bool IsFlush()
        {
            return Hands.GroupBy(card => card.Symbol).Count() == 1;
        }
        private  bool IsStraight()
        {
            var values = Hands.Select(card => card.Value).Distinct().OrderBy(val => val).ToList();

            if (values.Count != 5)
                return false;

            return values.Max() - values.Min() == 4;
        }
        private  bool IsThreeOfAKind()
        {
            return Hands.GroupBy(card => card.Value).Any(group => group.Count() == 3);
        }
        private  bool IsTwoPairs()
        {
            return Hands.GroupBy(card => card.Value).Count(group => group.Count() == 2) == 2;
        }
        private  bool IsOnePair()
        {
            return Hands.GroupBy(card => card.Value).Any(group => group.Count() == 2);
        }
        private  int ConvertCharToInt(char Char)
        {
            if (Char == 'J')
                return 11;
            else if (Char == 'Q')
                return 12;
            else if (Char == 'K')
                return 13;
            else if (Char == 'A')
                return 14;
            else
                return int.Parse(Char.ToString());
        }
    }
    public class Card
    {
        public char Symbol { get; }
        public int Value { get; }

        public Card(char symbol, int value)
        {
            Symbol = symbol;
            Value = value;
        }
    }
}
