using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PlaneGame
{
    class ProgressBar
    {
        public Rectangle rc, fon;
        protected int maxValue;
        protected int value;
        protected Texture2D texture;
        protected int step;
        public int maxLenght;
        public ProgressBar(Texture2D textur, int height = 30, int value = 0, int maxValue = 100, int maxLenght = 200)
        {
            this.texture = textur;
            this.value = value;
            this.maxValue = maxValue;
            this.maxLenght = maxLenght;
            step = maxLenght / maxValue;
            rc = new Rectangle(0, 0, 1, height);
            fon = new Rectangle(0, 0, this.maxLenght, height);
        }
        public void setValue(int val)
        {
            this.value = val;
            this.rc.Width = (int)(value * step);
            if (this.rc.Width > this.maxLenght)
            {
                this.rc.Width = this.maxLenght;
            }
            if (this.rc.Width < 1)
            {
                this.rc.Width = 1;
            }
        }
        public void Draw(SpriteBatch sb,bool powernut, Color color, Color fonColor,float alpha,int x = 0, int y = 0)
        {
            if (powernut == true)
            {
                fon.X = rc.X = x;
                fon.Y = rc.Y = y;
                sb.Draw(texture, fon, null, fonColor * alpha, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
                sb.Draw(texture, rc, null, color * alpha, 0, Vector2.Zero, SpriteEffects.None, 1);
            }
            else
            {
                fon.X = rc.X = x+this.maxLenght;
                fon.Y = rc.Y = y + this.fon.Height;
                sb.Draw(texture, fon, null, fonColor * alpha, MathHelper.Pi, new Vector2(0, 0), SpriteEffects.None, 0.9f);
                sb.Draw(texture, rc, null, color * alpha, MathHelper.Pi, new Vector2(0, 0), SpriteEffects.None, 1);
            }
        }
    }
}
