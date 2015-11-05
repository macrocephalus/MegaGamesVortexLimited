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
using dum;

namespace PlaneGame
{
    class Plane
    {
        ProgressBar pogres;
        ParticleController particleController;
        public int CoutBullet = 10; //кулі
        public bullet[] AreaBalls;
        public Sprite sprite;
        public Vector2 position;
        public float rotation;
        public float speed;
        public float earth;
        public bool alive;
        protected float With, Height;
        public float RotationAngle = 0.03f;
        public float FallenSpeed = 0.05f;
        public float MaxSpeed = 6.0f;
        public float BonusRotationAngle = 1;
        public float BonusSpeed = 1.5f;// бонусна швидкість
        public float MinSpeed = -0.2f;// відємне значення для того щоб літак не смикавсяя при падінні
        public bool Left;
        Sprite boom;
        Sprite shield;
        float layerbullet = 0.3f, layerPlane = 0.4f, layerEfect = 0.5f;
        public int life;
        SpriteEffects efect = new SpriteEffects();//зеркальне відображення
        public bool alreadydie;
        public bool IsSpeedBonus;
        public int bulletKick = 5;
        int timeSince;//накопичуємо час
        public Color colorbulet;
        public Vector2 SmoukePosition;
        public bool undead;// Чи літак бесмертний тру так фелс ні
        public int TimePrevBonus, timeBonus = 15000;
        public bool MenuGameBlock;
        public int timeSinceLastbullet, millisecondsPerBullet=100;//стрільба кулею скіки пройшло і скільки нада
       
        public Plane(ContentManager content, bool rol, float With, float Height) 
        {
            MenuGameBlock = true;
            particleController = new ParticleController();
            SmoukePosition = new Vector2();
            AreaBalls = new bullet[CoutBullet];
            for (int i = 0; i < CoutBullet; i++)
            {
                AreaBalls[i] = new bullet(
                content.Load<Texture2D>("bullet"));
            }
            boom = new Sprite();
            shield = new Sprite();
            shield.LoadAnimation(content.Load<Texture2D>("shield"), 4, 1, 70, 70, 42);
            this.pogres = new ProgressBar(content.Load<Texture2D>("ProgresBar"),15);
            this.Left = rol;
            this.With = With;
            this.Height = Height;
            sprite = new Sprite();
            particleController.LoadContent(content);
            if (this.Left == true)
            {
                sprite.LoadAnimation(content.Load<Texture2D>("result1"), 1, 1, 65, 30, 42);
            }
            else
            {
                sprite.LoadAnimation(content.Load<Texture2D>("result2"), 1, 1, 65, 30, 42);
            }
            this.boom.LoadAnimation(content.Load<Texture2D>(@"boom"), 8, 7, 128, 128, 42);
            earth = 30;
            if (this.Left == true)
            {
                this.efect = SpriteEffects.None;
            }
            if (this.Left == false)
            {
                this.efect = SpriteEffects.FlipVertically;
            }
            this.Start();
        }
        public void SetSpeedBonus()//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            IsSpeedBonus = true;
            BonusRotationAngle = 2f;
            BonusSpeed = 1.5f;

        }
        public void NormalizeSpeedBonus()//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            IsSpeedBonus = false;
            BonusRotationAngle = 1f;
            BonusSpeed = 1;
        }
        public void SetBulletHarm()
        {
            this.bulletKick = 10;
            this.colorbulet = Color.Red;
        }
        public void NormalizeBulletHarm()
        {
            this.bulletKick = 5;
            this.colorbulet = Color.White;
        }
        public void Start()
        {
            undead = false;
            this.colorbulet = Color.White;
            NormalizeSpeedBonus();
            this.alreadydie = false;
            this.alive = true;
            this.speed = 1.5f;
            if (this.Left == true)
            {
                this.position = new Vector2(50, this.Height - earth);
                sprite.RectangleSprite.X = (int)this.position.X;
                sprite.RectangleSprite.Y = (int)this.position.Y;
                this.rotation = 0;
            }
            else 
            {
                this.position = new Vector2(this.With - 50, this.Height - earth);
                sprite.RectangleSprite.X = (int)this.position.X;
                sprite.RectangleSprite.Y = (int)this.position.Y;
                this.rotation = MathHelper.Pi;
            }
            this.life = 100;
        }
        public void setMenuBlock(bool temp)
        {
            MenuGameBlock = temp;
        }
        public void fly()
        {
            if (this.speed > 0)
            { this.position += (this.speed) * new Vector2((float)Math.Cos(this.rotation), (float)Math.Sin(this.rotation)); } //переміщуємось 
            if (this.speed <= 0)
            {
                this.fallen(); //падаєм
            }
            if (this.rotation > 5 * MathHelper.Pi / 4 && this.rotation < 7 * MathHelper.Pi / 4 && this.speed > MinSpeed)//зменшується швидкість при напрямі вгору
            { this.speed = this.speed - FallenSpeed-0.03f; }
            if (this.rotation < 3 * MathHelper.Pi / 4 && this.rotation > MathHelper.Pi / 4 && this.speed < MaxSpeed)// збільшується швидкість при напрямі вниз
            { this.speed = this.speed + FallenSpeed+ 0.02f; }
            // придумати формулу яка буде збільшуваи швидкість в прогресії
            if (this.speed <= 12) { this.speed = (this.speed * 0.99f + 0.055f * BonusSpeed); }
        }
        public void die(GameTime gameTime)
        {
               //літак стає неживим при відповідному куті нахилу і розташуванні на землі
            // УЙОБОК!!!! Зроби шоб вони нормально розбивались! Якого хрена він робить перевірку то з кутом повороту то з пі????
               if ((this.rotation > 10 * RotationAngle) && (this.rotation <= 2 * MathHelper.Pi - MathHelper.Pi / 4) && (this.position.Y >= this.Height - this.earth) && this.Left == true)
               { this.alive = false; }
               if (((this.rotation < MathHelper.Pi - 12 * RotationAngle && this.rotation > 0) ||
                   (this.rotation > MathHelper.Pi + 12 * RotationAngle && this.rotation <= 2 * MathHelper.Pi)) && 
                   (this.position.Y >= this.Height - this.earth) && this.Left == false)
           { this.alive = false;}
               if (this.alive == false) { this.life = 0; this.undead = false; }
               if (this.life <= 0)
               {
                   this.timeSince += gameTime.ElapsedGameTime.Milliseconds;
                   this.alive = false;
                   this.life = 0;
                   if (this.timeSince > 3000)
                   {
                       Start();
                       this.timeSince = 0;  
                   }
               }
             if (this.life >100)
            {
                this.alive = true;
                this.life = 100;
            }
        }
        public void fallen()
        {
            if (this.position.Y < this.Height - this.earth)// літак не падає нижче землі
            {
                if (this.rotation <= 3 * MathHelper.Pi / 2 && this.rotation >= MathHelper.Pi) 
                {
                    this.rotation -= 0.01f;//змінюється кут 
                    //вектор падіння знаходиться сумою векторів напряму польоту і "вниз"
                    this.position += Vector2.Add((this.speed) * new Vector2((float)Math.Cos(this.rotation), (float)Math.Sin(this.rotation)), (-0.4f) * new Vector2(3f, -6f));
                }
                if (this.rotation <= MathHelper.Pi && this.rotation >= MathHelper.Pi / 2)
                {
                    this.rotation -= 0.01f;
                    this.position += Vector2.Add((this.speed) * new Vector2((float)Math.Cos(this.rotation), (float)Math.Sin(this.rotation)), (-0.4f) * new Vector2(3f, -6f));
                }
                if (this.rotation <= MathHelper.Pi / 2 && this.rotation >= 0)
                {
                    this.rotation += 0.01f;
                    this.position += Vector2.Add((this.speed) * new Vector2((float)Math.Cos(this.rotation), (float)Math.Sin(this.rotation)), (-0.4f) * new Vector2(-3f, -6f));
                }
                if (this.rotation <= 2 * MathHelper.Pi && this.rotation >= 3 * MathHelper.Pi / 2)
                {
                    this.rotation += 0.01f;
                    this.position += Vector2.Add((this.speed) * new Vector2((float)Math.Cos(this.rotation), (float)Math.Sin(this.rotation)), (-0.4f) * new Vector2(-3f, -6f));
                }
            }
        }
        public void BulletShot(GameTime gameTime)// куля
        {
            foreach (var x in AreaBalls)
            {
                if (x.alive)
                {
                    x.position += x.velocity;
                    if (x.position.X > this.With || x.position.X < 0 || x.position.Y > this.Height - this.earth || x.position.Y < 0)
                    {
                        x.alive = false;
                    }
                }
            }
        }
        public void UpdatePlane(GameTime gameTime,float WidthT,float HeightT)
        {
            this.With = WidthT;
            this.Height = HeightT;
            var kb = Keyboard.GetState();
            if (this.alive)//живем
            {
                if (MenuGameBlock)
                {
                    if ((this.Left == true && kb.IsKeyDown(Keys.D)) || (this.Left == false && kb.IsKeyDown(Keys.Right)))
                    {
                        this.rotation += RotationAngle * BonusRotationAngle;//повертаєм
                    }
                    if ((this.Left == true && kb.IsKeyDown(Keys.A)) || (this.Left == false && kb.IsKeyDown(Keys.Left)))
                    {
                        this.rotation -= RotationAngle * BonusRotationAngle;//повертаєм
                    }
                    if ((this.Left == true && kb.IsKeyDown(Keys.W)) || (this.Left == false && kb.IsKeyDown(Keys.Up)))
                    {
                        fly();
                    }
                    if ((this.Left == true && kb.IsKeyUp(Keys.W)) || (this.Left == false && kb.IsKeyUp(Keys.Up)))
                    {
                        this.fallen();
                        if (this.speed > 1.5f)
                        { 
                            this.speed = this.speed - 0.05f;
                        }
                    }
                    if ((this.Left == true && kb.IsKeyDown(Keys.LeftControl)) || (this.Left == false && kb.IsKeyDown(Keys.RightControl)))
                    {
                        this.timeSinceLastbullet += gameTime.ElapsedGameTime.Milliseconds;
                        if (this.timeSinceLastbullet > this.millisecondsPerBullet)
                        {
                             this.timeSinceLastbullet = 0;
                             foreach (var x in AreaBalls)
                             {
                                if (!x.alive)
                                {
                                    x.alive = true;
                                    x.position = this.position;
                                    x.rotation = this.rotation;
                                    x.velocity = 20.0f * new Vector2((float)Math.Cos(this.rotation), (float)Math.Sin(this.rotation));
                                    break;
                                }
                            }
                        }
                    }
                }
            sprite.Update(gameTime);
            }
            else //розбиваємся
            {
                this.fallen();
                sprite.Update(1,1);
            }
            //if (kb.IsKeyDown(Keys.R)) { this.Start(); }
            BulletShot(gameTime);
            die(gameTime);// перевіряємо чи випадково він не мертвий
            if (this.alive)
            {
                this.boom.zeroFrame();
            }
            else
            {
                this.boom.Update(gameTime,1);
            }
            // обмежує можливість повертати при старті
            if (this.Left == true)
            {
                if ((this.rotation < 2 * MathHelper.Pi - MathHelper.Pi / 10 && this.rotation > 2 * MathHelper.Pi - MathHelper.Pi / 4) && (this.position.Y >= this.Height - this.earth) && this.alive == true)
                {
                    this.rotation = 2 * MathHelper.Pi - MathHelper.Pi / 10;
                } 
                if ((this.rotation > 7 * RotationAngle && this.rotation < 10 * RotationAngle) && (this.position.Y >= this.Height - this.earth) && this.alive == true)
                {
                    this.rotation = 7 * RotationAngle;
                }
            }
                else 
            {
                if ((this.rotation < MathHelper.Pi - 8 * RotationAngle && this.rotation > 0) && (this.position.Y >= this.Height - this.earth) && this.alive == true)
                {
                    this.rotation = MathHelper.Pi - 8 * RotationAngle;
                }
                if ((this.rotation >= MathHelper.Pi + 8 * RotationAngle && this.rotation <= 2 * MathHelper.Pi) && (this.position.Y >= this.Height - this.earth) && this.alive == true)
                {
                    this.rotation = MathHelper.Pi + 8 * RotationAngle;
                }
            }
            if (this.rotation > 2 * MathHelper.Pi) this.rotation = 0; // перевіряє чи є літак в межах від 0 до 2 пі
            if (this.rotation < 0) this.rotation = 2 * MathHelper.Pi; // (для того щоб літак крутився правильно)
            if (this.position.Y < 0) { this.position.Y = 0; }
            if (this.position.Y >= Height - this.earth) { this.position.Y = Height - this.earth; } // літак не палдає нижче землі
            if (this.position.X < 0) { this.position.X = this.With; }
            if (this.position.X > this.With) { this.position.X = 0; }
            if (IsSpeedBonus == false)
            {
                if (this.life > 70) { this.particleController.color = Color.White; this.particleController.kofAlpha = 0.5f; }
                else if (this.life > 35) { this.particleController.color = Color.Gray; this.particleController.kofAlpha = 2; }
                else if (this.life >= 0) { this.particleController.color = Color.Black; this.particleController.kofAlpha = 3; }
            }
            else
            {
                if (this.life > 70) {  this.particleController.kofAlpha = 0.5f; }
                else if (this.life > 35) { this.particleController.kofAlpha = 2; }
                else if (this.life >= 0) { this.particleController.kofAlpha = 3; }
                this.particleController.color = Color.Yellow;
            }
                if (this.undead == true)
                {
                    shield.Update(gameTime);
                    this.TimePrevBonus+= gameTime.ElapsedGameTime.Milliseconds;
                    if (this.TimePrevBonus > this.timeBonus)
                {
                this.undead = false;
                this.TimePrevBonus = 0;
                }
                this.shield.RectangleSprite.X = (int)this.position.X;
                this.shield.RectangleSprite.Y = (int)this.position.Y;
            }
            particleController.EngineRocket(new Vector2(this.SmoukePosition.X, this.SmoukePosition.Y),this.rotation);
            particleController.Update(gameTime);
            this.pogres.setValue(this.life);
        }
        public void drawPlane(SpriteBatch spriteBatch)
        {
        foreach (bullet ball in AreaBalls)
        {
            if (ball.alive)
            {
                ball.SpriteBulet.RectangleSprite.X = (int)ball.position.X;
                ball.SpriteBulet.RectangleSprite.Y = (int)ball.position.Y;
                if (this.Left == true)
                {
                    ball.SpriteBulet.DrawSpriteRec(spriteBatch, colorbulet, ball.rotation, SpriteEffects.None, this.layerbullet);
                }
                else
                {
                    ball.SpriteBulet.DrawSpriteRec(spriteBatch, colorbulet, ball.rotation, SpriteEffects.FlipHorizontally, this.layerbullet);
                }
            }
        }
        sprite.RectangleSprite.X =(int)this.position.X;
        sprite.RectangleSprite.Y = (int)this.position.Y;
        if (this.alive)
        {sprite.DrawAnimation(spriteBatch, Color.White, this.rotation,efect,this.layerPlane);}
        else
        { 
        sprite.DrawAnimation(spriteBatch, Color.Black, this.rotation,efect,this.layerPlane);
        this.boom.RectangleSprite.X = (int)this.position.X;
        this.boom.RectangleSprite.Y = (int)this.position.Y;
        this.boom.DrawAnimation(spriteBatch, Color.White,0,SpriteEffects.None,this.layerEfect);
        }
        if (this.Left)
        {
            this.pogres.Draw(spriteBatch, true,Color.Green, Color.LightGreen,0.5f,10,30);
        }
        else
        {
            this.pogres.Draw(spriteBatch, false, Color.Red, Color.LightPink,0.5f,(int)this.With - this.pogres.maxLenght - 10, 30);
        }
        particleController.Draw(spriteBatch);
        if (this.undead == true)
        {
            this.shield.DrawAnimation(spriteBatch, Color.Wheat, 0, SpriteEffects.None, 0.8f);
        }
        }
    }
 }
