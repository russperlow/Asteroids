using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
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
        SoundEffect kachow;
        SoundEffectInstance sei;
        float timer; // used to measure time between shots

        // PROPERTIES
        public List<Bullet> BulletList { get { return bulletList; } set { bulletList = value; } }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="bulletText">Bullet's texture</param>
        public BulletManager(Texture2D bulletText, SoundEffect kachow)
        {
            this.bulletText = bulletText;
            this.kachow = kachow;
            sei = kachow.CreateInstance();
            sei.Volume = .5f;
            sei.Pan = 0f;
            sei.Pitch = 0f;
            bulletList = new List<Bullet>();
        }

        // METHODS

        /// <summary>
        /// Update all the bullets
        /// </summary>
        public void Update(GameTime gameTime)
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
            timer += gameTime.ElapsedGameTime.Milliseconds;

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
        /// <param name="color">The bullet's color</param>
        public void AddBullet(Vector2 position, Vector2 velocity, float rotation, Color color)
        {
            if (timer > 350)
            {
                bulletList.Add(new Bullet(bulletText, position, velocity, rotation, color));
                timer = 0;
                // Play a sound effect when shooting if not already playing
                if (sei.State != SoundState.Playing)
                    sei.Play();
            }
        }

    }
}
