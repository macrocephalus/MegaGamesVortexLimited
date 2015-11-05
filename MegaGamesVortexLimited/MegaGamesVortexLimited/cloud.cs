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
    class cloudOBJ
    {
        public Sprite SpriteObj;
        public Vector2 position;
        public Vector2 speed;
        public cloudOBJ()
        {
            SpriteObj = new Sprite();
        }
    }
    class cloud
    {
        public int minSpead=1,maxSpead=3,maxPozitionY=450;
        public int With, Height;
        public int cout=30;
        cloudOBJ[] masCloud;
        Random rnd1;
        public cloud()
        {
            rnd1 = new Random();
            masCloud = new cloudOBJ[cout];
        }
        public void Load(ContentManager content,int with,int heigft)
        {
            this.With = with;
            this.Height = heigft;
            for (int i = 0; i < cout; i++)
            {
                masCloud[i] = new cloudOBJ();
                int frame = (i % 10);
                masCloud[i].SpriteObj.Load(content.Load<Texture2D>("tychi\\" + frame));
                masCloud[i].position.X = rnd1.Next(0, this.With);
                masCloud[i].position.Y = rnd1.Next(0,(int)(maxPozitionY - masCloud[i].SpriteObj.frameSize.Y));
                masCloud[i].speed.X  = rnd1.Next(this.minSpead,this.maxSpead);
            }
        }
        public void Update(int west,int With,int Height )
        {
            this.With = With;
            this.Height = Height;
            if (west==1)
            {
                for (int i = 0; i < cout; i++)
                {
                    if (masCloud[i].position.X +masCloud[i].SpriteObj.texture.Width < 0)
                    {
                        masCloud[i].position.X = this.With + masCloud[i].SpriteObj.frameSize.X + masCloud[i].SpriteObj.texture.Width;
                        masCloud[i].speed.X  = rnd1.Next(this.minSpead,this.maxSpead);
                    }
                    masCloud[i].position.X -= masCloud[i].speed.X;
                }
            }
            else
            {
                for (int i = 0; i < cout; i++)
                {
                    if (masCloud[i].position.X - masCloud[i].SpriteObj.texture.Width> this.With)
                    {
                        masCloud[i].position.X = -masCloud[i].SpriteObj.texture.Width;
                        masCloud[i].speed.X = rnd1.Next(this.minSpead, this.maxSpead);
                    }
                    masCloud[i].position.X += masCloud[i].speed.X;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (cloudOBJ xmara in masCloud)
            {
                xmara.SpriteObj.RectangleSprite.X = (int)xmara.position.X;
                xmara.SpriteObj.RectangleSprite.Y = (int)xmara.position.Y;
                xmara.SpriteObj.DrawSpriteRec(spriteBatch, Color.White, 0, SpriteEffects.None, 0.9f);
            }
        }
    }
}


