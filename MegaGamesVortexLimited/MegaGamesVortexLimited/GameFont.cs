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
using PlaneGame;
using Sprite1;

namespace PlaneGame
{
    class GameFont
    {
        SpriteFont fontTexture;
        public GameFont()
        { }
        public void Load(ContentManager Content)
        {
            fontTexture = Content.Load<SpriteFont>("Texture");
        }
        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color colr, float scale, Color? border, int BorderSize)
        {
            if (border != null)
            {
                spriteBatch.DrawString(fontTexture, text, position - new Vector2(BorderSize, 0), (Color)border, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
                spriteBatch.DrawString(fontTexture, text, position - new Vector2(0, BorderSize), (Color)border, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
                spriteBatch.DrawString(fontTexture, text, position + new Vector2(BorderSize, 0), (Color)border, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
                spriteBatch.DrawString(fontTexture, text, position + new Vector2(0, BorderSize), (Color)border, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
            }
            spriteBatch.DrawString(fontTexture, text, position, colr, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
        }
    }
}
