using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sprite1;

namespace PlaneGame
{
    class WorldObj
    {
        public Sprite SpriteObj;
        public Vector2 position;
        public float rotation;
        public bool alive;

        public  WorldObj()
        {
            SpriteObj = new Sprite();
            this.alive=true;
            this.rotation = 0;
        }
        public void Load(Texture2D texture)
        {
            SpriteObj.LoadAnimation(texture, 3, 1, 60, 180, 100);
        }
        public void Update(GameTime gameTime,float With, float Height)
        {
            this.position.X = With / 2;
            this.position.Y = Height - SpriteObj.frameSize.Y/2-10;
            SpriteObj.Update(gameTime, 0);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteObj.RectangleSprite.X = (int)this.position.X;
            SpriteObj.RectangleSprite.Y = (int)this.position.Y;
            SpriteObj.DrawAnimation(spriteBatch, Color.White, this.rotation, SpriteEffects.None, 0.3f);
        }
    }
}
