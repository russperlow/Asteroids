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
        const int MAX_SPEED = 10;
        const int FULL_RECHARGE = 1000;
        Vector2 position;
        Texture2D texture, blueText, redText;
        Viewport viewport;
        BulletManager bm;
        SpriteFont font;
        float rotation, speed, recharge;
        int lives, score;
        bool invincible;

        // PROPERTIES
        public Vector2 Position { get { return position; } set { position = value; } }
        public Rectangle Rect { get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); } }
        public int Lives { get { return lives; } set { lives = value; } }
        public int Score { get { return score; } set { score = value; } }
        public bool Invincible { get { return invincible; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">The ships texture</param>
        /// <param name="viewport">Gives dimensons to help with wrapping</param>
        public Ship(Texture2D redText, Texture2D blueText, Viewport viewport, BulletManager bm, SpriteFont font)
        {
            // Set all constructor params
            this.redText = redText;
            this.blueText = blueText;
            this.viewport = viewport;
            this.bm = bm;
            this.font = font;
            
            // Init variables 
            position = new Vector2(viewport.Width / 2, viewport.Height / 2);
            speed = 0;
            rotation = 0;
            lives = 3;
            score = 0;
            texture = redText;
            invincible = false;
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

                // Changes the bullet color to match the car
                Color color = Color.White;
                if (invincible)
                    color = Color.Blue;

                bm.AddBullet(position, new Vector2((float)Math.Cos(rotation) * (speed + 2), (float)Math.Sin(rotation) * (speed + 2)), rotation, color);
            }

            // Handles the invincible bonus recharging and usage
            if (!invincible)
            {
                if (recharge == FULL_RECHARGE && kbState.IsKeyDown(Keys.R))
                {
                    invincible = true;
                    texture = blueText;
                }
                else
                {
                    if (recharge > FULL_RECHARGE)
                        recharge = FULL_RECHARGE;
                    else
                        recharge += gameTime.ElapsedGameTime.Milliseconds / 10;
                }
            }
            else
            {
                if(recharge > 0)
                {
                    recharge -= 1f;
                }
                else
                {
                    invincible = false;
                    texture = redText;
                }
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
                spriteBatch.Draw(redText, new Rectangle(i * texture.Width / 2, 0, texture.Width / 2, texture.Height / 2), Color.White);
            }

            // Draw the recharging car
            spriteBatch.Draw(blueText, new Rectangle(viewport.Width - blueText.Width, 0, (int)(texture.Width * (recharge / 1000)) / 2, texture.Height / 2), new Rectangle(0, 0, (int)(texture.Width * (recharge / 1000)), texture.Height), Color.White);

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
