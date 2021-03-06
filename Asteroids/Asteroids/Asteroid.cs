﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;


namespace Asteroids
{
    /// <summary>
    /// Class for asteroid objects
    /// </summary>
    class Asteroid
    {
        // FIELDS
        Texture2D texture;
        bool split, active;
        Vector2 position, velocity;
        
        // PROPERTIES
        public Rectangle Rect { get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); } }
        public bool Active { get { return active; } set { active = false; } }
        public bool Split { get { return split; } }
        public Texture2D Texture { get { return texture; } }
        public Vector2 Position { get { return position; } }
        public Vector2 Velocity { get { return velocity; } }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="texture">The asteroids texture</param>
        /// <param name="position">The starting position</param>
        /// <param name="velocity">The velocity it will move at</param>
        /// <param name="split">Whether it is a big asteroid or a small one split from a bigger one</param>
        public Asteroid(Texture2D texture, Vector2 position, Vector2 velocity, bool split)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.split = split;
            active = true;
        }

        // METHODS

        /// <summary>
        /// Updates the asteroids movement
        /// </summary>
        public void Update()
        {
            position += velocity;
        }

        /// <summary>
        /// Draw the asteroid
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if(split)
                spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

    }
}
