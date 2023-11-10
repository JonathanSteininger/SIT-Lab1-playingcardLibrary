using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab1_playingcardLibrary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace forms
{
    public class Card
    {
        public PlayingCard PlayingCard { get; set; }

        public Transform transform { get; set; }

        public PictureBox _Image { get; set; }

        public CardLocation Location { get; set; }
        public Card(PlayingCard card, Transform transform, Form1 parentForm) {
            PlayingCard = card;
            this.transform = new Transform(transform);
            setupImage(parentForm);
            Location = CardLocation.Deck;
        }
        public Card(CardRank rank, CardSuit suit, Form1 parentForm) : this(new PlayingCard(rank, suit), new Transform(), parentForm) { }
        public Card(CardRank rank, CardSuit suit, Transform transform, Form1 parentForm) : this(new PlayingCard(rank, suit), transform, parentForm) { }
        public Card(Form1 parentForm) : this(new PlayingCard(CardRank.Ace, CardSuit.Heart), new Transform(), parentForm) { }

        private void setupImage(Form1 parentForm)
        {
            _Image = new PictureBox();
            _Image.Paint += Image_Paint;
            _Image.BorderStyle = BorderStyle.FixedSingle;
            parentForm.Controls.Add(_Image);
            _Image.MouseClick += _Image_MouseClick;
            //_Image.MouseEnter += _Image_MouseEnter;
            
        }

        private void _Image_MouseEnter(object sender, EventArgs e)
        {
            PlayingCard.Flip();
            Paint();
        }

        private void _Image_MouseClick(object sender, MouseEventArgs e)
        {
            //transform.ScaleMultiplier = 2f;
            //PlayingCard.Flip();
            //Paint();
        }

        public void Paint()
        {
            UpdatePictureBoxTransform();
            _Image.Refresh();
            Form1.ActiveForm?.Refresh();
        }
        public void PaintFast()
        {
            UpdatePictureBoxTransform();
            _Image.Refresh();
        }
        private void Image_Paint(object sender, PaintEventArgs e)
        {
            
            _Image.Image = PlayingCard.GetImage;
            Graphics g = e.Graphics;
            if (PlayingCard.GetImage == null)
            {
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, transform.Width, transform.Height);//draws rectange if no image available
                g.DrawString(PlayingCard.Abbreviation, new Font("Times New Roman", 12.0f),new SolidBrush(Color.Black), new PointF(0f, 0f));
            }
            else g.DrawImage(PlayingCard.GetImage, 0, 0, transform.Width, transform.Height);//draws card image
            
        }

        private void UpdatePictureBoxTransform()
        {
            _Image.Location = new Point(transform.Corner[0], transform.Corner[1]);
            _Image.Width = transform.Width;
            _Image.Height = transform.Height;
        }

        public void UpdateImage()
        {
            try
            {
                string file = string.Format("./cardimages/front/{0}s-{1}-75.png", PlayingCard._suit.ToString().ToLower(), ((int)PlayingCard._rank < 10 && (int)PlayingCard._rank > 0) ? ((int)PlayingCard._rank + 1).ToString() : PlayingCard._rank.ToString().Substring(0, 1).ToLower());
                PlayingCard.SetFrontFace(Image.FromFile(file));
                PlayingCard.SetBackFace(Image.FromFile(string.Format("./cardimages/back/back-blue-75-1.png")));
            }catch (Exception ex)
            {
                throw ex;
            }
        }

    }
    
}
