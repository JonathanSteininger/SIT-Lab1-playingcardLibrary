using Lab1_playingcardLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forms
{
    internal class PlayingHand : Hand
    {
        public Point PositionCards;

        public List<Card> _cards;
        public PlayingHand(Point CardsLocation) : base() { 
            _cards = new List<Card>();
            PositionCards = CardsLocation;
        }

        public override PlayingCard DealTopCard()
        {
            PlayingCard temp = base.DealTopCard();
            _cards.Remove(_cards.Find(s => s.PlayingCard == temp));
            return temp;
        }

        public void AddCard(PlayingCard card, Card card2)
        {
            base.AddCard(card);
            _cards.Add(card2); 
            
        }



        public void UpdateHand()
        {
            int offset = 30;
            int HandRange = offset * _cards.Count;

            int min = PositionCards.X - HandRange / 2;
            
            Sort();
            int i = 0;
            foreach(Card card in _cards)
            {
                if (card.Location != CardLocation.Hand1) continue;
                i++;
                card.transform.Xpos = min + offset * i;
                card.PaintFast();
            }
            foreach(Card card in _cards) if(card.Location == CardLocation.Hand1) card.PaintFast();
            //_cards[0]?.Paint();
        }
        public override void Sort()
        {
            base.Sort();
            sortToValues();
        }
        private void sortToValues()
        {
            _cards.Sort((a, b) =>
            {
                return a.PlayingCard.CompareTo(b.PlayingCard);

            });
            BringCardsToFront();
        }
        private void sortToLayers()
        {
            _cards.Sort((a, b) =>
            {
                int i = a._Image.Parent.Controls.GetChildIndex(a._Image);
                int j = b._Image.Parent.Controls.GetChildIndex(b._Image);
                return j.CompareTo(i);
            });
        }
        public void BringCardsToFront()
        {
            foreach (Card card in _cards)
            {
                card._Image.BringToFront();
            }
        }

        public PlayingHand(int Xpos, int Ypos) : this(new Point(Xpos, Ypos)) { }
    }
}
