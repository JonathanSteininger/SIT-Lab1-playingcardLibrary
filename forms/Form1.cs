using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab1_playingcardLibrary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace forms
{
    public enum CardLocation
    {
        Deck,
        DiscardPile,
        StandardPile,
        Hand1,
        Hand2
    }
    public partial class Form1 : Form
    {
        Random random = new Random();
        private Deck _deck;
        private PlayingHand _hand1;
        private PlayingHand _hand2;
        private Transform _cardTrasnfomrBlueprint;
        public float LerpSmoothness;

        private List<Card> _cards;
        public Card FindCard(PlayingCard card) => _cards.Find(s => s.PlayingCard == card);

        private Point _deckPostion;
        public Point DeckPosition { get { return _deckPostion; } set { MoveDeck(value, 200); } }


        
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Width = 1920;
            Height = 1080;
            BackColor = Color.Green;
            LerpSmoothness = 1f / 60f;
            DoubleBuffered = true;
            SetupGame();
            TimerMain.Start();
        }

        private void SetupGame()
        {
            _hand1 = new PlayingHand((Width / 2), Height / 8 * 7);
            _hand2 = new PlayingHand(Width / 2, Height / 8);
            _deck = new Deck(4, CardRank.Ace, CardRank.King, CardRank.Queen, CardRank.Jack);
            //_deck = new Deck(2);
            _cards = new List<Card>();
            _cardTrasnfomrBlueprint = new Transform(0,0,75,107,1);

            foreach(PlayingCard card in _deck.Cards) { _cards.Add(new Card(card,_cardTrasnfomrBlueprint,this)); }

            SetUpCards();
            SortCardLayers();

            RefreshAllCards();
            DeckPosition = new Point(70,140);
        }

        private void SortCardLayers2Deck()//sorts layers based of _deck card order. will follow the shuffle of the deck.
        {
            _cards.Sort((a, b) =>
            {
               return _deck.Cards.IndexOf(a.PlayingCard).CompareTo(_deck.Cards.IndexOf(b.PlayingCard));

            });
            foreach (Card card in _cards)
            {
                card._Image.BringToFront();
            }
        }

        private void SortCardLayers()//dont use, sorts layers based on card values. wont care about the shuffeled deck
        {
            _cards.Sort((a, b) =>
            {
                return a.PlayingCard.CompareTo(b.PlayingCard);

            });
            foreach(Card card in _cards)
            {
                card._Image.BringToFront();
            }
        }

        public void ShuffleDeck()//shuffles deck
        {
            _deck.Shuffle();
            _deck.Shuffle();
        }

        private void SetUpCards()//first time card setup
        {
            foreach (Card card in _cards)
            {
                card.UpdateImage();
                card.Paint();
                card._Image.MouseClick += MouseClickCard;
                card._Image.MouseEnter += MouseHandHover;
                card._Image.MouseLeave += MouseHandLeave;
            }
            ShuffleDeck();
        }


        private void RefreshAllCards()
        {
            foreach(Card card in _cards)
            {
                card.Paint();
            }
        }


        private async Task MoveCard(Card card, Point destination, int timeMS)
        {
            await CardMovement(card, destination, timeMS);
            return;
        }
        private async Task CardMovement(Card card, Point destination, int timeMS)
        {
            if (timeMS == 0) throw new Exception("timeMS cant be 0. Math error");
            Point StartPosition = new Point(card.transform.Xpos, card.transform.Ypos);
            float time = 0f;

            while(time < timeMS)
            {
                card.transform.Xpos = Lerp(StartPosition.X, destination.X, Math.Min(1f, time / timeMS));
                card.transform.Ypos = Lerp(StartPosition.Y, destination.Y, Math.Min(1f, time / timeMS));
                time += LerpSmoothness * 1000f;
                card.PaintFast();
                await Task.Delay((int)(LerpSmoothness * 1000));
            }
            card.transform.Xpos = destination.X;
            card.transform.Ypos= destination.Y;
            card.PaintFast();

            return;
        }
        private int Lerp(float a, float b, float f)
        {
            return (int) (a + f * (b - a));
        }
        private async void MoveDeck(Point destination, int TimeMS)
        {
            float time = 0f;
            float jumps = TimeMS / _cards.Count;
            int jumpsI = (int)jumps;
            int i = 0;
            while(time < TimeMS && i < _cards.Count)
            {
                //destination = new Point((int)(Width * random.NextDouble()), (int)(Height * random.NextDouble()));
                MoveCard(_cards[i], destination, TimeMS);
                time += jumps;
                i++;
                await Task.Delay(jumpsI);
            }
        }

        private async void DealCard(int speed)//gives a card to player 1
        {
            PlayingCard pcard = _deck.DealTopCard();//gets playingcard from deck. gets removed from _deck
            Card card = FindCard(pcard);//gets Card with the containing Playingcard.

            _hand1.AddCard(pcard, card);// add playingCard and Card to _hand 1

            await MoveCard(card, _hand1.PositionCards, speed);//does animation, waits for it to complete

            card.Location = CardLocation.Hand1;// sets cards location to hand1
            card.transform.ScaleMultiplier = 1.5f;//increases card scale
            card.PlayingCard.SetFaceUp();// makes face up
            card.PaintFast();//paints card again.
            _hand1.UpdateHand();// redraws all cards and positions.

            
        }



        // EVENTS


        private void MouseHandLeave(object sender, EventArgs e)//mouse hover exit playing card
        {
            Card card = _cards.Find(c => c._Image == (PictureBox)sender);
            if (card.Location == CardLocation.Hand1)
            {
                card.transform.Ypos = _hand1.PositionCards.Y;
                card.PaintFast();
            }
        }

        private void MouseHandHover(object sender, EventArgs e)// mouse hover enters playing card 
        {
            Card card = _cards.Find(c => c._Image == (PictureBox)sender);
            if(card.Location == CardLocation.Hand1)
            {
                card.transform.Ypos = _hand1.PositionCards.Y - 30;
                card.PaintFast();
            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)//reset button
        {
            foreach(Card card in _cards)
            {
                card.transform.ScaleMultiplier = 1f;
                card.Location = CardLocation.Deck;
                card.PlayingCard.SetFaceDown();
            }
            int count = _hand1._cards.Count;
            for(int i = 0; i < count; i++)
            {
                _deck.AddCard(_hand1.DealTopCard());
            }
            _deck.Shuffle();
            DeckPosition = new Point(70, 140);
        }

        
        private void MouseClickCard(object sender, MouseEventArgs e)//mouse clicks any card
        {
            Card card = _cards.Find(c => c._Image == (PictureBox)sender);
            if (card.Location == CardLocation.Deck) DealCard(300);
        }

        Card topCard;

        private void TimerTick(object sender, EventArgs e)
        {
            topCard?.PaintFast();
        }
    }
}
