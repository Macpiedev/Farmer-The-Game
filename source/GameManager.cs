using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPO
{
    /// <summary>
    /// This class manage the game
    /// </summary>
    public class GameManager : Game
    {
        private DisplayManager display;
        private Player player;
        private Trader trader;
        private List<FarmField> fields;
        private Inventory inventory;
        private GrassField grass;

        /// <summary>
        /// Initialization of game's window
        /// </summary>
        public GameManager()
        {
            display = new DisplayManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        /// <summary>
        /// Initialization of game resources
        /// </summary>
        protected override void Initialize()
        {
            player = new Player(0, 0);
            fields = new List<FarmField>();
            grass = new GrassField(0,0);

            player.Gold = 100;
            player.MoveSpeed = 6f;
            base.Initialize();
        }
        
        /// <summary>
        /// Load and initialize graphicial resources in the game
        /// </summary>
        protected override void LoadContent()
        {
            display.spriteBatch = new SpriteBatch(GraphicsDevice);
            display.loadTextures(this);
            grass.texture = display.BackgroundText;
            trader = new Trader(1700, 800, display, 5, new Vector2(200, 50));
            player.Texture2D = display.PlayerText(0);
            trader.ShopActivator.Click += delegate (object sender, EventArgs e) { trader.shopState = !trader.shopState; };
            trader.Texture2D = display.TraderText;
            inventory = new Inventory(display.InventoryBlock, new Vector2(300, 800));
            
            player.vegetables.Add(new PlayerItem(50, "Carrot", display.item("carrot")));
            player.vegetables.Add(new PlayerItem(80, "Turnip", display.item("turnip")));
            player.vegetables.Add(new PlayerItem(650, "Pumpkin", display.item("pumpkin")));
            player.groceries.Add(new PlayerItem(500, "Milk", display.item("milk")));
            player.groceries.Add(new PlayerItem(300, "Wool", display.item("wool")));

            player.seeds.Add(new SeedItem(20, "CarrotSeeds", display.item("carrotSeeds"), display.InventoryButton, 5000, new PlayerItem(50, "Carrot", display.item("carrot"))));
            player.seeds.Add(new SeedItem(20, "TurnipSeeds", display.item("turnipSeeds"), display.InventoryButton, 5000, new PlayerItem(50, "Turnip", display.item("turnip"))));
            player.seeds.Add(new SeedItem(20, "PumpkinSeeds", display.item("pumpkinSeeds"), display.InventoryButton, 5000, new PlayerItem(50, "Pumpkin", display.item("pumpkin"))));

            player.animals.Add(new AnimalItem(2000, "Cow", display.item("cow"), display.InventoryButton, 7000, new PlayerItem(500, "Milk", display.item("milk")),"Pumpkin"));
            player.animals.Add(new AnimalItem(1000, "Sheep", display.item("sheep"), display.InventoryButton, 7000, new PlayerItem(300, "Wool", display.item("wool")), "Carrot"));

            fields.Add(new FarmField(400, 150));
            int fieldsRow = 5;
            int fieldsColumn = 3;
            for (int i = 0; i < fieldsRow; i++)
            {
                for (int j = 0; j < fieldsColumn; j++)
                    fields.Add(new FarmField((int)fields[0].Position.X + (display.Field.Width - 10) * i, (int)fields[0].Position.Y + (display.Field.Height - 10) * j));
            }

            fields.ForEach(x => x.texture = display.Field);

            player.initPlayerInventory(display, inventory, grass);

            trader.articles[0] = new ShopItem(20, "CarrotSeeds", display.item("carrotSeeds"), new Vector2(trader.shopPosition.X + 100, trader.shopPosition.Y + 100));
            trader.articles[1] = new ShopItem(40, "TurnipSeeds", display.item("turnipSeeds"), new Vector2(trader.articles[0].ItemButton.Position.X, trader.articles[0].ItemButton.Position.Y + 150));
            trader.articles[2] = new ShopItem(200, "PumpkinSeeds", display.item("pumpkinSeeds"), new Vector2(trader.articles[1].ItemButton.Position.X, trader.articles[1].ItemButton.Position.Y + 150));
            trader.articles[3] = new ShopItem(1000, "Sheep", display.item("sheep"), new Vector2(trader.articles[2].ItemButton.Position.X, trader.articles[2].ItemButton.Position.Y + 200));
            trader.articles[4] = new ShopItem(2000, "Cow", display.item("cow"), new Vector2(trader.articles[3].ItemButton.Position.X, trader.articles[3].ItemButton.Position.Y + 200));


            trader.wantedItems[0] = new ShopItem(50, "Carrot", display.item("carrot"), new Vector2(trader.shopPosition.X + 800, trader.shopPosition.Y + 100));
            trader.wantedItems[1] = new ShopItem(80, "Turnip", display.item("turnip"), new Vector2(trader.wantedItems[0].ItemButton.Position.X, trader.wantedItems[0].ItemButton.Position.Y + 150));
            trader.wantedItems[2] = new ShopItem(650, "Pumpkin", display.item("pumpkin"), new Vector2(trader.wantedItems[1].ItemButton.Position.X, trader.wantedItems[1].ItemButton.Position.Y + 150));
            trader.wantedItems[3] = new ShopItem(300, "Wool", display.item("wool"), new Vector2(trader.wantedItems[2].ItemButton.Position.X, trader.wantedItems[2].ItemButton.Position.Y + 200));
            trader.wantedItems[4] = new ShopItem(500, "Milk", display.item("milk"), new Vector2(trader.wantedItems[3].ItemButton.Position.X, trader.wantedItems[3].ItemButton.Position.Y + 200));

            trader.initShop(display, player);

        }

        /// <summary>
        /// This is the main game loop
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                Exit();
            }
            trader.ShopActivator.Update(gameTime);
            trader.UpdateShop(player, gameTime, display);
            fields.ForEach(x => x.Update(player));
            player.UpdateIntercats();
            inventory.Update(player, gameTime);
            grass.Update(player, gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Function responsible for drawing graphics
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);
             display.spriteBatch.Begin();

            grass.DrawGrass(display); 
            
            trader.ShopActivator.Draw(gameTime, display.spriteBatch);

            fields.ForEach(x => x.Draw(display.spriteBatch));

            grass.DrawAnimals(display, gameTime, player);

            display.spriteBatch.Draw(player.Texture2D, position: player.Position, Color.White);
            display.spriteBatch.DrawString(display.font(0), "Zloto gracza: " + player.Gold.ToString(), Vector2.Zero, Color.Yellow);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                trader.shopState = false;
            }

            inventory.Draw(gameTime, display, display.InventoryBlock);
            trader.DrawShop(display, gameTime);

            display.spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
