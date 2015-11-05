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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch; 
        public enum GameState //менюшка
        {
            menu,
            game,
            options,
            help,
            autors,
        }
        public GameState CurrentGameState = GameState.menu;
        Texture2D backgroundTexture;
        Rectangle viewportRect;
        Plane plane, plane2;
        GameFont fontTexture;
        Vector2 scoredraiv;
        ColishunMeneger bang;
        WorldObj Town;//башня
        float LayerFon = 0.1F;
        int wind;//вітер 0 на захід 1 на схід
        Random rnd1 = new Random();
        cloud tych;
        Menu1 menu;
        int Rscore, Lscore;//score
        int Rounds = 15; // кількість раундів, які буде тривати гра
        bonusObj Bonus;
        bool IsKeyDawn;
        int TimeSince;
        int TimMenu = 7000;//затримка меню
        bool IsChangedResolution;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = true;
        }
        protected override void Initialize()
        {
            base.Initialize();
            wind = rnd1.Next(0, 2);
            scoredraiv = new Vector2(0.1f, 0.1f);//text  
        }
        protected override void LoadContent()
        {
            Window.AllowUserResizing = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontTexture = new GameFont();
            fontTexture.Load(Content);
            plane = new Plane(Content, true, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            plane2 = new Plane(Content, false, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            viewportRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            backgroundTexture = Content.Load<Texture2D>("sky2");
            Town = new WorldObj();
            Town.Load(Content.Load<Texture2D>("basna"));
            tych = new cloud();
            tych.Load(Content, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            Bonus = new bonusObj(Content);
            menu = new Menu1();
            bang = new ColishunMeneger();
            menu.Load(Content);
        }
        protected override void Update(GameTime gameTime)
        {
            this.viewportRect.Width = graphics.GraphicsDevice.Viewport.Width;
            this.viewportRect.Height = graphics.GraphicsDevice.Viewport.Height;
            var klava = Keyboard.GetState();
            switch (CurrentGameState)
            {
                case GameState.menu://менюшка
                        menu.MenuUpdate();
                        if (menu.CurPosMenu == 0 && klava.IsKeyDown(Keys.Enter) && menu.FirstTimeEnterMenu == false) { CurrentGameState = GameState.game; }
                        if (menu.CurPosMenu == 1 && klava.IsKeyDown(Keys.Enter) && menu.FirstTimeEnterMenu == false)
                        {menu.FirstTimeEnterOptions = true; CurrentGameState = GameState.options;}
                        if (menu.CurPosMenu == 2 && klava.IsKeyDown(Keys.Enter) && menu.FirstTimeEnterMenu == false) { CurrentGameState = GameState.help; }
                        if (menu.CurPosMenu == 3 && klava.IsKeyDown(Keys.Enter) && menu.FirstTimeEnterMenu == false) { CurrentGameState = GameState.autors; }
                        if (menu.CurPosMenu == 4 && klava.IsKeyDown(Keys.Enter) && menu.FirstTimeEnterMenu == false) { this.Exit(); }
                break;

                case GameState.help:
                    if (klava.IsKeyDown(Keys.Escape)) { CurrentGameState = GameState.menu; } 
                break;

                case GameState.autors:
                if (klava.IsKeyDown(Keys.Escape)) { CurrentGameState = GameState.menu; }
                break;

                case GameState.options://опції
                       menu.OptionsUpdate();
                       if (menu.CurPosOptions == 0 && klava.IsKeyDown(Keys.Left) && IsKeyDawn==false) 
                       { 
                           Rounds--; IsKeyDawn = true;
                           if (Rounds <= 1) { Rounds = 1; }
                       }
                       if (menu.CurPosOptions == 0 && klava.IsKeyDown(Keys.Right) && IsKeyDawn == false) { Rounds++; IsKeyDawn = true; }
                       if (menu.CurPosOptions == 1 && klava.IsKeyDown(Keys.Left) && IsKeyDawn==false) 
                       {
                           menu.ResolutionIndex--; IsKeyDawn = true; IsChangedResolution = true; 
                           if (menu.ResolutionIndex <= 0) { menu.ResolutionIndex = 0; }

                       }
                       if (menu.CurPosOptions == 1 && klava.IsKeyDown(Keys.Right) && IsKeyDawn == false) 
                       {
                           menu.ResolutionIndex++; IsKeyDawn = true;
                           if (menu.ResolutionIndex >= menu.ResolutionCount - 1) { menu.ResolutionIndex = menu.ResolutionCount - 1; IsChangedResolution = true; }
                       }
                       if (menu.CurPosOptions == 2 && klava.IsKeyDown(Keys.Enter) && menu.FirstTimeEnterOptions == false) { }
                       if (menu.CurPosOptions == 3 && klava.IsKeyDown(Keys.Enter) && menu.FirstTimeEnterOptions == false) { 
                           menu.FirstTimeEnterMenu = true; CurrentGameState = GameState.menu;
                           graphics.PreferredBackBufferWidth = (int)menu.Resolution[menu.ResolutionIndex].X;
                           graphics.PreferredBackBufferHeight = (int)menu.Resolution[menu.ResolutionIndex].Y;
                           graphics.ApplyChanges();
                       }
                       if (klava.IsKeyDown(Keys.Escape) && menu.FirstTimeEnterOptions == false) 
                       {
                           menu.FirstTimeEnterMenu = true; CurrentGameState = GameState.menu;
                       }
                       if (klava.IsKeyUp(Keys.Left) && klava.IsKeyUp(Keys.Right)) { IsKeyDawn = false; }
                break;
                case GameState.game:
                    if (menu.CurPosMenu == 0 && klava.IsKeyDown(Keys.Escape)) { CurrentGameState = GameState.menu; }
                    tych.Update(wind, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
                    Town.Update(gameTime, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
                    Bonus.Update(wind, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height, gameTime);
                    bang.Colishun(ref plane,
                                  ref plane2,
                                  new Rectangle((int)Town.position.X - Town.SpriteObj.RectangleSprite.Width / 2,
                                                (int)Town.position.Y - Town.SpriteObj.RectangleSprite.Height / 2,
                                                (int)Town.SpriteObj.RectangleSprite.Width,
                                                (int)Town.SpriteObj.RectangleSprite.Height),
                                  ref Bonus);
                    plane.SmoukePosition = bang.PointL[7];
                    plane2.SmoukePosition = bang.PointR[7];
                    if (plane.alive == false && plane.alreadydie == false)
                    {
                        ++Rscore;
                        plane.alreadydie = true;
                    }
                    if (plane2.alive == false && plane2.alreadydie == false)
                    {
                        ++Lscore;
                        plane2.alreadydie = true;
                    }
                    if (Lscore >= Rounds || Rscore >= Rounds)
                    {

                        TimeSince += gameTime.ElapsedGameTime.Milliseconds;
                        if (TimeSince > TimMenu)
                        {
                            TimeSince = 0;
                            CurrentGameState = GameState.menu;
                            Lscore = 0;
                            plane2.Start();
                            Rscore = 0;
                            plane.Start();
                        }
                    }
                    else
                    {
                        plane.setMenuBlock(true);
                        plane2.setMenuBlock(true);
                    }
                    plane.UpdatePlane(gameTime, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
                    plane2.UpdatePlane(gameTime, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
                    if (IsChangedResolution) { plane.Start(); plane2.Start(); IsChangedResolution = false; }
                break;
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            switch (CurrentGameState)
            {
                case GameState.menu:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    menu.DrawMenu(spriteBatch);
                    spriteBatch.Draw(backgroundTexture, viewportRect, Color.White);
                    spriteBatch.End();
                    break;
                case GameState.help:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    menu.DrawHelp(spriteBatch);
                    spriteBatch.Draw(menu.help, viewportRect, Color.White);
                    spriteBatch.End();
                    break;
                case GameState.autors:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    menu.DrawAutor(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.options:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    menu.DrawOptions(spriteBatch, Rounds);
                    spriteBatch.Draw(backgroundTexture, viewportRect, Color.White);
                    spriteBatch.End();
                    break;
                case GameState.game:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    if (TimeSince < TimMenu)
                    {
                        plane.setMenuBlock(false);
                        plane2.setMenuBlock(false);
                        if (Rscore >= Rounds && Lscore >= Rounds && Rscore == Lscore)
                        {
                            fontTexture.DrawString(spriteBatch,"Drawn game", new Vector2(150, 300), Color.Tomato, 1f, Color.Yellow, 2);
                        }
                        else
                        {
                            if (Rscore >= Rounds) 
                            { 
                                fontTexture.DrawString(spriteBatch,"Red Win", new Vector2(150, 300), Color.Green, 1f, Color.Red, 1);
                            }
                            if (Lscore >= Rounds) 
                            { 
                                fontTexture.DrawString(spriteBatch,"Green Win", new Vector2(150, 300), Color.Red, 1f, Color.Green, 1);
                            }
                        }
                    }
                    spriteBatch.Draw(backgroundTexture, viewportRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, this.LayerFon);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    fontTexture.DrawString(spriteBatch,"Score:" + this.Lscore, new Vector2(10, 60), Color.Green, 0.3f, null, 0);
                    fontTexture.DrawString(spriteBatch,"Score:" + this.Rscore, new Vector2(graphics.GraphicsDevice.Viewport.Width - 140, 60), Color.Red, 0.3f, null, 0);
                    plane.drawPlane(spriteBatch);
                    plane2.drawPlane(spriteBatch);
                    Bonus.draw(spriteBatch);
                    Town.Draw(spriteBatch);
                    tych.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }
                    base.Draw(gameTime);
        }
        
    }
}
