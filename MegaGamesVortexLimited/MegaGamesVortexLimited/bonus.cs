using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sprite1;

namespace PlaneGame
{
    class bonusObj
    {
        public Sprite []SpriteObj;
        public Vector2 position;//
        public Vector2 speed;//
        public int With, Height;

        public bool life;
        public int bonusNum;
        public const int MaxBonus=4;
        Random rnd1;
        public int timeSince;
        public int maxTime=10000;//10000


        public bonusObj(ContentManager Content)
        {
             SpriteObj=new Sprite[4];
            for (int i = 0; i < MaxBonus; i++)
            {
               
                SpriteObj[i] = new Sprite();
                SpriteObj[i].Load(Content.Load<Texture2D>("bonus\\"+i));
                
            }
            rnd1 = new Random();
            this.life = false;
            speed.Y = 1;
            speed.X = 0.5f;

        }
        public void Update(int west, int With, int Height, GameTime gameTime)
        {
            this.With = With;
            this.Height = Height;
            this.timeSince += gameTime.ElapsedGameTime.Milliseconds;
            if (this.timeSince > maxTime && this.life==false)
            {
                this.timeSince = 0;
               bonusNum= this.rnd1.Next(0, MaxBonus * 2);
               if (bonusNum < MaxBonus)
               {
                   this.life = true;
                   position.X = this.rnd1.Next(this.SpriteObj[bonusNum].RectangleSprite.Width, With - this.SpriteObj[bonusNum].RectangleSprite.Width);
                   position.Y = 0;
               }
               else { this.life = false; }
            }

            if (this.life == true)
            {
                if (west == 1)
                {
                    if (position.X + SpriteObj[bonusNum].texture.Width < 0)
                    {
                        position.X = this.With + SpriteObj[bonusNum].texture.Width;

                    }

                    position.Y += speed.Y;
                    position.X -= speed.X;
                }
                else 
               {
                   if (position.X - SpriteObj[bonusNum].texture.Width > this.With)
                   {
                       position.X = -SpriteObj[bonusNum].texture.Width;
                       
                   }
                   position.Y += speed.Y;
                   position.X += speed.X;

                }



                if (position.Y >= Height - this.SpriteObj[bonusNum].RectangleSprite.Height / 2)
                { this.life = false; }
            }


          
        }
        public void draw(SpriteBatch spriteBatch)
        {
            if (this.life == true)
            {
                this.SpriteObj[bonusNum].RectangleSprite.X = (int)position.X;
                this.SpriteObj[bonusNum].RectangleSprite.Y = (int)position.Y;
                SpriteObj[bonusNum].DrawSpriteRec(spriteBatch, Color.White);
 

             
 
            }
        }
       
    }
}
