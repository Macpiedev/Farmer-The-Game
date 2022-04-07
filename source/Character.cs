using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPO
{
    /// <summary>
    /// Represents characters in the game.
    /// </summary>
    public class Character
    {
        protected string name;
        protected Texture2D text;
        protected Vector2 position;
        /// <summary>
        /// Get character rectangle.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, text.Width - 40 , text.Height);
            }
        }
        /// <summary>
        /// Init character.
        /// </summary>
        /// <param name="x"> X axis</param>
        /// <param name="y"> Y axis </param>
        public Character(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }
        /// <summary>
        /// Get and set texture of the character.
        /// </summary>
        public Texture2D Texture2D
        {
            get { return text; }
            set { text = value; }
        }
        /// <summary>
        /// Get and set name of the character.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Get position of the character.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }



    }

    public class Player : Character
    {
        private float moveSpeed;
        private int gold;
 
        public List<SeedItem> seeds = new List<SeedItem>();
        public List<AnimalItem> animals = new List<AnimalItem>();
        public List<PlayerItem> vegetables = new List<PlayerItem>();
        public List<PlayerItem> groceries = new List<PlayerItem>();
        public FarmField fieldInInteract;
        /// <summary>
        /// Init player.
        /// </summary>
        /// <param name="x"> X axis </param>
        /// <param name="y"> Y axis </param>
        public Player(int x, int y):base(x,y)
        {
            
        }
        /// <summary>
        /// Move player.
        /// </summary>
        /// <param name="x"> X axis </param>
        /// <param name="y"> Y axis </param>
        public void move(int x, int y)
        {
                position.X += x * moveSpeed;   
                position.Y += y* moveSpeed;
        }

        /// <summary>
        /// Function control the player movement
        /// </summary>
        /// <param name="display"> Display manager </param>,
        /// <param name="trader"> Trader's rectangle </param>
        public void PlayerControl(DisplayManager display, Rectangle trader)
        {

                if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    this.move(0, -1);
                    this.Texture2D = display.PlayerText(3);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    this.move(0, 1);
                    this.Texture2D = display.PlayerText(0);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    this.move(1, 0);
                    this.Texture2D = display.PlayerText(1);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    this.move(-1, 0);
                    this.Texture2D = display.PlayerText(2);
                }
        }
        /// <summary>
        /// Get and set player's gold.
        /// </summary>
        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }
        /// <summary>
        /// Get and set player's move speed.
        /// </summary>
        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        /// <summary>
        /// Update interacts between player and fields.
        /// </summary>
        public void UpdateIntercats()
        {
            if (this.fieldInInteract != null)
                if (this.fieldInInteract.isTouched == false)
                    this.fieldInInteract = null;
        }

        /// <summary>
        /// Create possible items to get by the player.
        /// </summary>
        /// <param name="display"></param>
        /// <param name="inventory"></param>
        /// <param name="grass"></param>
        public void initPlayerInventory(DisplayManager display, Inventory inventory, GrassField grass)
        {

            foreach (SeedItem seed in this.seeds)
            {
                Button plantButton = new Button(display.InventoryButton);
                plantButton.Click += delegate (object sender, EventArgs e)
                {
                    if (seed.quantity != 0 && this.fieldInInteract != null)
                    {
                        if (this.fieldInInteract.isEmpty == true)
                        {
                            seed.quantity -= 1;
                            this.fieldInInteract.isEmpty = false;
                            this.fieldInInteract.PlantedSeed = seed;
                            this.fieldInInteract.clock.Start();
                        }

                    }
                };
                inventory.Add(1, new InventoryBlock(plantButton, seed, "Zasiej"));
            }

            foreach (PlayerItem item in this.vegetables)
            {
                inventory.Add(2, new InventoryBlock(item));
            }

            foreach (PlayerItem item in this.groceries)
            {
                inventory.Add(2, new InventoryBlock(item));
            }

            foreach (AnimalItem item in this.animals)
            {

                Button putAnimalButton = new Button(display.InventoryButton);

                putAnimalButton.Click += delegate (object sender, EventArgs e)
                {
                    if (item.quantity > 0)
                    {
                        bool noSpace = false;
                        if (grass.isTouched == true && this.fieldInInteract == null)
                        {
                            foreach (Animal animal in grass.animals)
                            {
                                if (animal.Rectangle.Intersects(this.Rectangle))
                                {
                                    noSpace = true;
                                }
                            }
                            if (noSpace == false)
                            {

                                Animal newAnimal = new Animal(item, this, new Vector2(this.Position.X, this.Position.Y), display.InventoryButton);
                                newAnimal.Name = item.animal.Name;
                                grass.animals.Add(newAnimal);
                                item.quantity -= 1;
                            }
                        }
                    }
                };

                inventory.Add(1, new InventoryBlock(putAnimalButton, item, "Postaw"));
            }
        }

    }

    /// <summary>
    /// Trader from the game. Works as the shop.
    /// </summary>
    public class Trader : Character
    {
        public Button ShopActivator;
        public bool shopState;
        public Vector2 shopPosition;
        public List<ShopItem> articles = new List<ShopItem>();
        public List<ShopItem> wantedItems = new List<ShopItem>();
        /// <summary>
        /// Init trader
        /// </summary>
        /// <param name="x"> X axis </param>
        /// <param name="y"> Y axis </param>
        /// <param name="dm"> Display manager</param>
        /// <param name="invSize"> Size of the shop </param>
        /// <param name="shopPos"> Vector2 of the shop </param>
        public Trader(int x, int y, DisplayManager dm, int invSize, Vector2 shopPos) : base(x, y)
        {

            articles = new List<ShopItem>();
            wantedItems = new List<ShopItem>();
            this.shopPosition = shopPos;
            for (int i = 0; i < invSize; i++)
            {
                this.articles.Add(null);
                this.wantedItems.Add(null);
            }               
            ShopActivator = new Button(dm.TraderText);
            ShopActivator.Position = this.position;
        }

        /// <summary>
        /// Init items in shop
        /// </summary>
        /// <param name="display"></param>
        /// <param name="player"></param>
        public void initShop(DisplayManager display, Player player)
        {
                this.articles.ForEach(item => {
                    item.CostFont = display.font(0);
                    item.ItemButton.Click += delegate (object sender, EventArgs e) {
                        if (player.Gold >= item.Cost)
                        {
                            player.Gold -= item.Cost;
                            PlayerItem playerItem = null;
                            foreach (SeedItem seed in player.seeds)
                            {
                                if (seed.Name == item.Name)
                                {
                                    playerItem = seed;
                                    break;
                                }
                            }
                            foreach (AnimalItem animal in player.animals)
                            {
                                if (animal.Name == item.Name)
                                {
                                    playerItem = animal;
                                    break;
                                }
                            }


                            playerItem.quantity += 1;
                        }
                    };
                });            
            this.wantedItems.ForEach(item =>
            {
                item.CostFont = display.font(0);
                item.ItemButton.Click += delegate (object sender, EventArgs e)
                {
                    PlayerItem playerItem = null;
                    foreach (PlayerItem vege in player.vegetables)
                    {
                        if (item.Name == vege.Name)
                            playerItem = vege;
                    }

                    foreach (PlayerItem grocery in player.groceries)
                    {
                        if (item.Name == grocery.Name)
                            playerItem = grocery;
                    }

                    if (playerItem != null)
                        if (playerItem.quantity != 0)
                        {
                            playerItem.quantity -= 1;
                            player.Gold += item.Cost;
                        }
                };
            });
        }
        /// <summary>
        /// Check if shop is open or not.
        /// </summary>
        /// <param name="player"> Player </param>
        /// <param name="gameTime"> Game time</param>
        /// <param name="display"> Display Manager </param>
        public void UpdateShop(Player player, GameTime gameTime, DisplayManager display)
        {
            if (!this.shopState)
                player.PlayerControl(display, this.Rectangle);
            else
            {
                foreach (ShopItem item in this.articles)
                    if (item != null)
                        item.ItemButton.Update(gameTime);
                foreach (ShopItem item in this.wantedItems)
                    if (item != null)
                        item.ItemButton.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws shop.
        /// </summary>
        /// <param name="display"> Display Manager</param>
        /// <param name="gameTime"> Game Time  </param>
        public void DrawShop(DisplayManager display, GameTime gameTime)
        {
            if (this.shopState)
            {
                display.spriteBatch.Draw(display.Shop, this.shopPosition, Color.White);
                display.spriteBatch.DrawString(display.font(0), "Kup", new Vector2(this.articles[0].ItemButton.Position.X + 50, this.articles[0].ItemButton.Position.Y - 50), Color.Yellow);
                display.spriteBatch.DrawString(display.font(0), "Sprzedaj", new Vector2(this.articles[0].ItemButton.Position.X + 800, this.articles[0].ItemButton.Position.Y - 50), Color.Yellow);
                foreach (ShopItem item in this.articles)
                    if (item != null)
                    {
                        item.ItemButton.Draw(gameTime, display.spriteBatch);
                        item.drawFont(display.spriteBatch);
                    }

                foreach (ShopItem item in this.wantedItems)
                    if (item != null)
                    {
                        item.ItemButton.Draw(gameTime, display.spriteBatch);
                        item.drawFont(display.spriteBatch);
                    }
            }
        }
    }
}
