using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Threading;

namespace ProjektPO
{
    /// <summary>
    /// Represents the field.
    /// </summary>
    public class Field
    {
        public Vector2 Position;
        public Texture2D texture;
        public Stopwatch clock = new Stopwatch();
        public bool isTouched;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width - 100, texture.Height - 100);
            }
        }
    }


    /// <summary>
    /// Represents the field used for planting.
    /// </summary>
    public class FarmField:Field
    {
        private bool empty;
        private bool grown;
        private SeedItem planted;

        /// <summary>
        /// Initialize of farm field.
        /// </summary>
        /// <param name="x"> Position axis X </param>
        /// <param name="y">Position axis Y </param>
        public FarmField(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
            empty = true;
            grown = false;
        }

        /// <summary>
        /// Check if field is already planted.
        /// </summary>
        public bool isEmpty
        {
           get { return empty; }
           set { empty = value; }
        }
        /// <summary>
        /// Check if plant has grown.
        /// </summary>
        public bool isGrown
        {
            get { return grown;  }
        }
        /// <summary>
        /// Set what is planted at farm field.
        /// </summary>
        public SeedItem PlantedSeed
        {
            set { planted = value; }
        }
        /// <summary>
        /// Check if player interact with field and update plant if field is planted.
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player)
        {
            if (this.Rectangle.Intersects(player.Rectangle))
            {
                isTouched = true;
                player.fieldInInteract = this;
            }
            else
            { 
                isTouched = false;
            }

            if (planted != null)
            {
                planted.GrownUp.itemPos = this.Position;

                if (clock.ElapsedMilliseconds >= this.planted.timeToGrowth)
                {

                    grown = true;
                    clock.Stop();

                    if (planted.GrownUpRectangle.Intersects(player.Rectangle))
                    {
                        foreach (PlayerItem item in player.vegetables)
                        {
                            if (item.Name == planted.GrownUp.Name)
                            {
                                item.quantity += 1;
                                break;
                            }

                        }
                        clock.Reset();
                        empty = true;
                        grown = false;
                        planted = null;
                    }


                }
            }


        }
        /// <summary>
        /// Draws field.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture, position: this.Position, Color.White);
            if (planted != null && grown == false)
            {
                sb.Draw(planted.itemTexture, new Vector2(this.Position.X + 25, this.Position.Y + 15), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            }

            if (grown == true && planted != null)
            {
                sb.Draw(planted.GrownUp.itemTexture, new Vector2(this.Position.X + 25, this.Position.Y + 15), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            }
        }
    }


    /// <summary>
    /// Represents the grass field where animals can stand.
    /// </summary>
    public class GrassField:Field
    {
        public List<Animal> animals = new List<Animal>();
        public List<Animal> deathAnimals = new List<Animal>();
        /// <summary>
        /// Init grass field.
        /// </summary>
        /// <param name="x"> Postion axis X </param>
        /// <param name="y"> Postion axis Y </param>
        public GrassField(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
        }
        /// <summary>
        /// Check is player is touching grass and update all animals.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
       public void Update(Player player, GameTime gameTime)
        {
            if(player.Rectangle.Intersects(this.Rectangle))
            {
                isTouched = true;
            }
            else
            {
                isTouched = false;
            }    

            foreach(Animal animal in animals)
            {
                animal.Update(player, gameTime);
                if(animal.health < 0)
                {
                    deathAnimals.Add(animal);
                }
            }
            foreach(Animal animal in deathAnimals)
            {
                animals.Remove(animal);
            }
            deathAnimals.Clear();
        }

        /// <summary>
        /// Draws grass.
        /// </summary>
        /// <param name="display"></param>
        public void DrawGrass(DisplayManager display)
        {
            display.spriteBatch.Draw(this.texture, Vector2.Zero, Color.White);
        }
        /// <summary>
        /// Draws animals standing on the grass.
        /// </summary>
        /// <param name="display"></param>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public void DrawAnimals(DisplayManager display, GameTime gameTime, Player player)
        {
            foreach (Animal animal in animals)
            {
                animal.Draw(display, gameTime, player);
            }
        }

    }




}
