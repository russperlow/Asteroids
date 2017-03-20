using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids
{
    /// <summary>
    /// A class used to create bullet objects
    /// </summary>
    class Bullet
    {
        // FIELDS
        Vector2 position, velocity;
        Texture2D texture;
        float speed, rotation;
        bool active;

        // PROPERTIES
        public bool Active { get { return active; } set { active = value; } }
        public Vector2 Position { get { return position; } }
        public Rectangle Rect { get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">The bullet texture</param>
        /// <param name="position">Its starting position</param>
        /// <param name="velocity">The direction it's moving in</param>
        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity, float rotation)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.rotation = rotation;
            speed = 1f;
            active = true;
        }

        // METHODS

        /// <summary>
        /// Update the bullets movement
        /// </summary>
        public void Update()
        {
            position += velocity * speed;
        }

        /// <summary>
        /// Draw the bullet
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

    }
}
