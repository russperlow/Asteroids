using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Asteroids
{
    /// <summary>
    /// Holds asteroids, handles ALL collisions and splitting
    /// </summary>
    class AsteroidManager
    {
        // FIELDS
        List<Texture2D> textures;
        List<Asteroid> asteroids;
        float timer;
        Random rand;
        Viewport viewport;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Content">ContentManager object</param>
        /// <param name="viewport">Viewport object</param>
        public AsteroidManager(ContentManager Content, Viewport viewport)
        {
            this.viewport = viewport;
            textures = new List<Texture2D>() { Content.Load<Texture2D>(@"Route66"), Content.Load<Texture2D>(@"Rusteze"), Content.Load<Texture2D>(@"PistonCup") };
            asteroids = new List<Asteroid>();
            rand = new Random();
            int temp = rand.Next(10, 15);
            for(int i = 0; i < temp; i++)
            {
                CreateAsteroid();
            }
        }

        // METHODS

        /// <summary>
        /// Updates all the existing asteroids
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        public void Update(GameTime gameTime)
        {
            foreach(Asteroid a in asteroids)
            {
                a.Update();
            }
        }

        /// <summary>
        /// Draws all existing asteroids
        /// </summary>
        /// <param name="spriteBatch">Spritebatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Asteroid a in asteroids)
            {
                a.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Creates a new asteroid
        /// </summary>
        public void CreateAsteroid()
        {
            // Randomly spawn a new asteroid outside of the screen bounds
            switch (rand.Next(4))
            {
                case 0:
                    asteroids.Add(new Asteroid(textures[rand.Next(3)], new Vector2(0, rand.Next(viewport.Height)), new Vector2((float)rand.NextDouble(), (float)rand.NextDouble()), false));
                    break;
                case 1:
                    asteroids.Add(new Asteroid(textures[rand.Next(3)], new Vector2(viewport.Width, rand.Next(viewport.Height)), new Vector2(-(float)rand.NextDouble(), (float)rand.NextDouble()), false));
                    break;
                case 2:
                    asteroids.Add(new Asteroid(textures[rand.Next(3)], new Vector2(rand.Next(viewport.Width), 0), new Vector2((float)rand.NextDouble(), -(float)rand.NextDouble()), false));
                    break;
                case 3:
                    asteroids.Add(new Asteroid(textures[rand.Next(3)], new Vector2(rand.Next(viewport.Width), viewport.Height), new Vector2(-(float)rand.NextDouble(), -(float)rand.NextDouble()), false));
                    break;
            }
        }
    }
}
