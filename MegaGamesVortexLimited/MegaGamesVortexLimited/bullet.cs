using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Sprite1;

namespace PlaneGame
{
    class bullet
    {
        public Sprite SpriteBulet;
        public Vector2 position;
        public float rotation;
        public Vector2 velocity;
        public bool alive;

        public bullet(Texture2D loadedTexture)
        {
            SpriteBulet = new Sprite();
            rotation = 0.0f;
            position = Vector2.Zero;
            SpriteBulet.Load(loadedTexture);
            velocity = Vector2.Zero;
            alive = false;
        }
    }
}
