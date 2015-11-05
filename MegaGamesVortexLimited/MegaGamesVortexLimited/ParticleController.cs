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

namespace dum
{
    class ParticleController
    {
        public List<Particle> particles;
        private Texture2D smoke; // текстура дыма
        private Random random;
        public Color color;
        public float kofAlpha;
        public ParticleController()
        {
            kofAlpha = 1;
            color = Color.White;
            this.particles = new List<Particle>();
            random = new Random();
        }
        public void LoadContent(ContentManager Manager)
        {
            smoke = Manager.Load<Texture2D>("smoke");
        }
        public void EngineRocket(Vector2 position,float tempAngale) // функция, которая будет генерировать частицы
        {
            for (int a = 0; a < 1; a++) // создаем 2 частицы дыма для трейла
            {
                Vector2 velocity = AngleToV2((float)(2d * random.NextDouble()), 0.6f);
                float angle = tempAngale;
                float angleVel = 0;
                float size = 1f;
                int ttl = 100;
                float alphaVel = random.Next(1, 3)/(100f*kofAlpha);
                float sizeVel = (float)(1f / random.Next(50, 300));
                GenerateNewParticle(smoke, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }
        private Particle GenerateNewParticle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl, float sizeVel, float alphaVel) // генерация новой частички
        {
            Particle particle = new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl, sizeVel, alphaVel);
            particles.Add(particle);
            return particle;
        }
        public void Update(GameTime gameTime)
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].Alpha <= 0) // если время жизни частички или её размеры равны нулю, удаляем её
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++) // рисуем все частицы
            {
                particles[index].Draw(spriteBatch);
            }
        }
        public Vector2 AngleToV2(float angle, float length)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)Math.Cos(angle) * length / 4;
            direction.Y = -(float)Math.Sin(angle) * length / 4;
            return direction;
        }
    }
}
