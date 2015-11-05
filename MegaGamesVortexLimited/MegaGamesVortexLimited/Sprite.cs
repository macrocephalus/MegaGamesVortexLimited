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

namespace Sprite1
{
    class Sprite
    {
        public Texture2D texture;
        public Rectangle RectangleSprite;//координати спрайта и розмір
        public Vector2 Center;//центер статичного спрайту
        //для анімації
        public Vector2 frameSize;// розмір фрейма наприклад (75, 75)
        Vector2 currentFrame;// вибраний фрейм
        Vector2 sheetSize; // кількість фреймів в спрайті наприклад (6, 8)
        //час для фреймів
        int timeSinceLastFrame;// прошло з остнього визову
        int millisecondsPerFrame;// на один кадер даеться 
        public int replay;//скіки раз треба повторювати анімацію якщо 0 то весь час
        public int CurrentReplay;//скіки раз треба повторювати анімацію якщо 0 то весь час

        public Sprite()
        {
            this.Center = Vector2.Zero;
            this.RectangleSprite = Rectangle.Empty;
            this.replay = 0;
            CurrentReplay = 0;
        }
        private void CenterSprite()
        {
            this.Center = new Vector2(this.RectangleSprite.Center.X, this.RectangleSprite.Center.Y);

        }
        public void Load(Texture2D texture, int X = 0, int Y = 0)
        {
            this.texture = texture;
            this.RectangleSprite = new Rectangle(X, Y, (int)(this.texture.Width), (int)(this.texture.Height));
            CenterSprite();
        }
        public void LoadAnimation(Texture2D texture, int Row, int Column, int WithFrame, int HeightFrame, int milisecond = 42, int X = 0, int Y = 0)
        {
            this.sheetSize.X = Row;
            this.sheetSize.Y = Column;
            this.frameSize.X = WithFrame;
            this.frameSize.Y = HeightFrame;
            this.currentFrame.X = 0;
            this.currentFrame.Y = 0;
            this.millisecondsPerFrame = milisecond;
            this.texture = texture;
            this.RectangleSprite = new Rectangle(X, Y, WithFrame, HeightFrame);
            CenterSprite();
        }
        public void zeroFrame()
        {
            currentFrame.X = 0;
            currentFrame.Y = 0;
            this.CurrentReplay = 0;
        }
        public void DrawSpriteRec(SpriteBatch spriteBatch, Color color, float rotation = 0, SpriteEffects spriteefect = SpriteEffects.None, float layer = 0)
        {
            spriteBatch.Draw(texture, RectangleSprite, null, color, rotation, this.Center, spriteefect, 0);
        }
        public void Update()
        {
            ++currentFrame.X;
            if (currentFrame.X >= sheetSize.X)
            {
                currentFrame.X = 0;
                ++currentFrame.Y;
                if (currentFrame.Y >= sheetSize.Y) { currentFrame.Y = 0; }
            }
        }
        public void Update(GameTime gameTime, int rep = 0)
        {
            this.replay = rep;
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                        ++this.CurrentReplay;
                    }
                }
            }
        }
        public void Update(int Row, int Column)
        {
            int tempRow = Row;
            int tempColumn = Column;
            if (tempRow <= 0 || tempRow > sheetSize.X) { tempRow = 1; }
            if (Column <= 0 || Column > sheetSize.Y) { tempColumn = 1; }
            currentFrame.X = tempRow - 1;
            currentFrame.Y = tempColumn - 1;
        }
        public void Update(bool RowColumn, int Picition, GameTime gameTime)// якщо trye то row(рядок) иначе column (стовпчик)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            int TempPos = Picition;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                if (RowColumn == true && (TempPos > sheetSize.Y || TempPos <= 0)) { TempPos = 1; }
                if (RowColumn == false && (TempPos > sheetSize.X || TempPos <= 0)) { TempPos = 1; }
                if (RowColumn)
                {
                    currentFrame.Y = TempPos - 1;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X) { currentFrame.X = 0; }
                }
                else
                {
                    currentFrame.X = TempPos - 1;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }
        public void DrawAnimation(SpriteBatch spriteBatch, Color color, float rotation = 0, SpriteEffects spriteefect = SpriteEffects.None, float layer = 0)
        {
            if (this.replay == 0)
            {
                Rectangle frame = new Rectangle((int)(currentFrame.X * frameSize.X), (int)(currentFrame.Y * frameSize.Y), (int)frameSize.X, (int)frameSize.Y);
                spriteBatch.Draw(texture, RectangleSprite, frame, color, rotation, this.Center, spriteefect, layer);//доробити центр
            }
            else
            {
                if (this.CurrentReplay < this.replay)
                {
                    Rectangle frame = new Rectangle((int)(currentFrame.X * frameSize.X), (int)(currentFrame.Y * frameSize.Y), (int)frameSize.X, (int)frameSize.Y);
                    spriteBatch.Draw(texture, RectangleSprite, frame, color, rotation, this.Center, spriteefect, layer);//доробити центр
                }
            }
        }
    }
}

