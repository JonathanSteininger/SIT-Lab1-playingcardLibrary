using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq.Expressions;

namespace Lab1_playingcardLibrary {
    public class Deck
    {
        private static Random _random = new Random();

        public List<PlayingCard> Cards;

        private int _deckCount;

        private CardRank[] _ranks;

        public Deck(int DeckCount, params CardRank[] ranks)
        {
            Cards = new List<PlayingCard>();
            _ranks = ranks;
            for (int i = 0; i < DeckCount; i++)
            {
                AddDeck();
            }
            _deckCount = DeckCount;
        }
        public Deck(params CardRank[] ranks) : this(1, ranks) { }
        public Deck(int DeckCount) : this(DeckCount, (CardRank[])Enum.GetValues(typeof(CardRank))) { }
        public Deck() : this(1) { }


        public void AddCard(PlayingCard card)
        {
            Cards.Add(card);
        }




        private void AddDeck()
        {
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))//loops through all Suits in the enum.
            {
                foreach (CardRank rank in _ranks)//Loops through all faces in the enum.
                {
                    Cards.Add(new PlayingCard(rank, suit));//fills the Cards with the current suit and rank.
                }
            }
        }


        public PlayingCard DealTopCard()//return the last card in the Cards list
        {
            if (IsEmpty) return null;
            PlayingCard temp = Cards[Cards.Count - 1];
            Cards.RemoveAt(Cards.Count - 1);
            return temp;
        }

        public PlayingCard TopCard()
        {
            if (IsEmpty) return null;
            return Cards[Cards.Count - 1];
        }

        public bool IsEmpty => Cards.Count <= 0;

        public void Shuffle()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                int pos = _random.Next(i);

                PlayingCard temp = Cards[i];
                Cards[i] = Cards[pos];
                Cards[pos] = temp;
            }

        }


        public void ApplyFaces(List<Image> faces, Image back)
        {
            if (faces.Count * _deckCount != Cards.Count) throw new Exception("DeckSize to facesCount missmatch");
            for(int i = 0; i < Cards.Count; i++) {
                Cards[i].SetFrontFace(faces[i % faces.Count]);
                Cards[i].SetBackFace(back);
            }
        }
        
        
    }
}
