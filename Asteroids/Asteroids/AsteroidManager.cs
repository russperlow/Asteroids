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
        const int OFF_SCREEN_DIST = 1000;
        List<Texture2D> textures;
        List<Asteroid> asteroids;
        float timer;
        Random rand;
        Viewport viewport;
        BulletManager bm;
        Ship ship;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Content">ContentManager object</param>
        /// <param name="viewport">Viewport object</param>
        public AsteroidManager(ContentManager Content, Viewport viewport, BulletManager bm, Ship ship)
        {
            this.viewport = viewport;
            this.bm = bm;
            this.ship = ship;
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
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if(timer > 1000)
            {
                CreateAsteroid();
                timer = 0;
            }

            foreach(Asteroid a in asteroids)
            {
                a.Update();
            }
            CollisionCheck();
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


        /// <summary>
        /// Check asteroid collision with bullets and the player
        /// As well as if it is off the screen
        /// </summary>
        public void CollisionCheck()
        {
            for(int i = 0; i < asteroids.Count; i++)
            {

                // Checks to see if asteroid is far off screen
                if (asteroids[i].Position.X < -OFF_SCREEN_DIST || asteroids[i].Position.Y < -OFF_SCREEN_DIST || asteroids[i].Position.X > viewport.Width + OFF_SCREEN_DIST || asteroids[i].Position.Y > viewport.Height + OFF_SCREEN_DIST)
                {
                    asteroids.Remove(asteroids[i]);
                    break;
                }

                // Handle player collision
                if (asteroids[i].Rect.Intersects(ship.Rect) && !ship.Invincible)
                {
                    Reset();
                    break;
                }

                // Handle bullet collision
                foreach (Bullet b in bm.BulletList)
                {
                    // Checks to see if bullet is even on screen
                    if (b.Position.X < -OFF_SCREEN_DIST || b.Position.Y < -OFF_SCREEN_DIST || b.Position.X > viewport.Width + OFF_SCREEN_DIST || b.Position.Y > viewport.Height + OFF_SCREEN_DIST)
                    {
                        b.Active = false;
                        break;
                    }

                    if (asteroids[i].Rect.Intersects(b.Rect) && b.Active)
                    {
                        b.Active = false;
                        // If it is a large asteroid split it
                        if (!asteroids[i].Split)
                        {
                            asteroids.Add(new Asteroid(asteroids[i].Texture, asteroids[i].Position, new Vector2(asteroids[i].Velocity.X + (float)rand.NextDouble(), asteroids[i].Velocity.Y + (float)rand.NextDouble()), true));
                            asteroids.Add(new Asteroid(asteroids[i].Texture, asteroids[i].Position, new Vector2(asteroids[i].Velocity.X - (float)rand.NextDouble(), asteroids[i].Velocity.Y - (float)rand.NextDouble()), true));
                            ship.Score += 20;
                        }
                        else
                        {
                            ship.Score += 50;
                        }
                        asteroids.Remove(asteroids[i]);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// If a player collides with an asteroid reset the game
        /// </summary>
        public void Reset()
        {
            // Clear everything
            asteroids.Clear();
            bm.BulletList.Clear();

            // Move the player back to the center and remove a life
            ship.Position = new Vector2(viewport.Width / 2, viewport.Height / 2);
            ship.Lives--;

            // Create new asteroids
            int temp = rand.Next(10, 15);
            for (int i = 0; i < temp; i++)
            {
                CreateAsteroid();
            }
        }
    }
}
