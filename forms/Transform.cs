using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forms
{
    public class Transform
    {
        private int _width;
        private int _height;
        public int Width { get{ return (int)((float)_width * ScaleMultiplier); } set { _width = value; } }
        public int Height { get { return(int)((float) _height * ScaleMultiplier); } set { _width = value; } }
        public int Xpos;
        public int Ypos;

        public float ScaleMultiplier;

        public int[] Corner => new int[2] { Xpos - Width / 2, Ypos - Height / 2 };
        public Transform(int Xpos, int Ypos, int Width, int Height, float ScaleMultiplier = 1)
        {
            _width = Width;
            _height = Height;
            this.Xpos = Xpos;
            this.Ypos = Ypos;
            this.ScaleMultiplier = ScaleMultiplier;
        }
        public Transform(Transform transform) : this(transform.Xpos, transform.Ypos, transform.Width, transform.Height, transform.ScaleMultiplier) { }
        public Transform() : this(0,0,10,10,1) { }
    }
}
