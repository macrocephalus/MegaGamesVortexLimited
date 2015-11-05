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
    class ColishunMeneger
    {
        protected int OneColishinPlane;//перевірка шо зіткнувся один раз
        public Vector2[] PointL;
        public Vector2[] PointR;
        public ColishunMeneger()
        {
            OneColishinPlane = 0;
            PointL = new Vector2[16];
            PointR = new Vector2[16];
        }
        public float square(Vector2 poin1, Vector2 poin2, Vector2 poinT3) ///(x1, y1, x2, y2, x3, y3)
        {
            return (Math.Abs(poin2.X * poinT3.Y - poinT3.X * poin2.Y - poin1.X * poinT3.Y + poinT3.X * poin1.Y + poin1.X * poin2.Y - poin2.X * poin1.Y)) / 2;
        }
        public void Rotate(ref float x, ref float y, float cx, float cy, float fi)
        {
            float tx;
            x = x - cx;
            y = y - cy;
            // поворот
            tx = x;
            x = (int)(tx * Math.Cos(fi) + y * Math.Sin(fi));
            y = (int)(-tx * Math.Sin(fi) + y * Math.Cos(fi));
            // перенос обратно на место
            x = x + cx;
            y = y + cy;
        }
        public bool Intersection(Rectangle L, float rotationLeft, Rectangle R, float rotationRight)
        {
            rotationLeft = MathHelper.TwoPi - rotationLeft;
            rotationRight = MathHelper.TwoPi - rotationRight;
            Vector2 RCentr, LCentr;
            LCentr = new Vector2(L.Center.X, L.Center.Y);
            RCentr = new Vector2(R.Center.X, R.Center.Y);

            PointL[0] = new Vector2(L.X, L.Y);
            PointL[1] = new Vector2(L.X + L.Width, L.Y);
            PointL[2] = new Vector2(L.X, L.Y + L.Height);
            PointL[3] = new Vector2(L.X + L.Width, L.Y + L.Height);
            PointL[4] = new Vector2(L.X + L.Width / 2, L.Y);
            PointL[5] = new Vector2(L.X + L.Width, L.Y + L.Height / 2);
            PointL[6] = new Vector2(L.X + L.Width / 2, L.Y + L.Height);
            PointL[7] = new Vector2(L.X, L.Y + L.Height / 2);
            PointL[8] = new Vector2(L.X + L.Width / 4, L.Y);
            PointL[9] = new Vector2(L.X + L.Width / 4 + L.Width / 2, L.Y);
            PointL[10] = new Vector2(L.X + L.Width, L.Y + L.Height / 4);
            PointL[11] = new Vector2(L.X + L.Width, L.Y + L.Height / 2 + L.Height / 4);
            PointL[12] = new Vector2(L.X + L.Width / 2 + L.Width / 4, L.Y + L.Height);
            PointL[13] = new Vector2(L.X + L.Width / 4, L.Y + L.Height);
            PointL[14] = new Vector2(L.X, L.Y + L.Height / 2 + L.Height / 4);
            PointL[15] = new Vector2(L.X, L.Y + L.Height / 4);

            PointR[0] = new Vector2(R.X, R.Y);
            PointR[1] = new Vector2(R.X + R.Width, R.Y);
            PointR[2] = new Vector2(R.X, R.Y + R.Height);
            PointR[3] = new Vector2(R.X + R.Width, R.Y + R.Height);
            PointR[4] = new Vector2(R.X + R.Width / 2, R.Y);
            PointR[5] = new Vector2(R.X + R.Width, R.Y + R.Height / 2);
            PointR[6] = new Vector2(R.X + R.Width / 2, R.Y + R.Height);
            PointR[7] = new Vector2(R.X, R.Y + R.Height / 2);
            PointR[8] = new Vector2(R.X + R.Width / 4, R.Y);
            PointR[9] = new Vector2(R.X + R.Width / 4 + R.Width / 2, R.Y);
            PointR[10] = new Vector2(R.X + R.Width, R.Y + R.Height / 4);
            PointR[11] = new Vector2(R.X + R.Width, R.Y + R.Height / 2 + R.Height / 4);
            PointR[12] = new Vector2(R.X + R.Width / 2 + R.Width / 4, R.Y + R.Height);
            PointR[13] = new Vector2(R.X + R.Width / 4, R.Y + R.Height);
            PointR[14] = new Vector2(R.X, R.Y + R.Height / 2 + R.Height / 4);
            PointR[15] = new Vector2(R.X, R.Y + R.Height / 4);

            for (int i = 0; i < 16; i++)
            {
                Rotate(ref PointL[i].X, ref PointL[i].Y, LCentr.X, LCentr.Y, rotationLeft);
                Rotate(ref PointR[i].X, ref PointR[i].Y, RCentr.X, RCentr.Y, rotationRight);
            }
            float SRecL = L.Width * L.Height;
            float Ssyma = 0f;
            for (int i = 0; i < 16; i++)
            {
                Ssyma = 0;
                Ssyma = square(PointL[0], PointL[8], PointR[i]) + square(PointL[8], PointL[4], PointR[i]) + square(PointL[4], PointL[9], PointR[i]) + square(PointL[9], PointL[1], PointR[i]) +
                square(PointL[1], PointL[10], PointR[i]) + square(PointL[10], PointL[5], PointR[i]) + square(PointL[5], PointL[11], PointR[i]) + square(PointL[11], PointL[3], PointR[i]) +
                square(PointL[3], PointL[12], PointR[i]) + square(PointL[12], PointL[6], PointR[i]) + square(PointL[6], PointL[13], PointR[i]) + square(PointL[13], PointL[2], PointR[i]) +
                square(PointL[2], PointL[14], PointR[i]) + square(PointL[14], PointL[7], PointR[i]) + square(PointL[7], PointL[15], PointR[i]) + square(PointL[15], PointL[0], PointR[i]);
                if (Ssyma <= SRecL)
                {
                    return true;
                }
            }
            return false;
        }
        public void Colishun(ref Plane Left, ref Plane Right, Rectangle Tower, ref bonusObj Bonus)
        {
            Rectangle RectanLeft, RectanRight;
            Rectangle RecTower;
            RecTower = new Rectangle(Tower.X + 8, Tower.Y + 5, Tower.Width-20, Tower.Height);
            Rectangle RecBonus = new Rectangle((int)Bonus.position.X- Bonus.SpriteObj[0].texture.Width/2, (int)Bonus.position.Y - Bonus.SpriteObj[0].texture.Height/2, Bonus.SpriteObj[0].RectangleSprite.Width, Bonus.SpriteObj[0].RectangleSprite.Height);
            RectanLeft = new Rectangle(Left.sprite.RectangleSprite.X - Left.sprite.RectangleSprite.Width / 2, Left.sprite.RectangleSprite.Y - Left.sprite.RectangleSprite.Height / 2, Left.sprite.RectangleSprite.Width, Left.sprite.RectangleSprite.Height - 10);
            RectanRight = new Rectangle(Right.sprite.RectangleSprite.X - Right.sprite.RectangleSprite.Width / 2, Right.sprite.RectangleSprite.Y - Right.sprite.RectangleSprite.Height / 2, Right.sprite.RectangleSprite.Width, Right.sprite.RectangleSprite.Height - 10);
            if (Intersection(RecBonus, 0, RectanLeft, Left.rotation) && Left.alive == true && Bonus.life == true)// лівий з бонусом
            {
                switch (Bonus.bonusNum)
                {
                    case 0: Left.life += 50; break;
                    case 1: Left.SetSpeedBonus(); break;
                    case 2: Left.SetBulletHarm(); break;
                    case 3: { Left.undead = true; Left.TimePrevBonus = 0; }; break;  
                }
                Bonus.life = false;
            }
            if (Intersection(RecBonus, 0, RectanRight, Right.rotation) && Right.alive == true && Bonus.life == true)
            {
                switch (Bonus.bonusNum)
                {
                    case 0: Right.life += 50; break;
                    case 1: Right.SetSpeedBonus(); break;
                    case 2: Right.SetBulletHarm(); break;
                    case 3: { Right.undead = true; Right.TimePrevBonus = 0; }; break;
                }
                Bonus.life = false;
            }
            if (Left.alive == false) { Left.NormalizeBulletHarm(); }
            if (Right.alive == false) { Right.NormalizeBulletHarm(); }
            if (Intersection(RecTower, 0, RectanLeft, Left.rotation) && Left.alive == true)
            {
                Left.alive = false;
            }
            if (Intersection(RecTower, 0, RectanRight, Right.rotation) && Right.alive == true)
            {
                Right.alive = false;
            }
            for (int i = 0; i < Right.CoutBullet; i++) // червоний стріляє в зелений
            {
                if (Intersection(RecTower, 0, Right.AreaBalls[i].SpriteBulet.RectangleSprite, Right.AreaBalls[i].rotation) && Right.AreaBalls[i].alive == true)
                // кулі червоного в башню посеред карти
                {
                    Right.AreaBalls[i].alive = false;
                }
                if (Intersection(RectanLeft, Left.rotation, Right.AreaBalls[i].SpriteBulet.RectangleSprite, Right.AreaBalls[i].rotation) && Right.AreaBalls[i].alive == true)
                {
                    if (Left.undead == false)
                    {
                        Left.life -= Right.bulletKick;
                    }
                    Right.AreaBalls[i].alive = false;
                }
            }
            for (int i = 0; i < Left.CoutBullet; i++) // зелений стріляє в червоний
            {
                if (Intersection(RecTower, 0, Left.AreaBalls[i].SpriteBulet.RectangleSprite, Left.AreaBalls[i].rotation) && Left.AreaBalls[i].alive == true)
                // кулі зеленого в башню посеред карти
                {
                    Left.AreaBalls[i].alive = false;
                }
                // зелений стріляє в червоний
                if (Intersection( RectanRight, Right.rotation,Left.AreaBalls[i].SpriteBulet.RectangleSprite, Left.AreaBalls[i].rotation) && Left.AreaBalls[i].alive == true)
                {
                    if (Right.undead == false)
                    {
                        Right.life -= Left.bulletKick;
                    }
                    Left.AreaBalls[i].alive = false;
                }
            }
            if (Intersection(RectanLeft, Left.rotation, RectanRight, Right.rotation))
            {
                if (Left.alive == true && Right.alive == true && this.OneColishinPlane == 0)
                {
                    if (Left.undead == false)
                    {
                        Left.life -= 25;
                    }
                    if (Right.undead == false)
                    {
                        Right.life -= 25;
                    }
                    ++this.OneColishinPlane;
                }
            }
            else
            {
                this.OneColishinPlane = 0;
            }
        }
    }
}
