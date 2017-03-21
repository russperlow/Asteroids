using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Asteroids
{
    enum GameStates
    {
        MainMenu,
        GamePlaying,
        GameOver
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Ship ship;
        BulletManager bm;
        AsteroidManager am;
        KeyboardState kbState;
        SpriteFont font;
        Texture2D background;
        GameStates currState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            currState = GameStates.MainMenu;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>(@"Font");
            // TODO: use this.Content to load your game content here
            bm = new BulletManager(Content.Load<Texture2D>(@"Bolt"), Content.Load<SoundEffect>(@"Kachow"));
            ship = new Ship(Content.Load<Texture2D>(@"McQueen"), Content.Load<Texture2D>(@"McQueenBlue"), GraphicsDevice.Viewport, bm, font);
            am = new AsteroidManager(Content, GraphicsDevice.Viewport, bm, ship);
            background = Content.Load<Texture2D>(@"Radiator_Springs");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            kbState = Keyboard.GetState();

            switch (currState) {
                case GameStates.MainMenu:
                    if (kbState.IsKeyDown(Keys.Space))
                        currState = GameStates.GamePlaying;
                    break;
                case GameStates.GamePlaying:
                    bm.Update(gameTime);
                    am.Update(gameTime);
                    ship.Update(gameTime, kbState);
                    if (ship.Lives <= 0)
                        currState = GameStates.GameOver;            
                    break;
                case GameStates.GameOver:
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        currState = GameStates.MainMenu;
                        am.Reset();
                        ship.Reset();
                    }
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            switch (currState)
            {
                case GameStates.MainMenu:
                    spriteBatch.DrawString(font, "Press Space to Play Game", new Vector2((GraphicsDevice.Viewport.Width / 2) - (font.MeasureString("Press Space to Play Game").Length() / 2), GraphicsDevice.Viewport.Height / 2), Color.White);
                    break;
                case GameStates.GamePlaying:
                    bm.Draw(spriteBatch);
                    am.Draw(spriteBatch);
                    ship.Draw(spriteBatch);
                    break;
                case GameStates.GameOver:
                    spriteBatch.DrawString(font, "Game Over! Final Score: " + ship.Score + "\nPress Space to Continue" , new Vector2((GraphicsDevice.Viewport.Width / 2) - (font.MeasureString("Game Over! Final Score: " + ship.Score).Length() / 2), GraphicsDevice.Viewport.Height / 2), Color.White);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
