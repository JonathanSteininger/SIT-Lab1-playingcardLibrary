using System;
using System.Drawing;

namespace Lab1_playingcardLibrary
{
    public class PlayingCard : IComparable<PlayingCard>
    {
        public CardRank _rank;
        public CardSuit _suit;
        private int points;
        private bool _isFaceUp;
        private Image _frontImage;
        private Image _backImage;
        private int _id;
        private string _name;
        private string _abbreviation;
        public string Abbreviation { get { return _abbreviation; } }
        public int ID { get { return _id; } }

        public PlayingCard(CardRank Rank, CardSuit Suit)
        {
            _rank = Rank;
            _suit = Suit;
            points = 0;
            _frontImage = null;
            _backImage = null;
            _name = string.Format("{0} of {1}", Rank, Suit);
            SetCardId((int)Rank, (int)Suit);
            _abbreviation = GetAbbreviation;
        }


        private string GetAbbreviation => string.Format("{0}{1}", ((int)_rank < 10 && (int)_rank > 0) ? ((int)_rank + 1).ToString() : _rank.ToString().Substring(0, 1), _suit.ToString().Substring(0, 1));


        private void SetCardId(int Rank, int Suit)
        {
            _id = Rank + Suit * 13;
        }

        public override string ToString()
        {
            return _name;
        }

        public void SetFrontFace(Image image) { _frontImage = image; }
        public void SetBackFace(Image image) { _backImage = image; }

        public Image GetImage { get { return _isFaceUp ? _frontImage : _backImage; } }

        public void Flip() { _isFaceUp = !_isFaceUp; }

        public void SetFaceUp() { _isFaceUp = true; }
        public void SetFaceDown() { _isFaceUp = false; }

        public int CompareTo(PlayingCard card)
        {
            return card.ID.CompareTo(_id);
        }
    }
}
