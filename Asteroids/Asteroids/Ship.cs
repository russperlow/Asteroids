using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids
{
    /// <summary>
    /// The player/ship handles shooting, movement, drawing
    /// </summary>
    class Ship
    {
        // FIELDS
        float rotation, speed;
        Vector2 position;
        Texture2D texture;
        Viewport viewport;
        BulletManager bm;
        SpriteFont font;
        const int MAX_SPEED = 10;
        int lives;
        int score;

        // PROPERTIES
        public Vector2 Position { get { return position; } set { position = value; } }
        public Rectangle Rect { get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); } }
        public int Lives { get { return lives; } set { lives = value; } }
        public int Score { get { return score; } set { score = value; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">The ships texture</param>
        /// <param name="viewport">Gives dimensons to help with wrapping</param>
        public Ship(Texture2D texture, Viewport viewport, BulletManager bm, SpriteFont font)
        {
            this.texture = texture;
            this.viewport = viewport;
            this.bm = bm;
            this.font = font;
            position = new Vector2(viewport.Width / 2, viewport.Height / 2);
            speed = 0;
            rotation = 0;
            lives = 3;
            score = 0;
        }
        
        // METHODS

        /// <summary>
        /// Updates the spaceships properties
        /// </summary>
        public void Update(GameTime gameTime, KeyboardState kbState)
        {
            Wrap();

            // Move forward and back or decelerate if neither
            if(kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
            {
                if(speed < MAX_SPEED)
                    speed += .05f;
                
            }
            else
            {
                if (speed > 1 || speed > -1)
                    speed *= .95f;
                else
                    speed = 0f;
            }

            // Rotate left and right
            if(kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left))
            {
                rotation += .1f;
            }
            else if(kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
            {
                rotation -= .1f;
            }

            position += new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * speed;

            // Shoot a bullet
            if (kbState.IsKeyDown(Keys.Space))
            {
                Vector2 direction = position;
                direction.Normalize();
                bm.AddBullet(position, new Vector2((float)Math.Cos(rotation) * 3, (float)Math.Sin(rotation) * 3), rotation, gameTime);
            }

        }

        /// <summary>
        /// Draws the spaceship
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch object to allow for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), .75f, SpriteEffects.None, 1f);

            // Draw the lives remaining 
            for(int i = 0; i < lives; i++)
            {
                spriteBatch.Draw(texture, new Rectangle(i * texture.Width / 2, 0, texture.Width / 2, texture.Height / 2), Color.White);
            }

            // Draw the score
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, texture.Height * 3 / 4), Color.White);
        }

        /// <summary>
        /// Makes sure the spaceship wraps if out of screen bounds
        /// </summary>
        public void Wrap()
        {
            // Checks left and right wrapping
            if(position.X < 0)
            {
                position.X = viewport.Width;
            }
            else if(position.X > viewport.Width)
            {
                position.X = 0;
            }

            // Checks top and bottom wrapping
            if(position.Y < 0)
            {
                position.Y = viewport.Height;
            }
            else if(position.Y > viewport.Height)
            {
                position.Y = 0;
            }
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        public void Reset()
        {
            position = new Vector2(viewport.Width / 2, viewport.Height / 2);
            speed = 0;
            rotation = 0;
            lives = 3;
            score = 0;
        }
    }
}
