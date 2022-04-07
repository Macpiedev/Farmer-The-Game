using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPO
{
    /// <summary>
    /// Class responsible for holding textures and sevicing graphics.
    /// </summary>
    public class DisplayManager
    {
        private int screenWidth = 1920;
        private int screenHeight = 1000;

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private List<Texture2D> player = new List<Texture2D>();
        private Texture2D background;
        private Texture2D trader;
        private Texture2D shop;
        private Dictionary<string,Texture2D> items = new Dictionary<string,Texture2D>();
        private List<SpriteFont> fonts = new List<SpriteFont>();
        private Texture2D inventoryBlock;
        private Texture2D inventoryButton;
        private Texture2D field;
        public DisplayManager(Game game)
        {
            graphics = new GraphicsDeviceManager(game);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ToggleFullScreen();
        }
        /// <summary>
        /// Loading textures
        /// </summary>
        /// <param name="game"></param>
        public void loadTextures(Game game)
        {
            player.Add(game.Content.Load<Texture2D>("Farmer"));
            player.Add(game.Content.Load<Texture2D>("FarmedZbok"));
            player.Add(game.Content.Load<Texture2D>("FarmedZbok2"));
            player.Add(game.Content.Load<Texture2D>("FarmerTyl"));
            background = game.Content.Load<Texture2D>("Tlo");
            trader = game.Content.Load<Texture2D>("Handlarz");
            shop = game.Content.Load<Texture2D>("sklep");
            fonts.Add(game.Content.Load<SpriteFont>("Font"));
            fonts.Add(game.Content.Load<SpriteFont>("Font2"));

            items.Add("carrot", game.Content.Load<Texture2D>("carrot"));
            items.Add("turnip", game.Content.Load<Texture2D>("turnip"));
            items.Add("pumpkin", game.Content.Load<Texture2D>("pumpkin"));

            inventoryBlock = game.Content.Load<Texture2D>("InventoryBlock");
            items.Add("turnipSeeds",game.Content.Load<Texture2D>("TurnipSeeds"));
            items.Add("pumpkinSeeds", game.Content.Load<Texture2D>("PumpkinSeeds"));
            items.Add("carrotSeeds", game.Content.Load<Texture2D>("CarrotSeeds"));
            field = game.Content.Load<Texture2D>("ziemia");
            inventoryButton = game.Content.Load<Texture2D>("PlantButton");
            items.Add("cow", game.Content.Load<Texture2D>("cow"));
            items.Add("milk", game.Content.Load<Texture2D>("milk"));
            items.Add("sheep", game.Content.Load<Texture2D>("Sheep"));
            items.Add("wool", game.Content.Load<Texture2D>("wool"));
        }
        /// <summary>
        /// Get graphics manager.
        /// </summary>
        public GraphicsDeviceManager GraphManager
        {
            get { return graphics; }
        }
        /// <summary>
        /// Get background texture.
        /// </summary>
        public Texture2D BackgroundText
        {
            get { return background; }
        }
        /// <summary>
        /// Get one of the player's textures
        /// </summary>
        /// <param name="txtId"> Id of player texture</param>
        /// <returns> one of the player's textures </returns>
        public Texture2D PlayerText(int txtId)
        {
            return player[txtId];
        }
        /// <summary>
        /// Get the trader's texture
        /// </summary>
        public Texture2D TraderText
        {
            get { return trader; }
        }
        /// <summary>
        /// Get the shop's texture
        /// </summary>
        public Texture2D Shop
        {
            get { return shop; }
        }

        /// <summary>
        /// Get the fonts used for project
        /// </summary>
        public SpriteFont font(int fontId)
        {
            return fonts[fontId];
        }
        /// <summary>
        /// Get the item texture.
        /// </summary>
        /// <param name="name"> Name of item </param>
        /// <returns> Texture of the item </returns>
        public Texture2D item(string name)
        {
            return items[name];
        }

        /// <summary>
        /// Get the inventory block's texture.
        /// </summary>
        public Texture2D InventoryBlock
        {
            get { return inventoryBlock; }
        }

        /// <summary>
        /// Get the texture
        /// </summary>
        public Texture2D Field
        {
            get { return field; }
        }
        /// <summary>
        /// Get the texture of inventory button.
        /// </summary>
        public Texture2D InventoryButton
        {
            get { return inventoryButton; }
        }
    }
}
