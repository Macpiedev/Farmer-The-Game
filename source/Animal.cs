
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
    /// Class to create animal put on the field.
    /// </summary>
    public class Animal
    {
        public string Name;
        public int produceTime;
        public int health;
        public Vector2 position;
        public Texture2D texture;
        public PlayerItem producedItem;
        private Stopwatch produceClock = new Stopwatch();
        private Stopwatch deathClock = new Stopwatch();
        private Button feedButton;
        private bool visible;
        private string foodName;

        /// <summary>
        /// Get the animal's rectangle.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X + 40, (int)position.Y, texture.Width, texture.Height);
            }
        }
        /// <summary>
        /// Get rectangle of item which is produced by the animal.
        /// </summary>
        public Rectangle ProducedItemRectangle
        {
            get
            {
                return new Rectangle((int)position.X + 50, (int)position.Y + 20, producedItem.itemTexture.Width - 20, producedItem.itemTexture.Height);
            }
        }

        /// <summary>
        /// Animal initialization class to create animal on field
        /// </summary>
        /// <param name="aitem"></param>
        /// <param name="player"></param>
        /// <param name="pos"></param>
        /// <param name="fbt"> Feed button texture </param>
        public Animal(AnimalItem aitem, Player player, Vector2 pos, Texture2D fbt)
        {
            this.Name = aitem.animal.Name;
            this.health = aitem.animal.health;
            visible = false;
            this.texture = aitem.animal.texture;
            this.producedItem = aitem.animal.producedItem;
            this.produceTime = aitem.animal.produceTime;
            this.foodName = aitem.animal.foodName;
            this.position = pos;
            feedButton = new Button(fbt);
            feedButton.Position = new Vector2(this.position.X, this.position.Y - feedButton.texture.Height);
            feedButton.Click += delegate (object sender, EventArgs e)
            {
                foreach (PlayerItem item in player.vegetables)
                {
                    if (item.Name == foodName && item.quantity > 0)
                    {
                       switch(item.Name)
                        {
                            case "Carrot":
                                health += 3;
                                break;
                            case "Pumpkin":
                                health += 10;
                                break;
                        }
                        item.quantity -= 1;
                    }
                }
            };
        }
        /// <summary>
        /// Animal initialization clas to create animal assigned to animal item.
        /// </summary>
        /// <param name="name"> The animal's name </param>
        /// <param name="texture"> The animal's texture </param>
        /// <param name="producedItem"> Product which is produced by the animal</param>
        /// <param name="produceTime"> Time to produce item by the animal </param>
        /// <param name="foodName"> Name of the food eaten by the animal </param>
        public Animal(string name,Texture2D texture, PlayerItem producedItem, int produceTime, string foodName)
        {
            visible = false;
            this.Name = name;
            switch (this.Name)
            {
                case "Cow":
                    this.health = 15;
                    break;
                case "Sheep":
                    this.health = 10;
                    break;
            }
            this.texture = texture;
            this.producedItem = producedItem;
            this.produceTime = produceTime;
            this.foodName = foodName;

        }

        /// <summary>
        /// Update animal to check if animal is dead or produced the item.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public void Update(Player player, GameTime gameTime)
        {
            deathClock.Start();
            if(deathClock.ElapsedMilliseconds >= 3000)
            {
                health -= 1;
                deathClock.Reset();
            }
            if(player.Rectangle.Intersects(this.Rectangle) )
                visible = true;
            else
                visible = false;

            if(visible)
                feedButton.Update(gameTime);


            produceClock.Start();
            if(produceClock.ElapsedMilliseconds >= produceTime)
            {
                if(player.Rectangle.Intersects(ProducedItemRectangle))
                {
                    foreach(PlayerItem item in player.groceries)
                    {
                        if(item.Name == producedItem.Name)
                        {
                            item.quantity += 1;
                            produceClock.Reset();
                        }
                    }
                } 
            }

        }
        /// <summary>
        /// Draws the animal.
        /// </summary>
        /// <param name="display"></param>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public void Draw(DisplayManager display, GameTime gameTime, Player player)
        {
            display.spriteBatch.Draw(texture, position: position, Color.White);
            if (produceClock.ElapsedMilliseconds >= produceTime)
            { 
                display.spriteBatch.Draw(producedItem.itemTexture,  new Vector2(position.X + 70, position.Y + 70),null,Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }
            if(visible)
            {
                feedButton.Draw(gameTime, display.spriteBatch);
                display.spriteBatch.DrawString(display.font(1), "Nakarm", new Vector2(feedButton.Position.X + 15, feedButton.Position.Y + 10), Color.White);
            }
            if(player.Rectangle.Intersects(this.Rectangle))
                display.spriteBatch.DrawString(display.font(0), "HP:" + health, new Vector2(position.X + 80, position.Y), Color.Red);
        }

    }
}
