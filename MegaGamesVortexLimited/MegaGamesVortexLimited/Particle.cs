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
using System.Diagnostics;


namespace dum
{
    public class Particle
    {
        public Texture2D Texture { get; set; }        // Текстура нашей частички
        public Vector2 Position { get; set; }        // Позиция частички
        public Vector2 Velocity { get; set; }        // Скорость частички
        public float Angle { get; set; }            // Угол поворота частички
        public float AngularVelocity { get; set; }    // Угловая скорость
        Color colorTemp;       // Цвет частички
        public float Size { get; set; }                // Размеры
        public float SizeVel { get; set; }		// Скорость уменьшения размера
        public float AlphaVel { get; set; }		// Скорость уменьшения альфы
        public int TTL { get; set; }                // Время жизни частички
        public Color Colordraw;
        public float Alpha;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl, float sizeVel, float alphaVel) // конструктор
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            colorTemp = color;
            AngularVelocity = angularVelocity;
            Size = size;
            SizeVel = sizeVel;
            AlphaVel = alphaVel;
            TTL = ttl;
            Alpha = 1;
        }
        public void Update() // цикл обновления
        {
            TTL--; // уменьшаем время жизни
            // Меняем параметры в соответствии с скоростями
            Position += Velocity;
            Angle += AngularVelocity;
            Size += SizeVel;
            Alpha -= AlphaVel;
            Colordraw = colorTemp * Alpha;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height); // область из текстуры: вся
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2); // центр
            spriteBatch.Draw(Texture, Position, sourceRectangle, Colordraw,
                Angle, origin, Size, SpriteEffects.None, 0); // акт прорисовки
        }
    }
}
