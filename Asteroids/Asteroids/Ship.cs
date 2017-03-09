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
        KeyboardState kbState;
        Viewport viewport;
        BulletManager bm;
        const int MAX_SPEED = 10;

        // PROPERTIES
        public Vector2 Position { get { return position; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">The ships texture</param>
        /// <param name="viewport">Gives dimensons to help with wrapping</param>
        public Ship(Texture2D texture, Viewport viewport, BulletManager bm)
        {
            this.texture = texture;
            this.viewport = viewport;
            this.bm = bm;
            position = new Vector2(viewport.Width / 2, viewport.Height / 2);
            speed = 0;
            rotation = 0;
        }
        
        // METHODS

        /// <summary>
        /// Updates the spaceships properties
        /// </summary>
        public void Update()
        {
            kbState = Keyboard.GetState();
            Wrap();

            // Move forward and back or decelerate if neither
            if(kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
            {
                if(speed < MAX_SPEED)
                    speed += .05f;
                
            }
            else if(kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down))
            {
                if(speed > -MAX_SPEED)
                    speed -= .05f;
                
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

            if (kbState.IsKeyDown(Keys.Space))
            {
                Vector2 direction = position;
                direction.Normalize();
                bm.AddBullet(position, direction, rotation);
            }

        }

        /// <summary>
        /// Draws the spaceship
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch object to allow for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 1f);
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
    }
}
