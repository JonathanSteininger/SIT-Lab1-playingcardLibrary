using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_playingcardLibrary
{
    public class Hand
    {
        public List<PlayingCard> Cards;

        public Hand()
        {
            Cards = new List<PlayingCard>();
        }

        public virtual void AddCard(PlayingCard card)
        {
            Cards.Add(card);
        }
        public bool IsEmpty => Cards.Count <= 0;

        public virtual PlayingCard DealTopCard()
        {
            if (IsEmpty) return null;

            PlayingCard temp = Cards[Cards.Count - 1];
            Cards.RemoveAt(Cards.Count - 1);
            return temp;
        }

        public virtual void Sort()
        {
            Cards.Sort();
        }

    }
    
}
