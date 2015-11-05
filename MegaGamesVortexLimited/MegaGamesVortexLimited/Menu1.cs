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
using PlaneGame;

namespace PlaneGame
{
    class Menu1
    {
        public int CurPosMenu;//поточна позиція курсору меню
        public int CurPosOptions;//Поточна позиція курсору опцій
        int NumPosMenu = 4;
        int NumPosOptions = 3;
        public Vector2 position;
        public Sprite cursor;
        public Texture2D help;
        public bool FirstTimePressMenu = true;
        public bool FirstTimePressOptions = true;
        public bool FirstTimeEnterOptions;
        public bool FirstTimeEnterMenu;
        public Vector2[] Resolution;
        public int ResolutionCount=4;
        public int ResolutionIndex=1; // одиниця щоб не змінювалось розширення коли не треба
        GameFont fontTexture;
        public Menu1()
        {

            cursor = new Sprite();
            position.X = 200;
            position.Y = 300;
            Resolution = new Vector2[ResolutionCount];
            for (int i = 0; i < ResolutionCount; i++)
            {
                 Resolution[i] = new Vector2();
            }     
        }
        void SetResolutionValue()
        {
            this.Resolution[0] = (new Vector2(800, 600));
            this.Resolution[1] = (new Vector2(1024, 768));
            this.Resolution[2] = (new Vector2(1280, 760));
            this.Resolution[3] = (new Vector2(1360, 768));
        }
        public void Load(ContentManager content)
        {
            cursor.Load(content.Load<Texture2D>("cursor"));
            help = content.Load<Texture2D>("Help");
            fontTexture = new GameFont();
            fontTexture.Load(content);
            SetResolutionValue();
        }

        public void OptionsUpdate()
        {
            var key = Keyboard.GetState();
            if (FirstTimeEnterOptions ) { CurPosOptions = 0; position.Y = 300;}
            if (key.IsKeyDown(Keys.Up) && FirstTimePressOptions)
            {
                FirstTimePressOptions = false;
                CurPosOptions--;
                position.Y -= 50;
                if (CurPosOptions < 0) { CurPosOptions = NumPosOptions; position.Y = 450; }
            }
            if (key.IsKeyDown(Keys.Down) && FirstTimePressOptions)
            {
                FirstTimePressOptions = false;
                CurPosOptions++;
                position.Y += 50;
                if (CurPosOptions > NumPosOptions) { CurPosOptions = 0; position.Y = 300; }
            }
            if (key.IsKeyUp(Keys.Up) && key.IsKeyUp(Keys.Down)) { FirstTimePressOptions = true; }
            if (key.IsKeyUp(Keys.Enter)) { FirstTimeEnterOptions = false; }
        }
        public void MenuUpdate()
        {
            var key = Keyboard.GetState();
            if (FirstTimeEnterMenu) { CurPosMenu = 0; position.Y = 300;}
            if (key.IsKeyDown(Keys.Up) && FirstTimePressMenu)
            {
                FirstTimePressMenu = false;
                CurPosMenu--;
                position.Y -= 50;
                if (CurPosMenu < 0) { CurPosMenu = NumPosMenu; position.Y = 500; }
            }
            if (key.IsKeyDown(Keys.Down) && FirstTimePressMenu)
            {
                FirstTimePressMenu = false;
                CurPosMenu++;
                position.Y += 50;
                if (CurPosMenu > NumPosMenu) { CurPosMenu = 0; position.Y = 300; }
            }
            if (key.IsKeyUp(Keys.Up) && key.IsKeyUp(Keys.Down)) { FirstTimePressMenu = true; }
            if (key.IsKeyUp(Keys.Enter)) { FirstTimeEnterMenu = false; }
        }
        public void DrawOptions(SpriteBatch spriteBatch, int GameRounds)
        {
            fontTexture.DrawString(spriteBatch, "Rounds   < " + GameRounds + " >", new Vector2(250, 285), Color.DeepPink, 0.3f, null, 0);
            fontTexture.DrawString(spriteBatch, "Resolution   < " + Resolution[ResolutionIndex].X + " x " + Resolution[ResolutionIndex].Y + " >", new Vector2(250, 335), Color.DeepPink, 0.3f, null, 0);
            fontTexture.DrawString(spriteBatch, "-----", new Vector2(250, 385), Color.DeepPink, 0.3f, null, 0);
            fontTexture.DrawString(spriteBatch, "Accept changes", new Vector2(250, 435), Color.DeepPink, 0.3f, null, 0);
            this.cursor.RectangleSprite.X = (int)position.X;
            this.cursor.RectangleSprite.Y = (int)position.Y;
            this.cursor.DrawSpriteRec(spriteBatch, Color.White);
        }
        public void DrawMenu(SpriteBatch spriteBatch)
        {
             fontTexture.DrawString(spriteBatch,"Game", new Vector2(250, 285),Color.DeepPink,0.3f,null,0);
             fontTexture.DrawString(spriteBatch, "Options", new Vector2(250, 335), Color.DeepPink, 0.3f, null, 0);
             fontTexture.DrawString(spriteBatch, "Help", new Vector2(250, 385), Color.DeepPink, 0.3f, null, 0);
             fontTexture.DrawString(spriteBatch, "About autors", new Vector2(250, 435), Color.DeepPink, 0.3f, null, 0);
             fontTexture.DrawString(spriteBatch, "Exit", new Vector2(250, 485), Color.DeepPink, 0.3f, null, 0);
             this.cursor.RectangleSprite.X = (int)position.X;
             this.cursor.RectangleSprite.Y = (int)position.Y;
             this.cursor.DrawSpriteRec(spriteBatch, Color.White);
        }
        public void DrawHelp(SpriteBatch spriteBatch)
        {
 
        }
        public void DrawAutor(SpriteBatch spriteBatch)
        {
            fontTexture.DrawString(spriteBatch, "By Zhila Bogdan & Duhnitskiy Oleg", new Vector2(10, 10), Color.Wheat, 0.3f, null, 0);
            fontTexture.DrawString(spriteBatch, "special for project practicum", new Vector2(10, 50), Color.Wheat, 0.3f, null, 0);
        }
    }
}
