using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Asteroids
{
    /// <summary>
    /// Holds bullet objects and handles shooting timing
    /// </summary>
    class BulletManager
    {
        // FIELDS
        List<Bullet> bulletList;
        Texture2D bulletText;
        float timer; // used to measure time between shots

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="bulletText">Bullet's texture</param>
        public BulletManager(Texture2D bulletText)
        {
            this.bulletText = bulletText;
            bulletList = new List<Bullet>();
        }

        // METHODS

        /// <summary>
        /// Update all the bullets
        /// </summary>
        public void Update()
        {
            for(int i = 0; i < bulletList.Count; i++)
            {
                // If a bullet is active move it, otherwise delete it
                if (bulletList[i].Active)
                {
                    bulletList[i].Update();
                }
                else
                {
                    bulletList.Remove(bulletList[i]);
                }
            }
        }

        /// <summary>
        /// Draw all the bullets
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Creates a new bullet
        /// </summary>
        /// <param name="position">The bullet's starting position</param>
        /// <param name="velocity">The bullet's directional velocity</param>
        /// <param name="rotation">The bullet's rotation</param>
        /// <param name="gameTime">A GameTime object</param>
        public void AddBullet(Vector2 position, Vector2 velocity, float rotation, GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer > 350)
            {
                bulletList.Add(new Bullet(bulletText, position, velocity, rotation));
                timer = 0;
            }
        }
    }
}
