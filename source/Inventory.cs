using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPO
{
    /// <summary>
    /// This class is responsible for player's inventory
    /// </summary>
    public class Inventory
    {
        private List<InventoryBlock> inventory = new List<InventoryBlock>();
        private Vector2 lastPos1;
        private Vector2 lastPos2;

        private Texture2D texture;

        public Inventory(Texture2D texture, Vector2 inventoryPosition)
        {
            this.texture = texture;
            this.lastPos1 = inventoryPosition;
            this.lastPos2 = new Vector2(inventoryPosition.X, inventoryPosition.Y + texture.Height);
        }

        /// <summary>
        /// Function adds one inventory block to inventory. Expands inventory size.
        /// </summary>
        /// <param name="id"> number of row </param>
        /// <param name="invB"> one inventory block </param>
        public void Add(int id, InventoryBlock invB)
        {
            if(id == 1)
            {
                    invB.BlockPos = new Vector2(lastPos1.X + texture.Width, lastPos1.Y);
                    lastPos1 = invB.BlockPos;
            }

            if (id == 2)
            {
                invB.BlockPos = new Vector2(lastPos2.X + texture.Width, lastPos2.Y);
                lastPos2 = invB.BlockPos;
            }
            if (invB.button != null)
                invB.button.Position = new Vector2(invB.BlockPos.X, invB.BlockPos.Y - invB.button.texture.Height);
            inventory.Add(invB);
        }

        /// <summary>
        /// Function draws the inventory block.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="display"></param>
        /// <param name="texture"> inventory block texture </param>
        public void Draw(GameTime gameTime, DisplayManager display, Texture2D texture)
        {
            foreach(InventoryBlock block in inventory)
            {
                block.Draw(gameTime, display, texture);
                block.DrawButton(gameTime, display);
            }
        }

        /// <summary>
        /// Function updates the state of inventory
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public void Update(Player player, GameTime gameTime)
        {
            foreach (InventoryBlock block in inventory)
            {
                if(block.button != null)
                {
                    block.button.Update(gameTime);
                }
            }
        }

    }

    /// <summary>
    /// Class to create one inventory block
    /// </summary>
    public class InventoryBlock
    {

        private Button butt;
        private PlayerItem item;
        private Vector2 blockPos;
        private string text;

        /// <summary>
        /// Get inventory block button.
        /// </summary>
        public Button button
        {
            get { return butt; }
           
        }
        /// <summary>
        /// Get inventory block position.
        /// </summary>
        public Vector2 BlockPos
        {
            get { return blockPos; }
            set { blockPos = value; }
        }

        /// <summary>
        /// Init inventory block
        /// </summary>
        /// <param name="button"></param>
        /// <param name="item"></param>
        /// <param name="text"></param>
        public InventoryBlock(Button button, PlayerItem item, string text)
        {
            this.butt = button;
            this.item = item;
            this.text = text;
        }
        /// <summary>
        /// Function assigns player's item to the inventory block
        /// </summary>
        /// <param name="item"> player's item </param>
        public InventoryBlock(PlayerItem item)
        {
            this.item = item;
 
        }

        /// <summary>
        /// Function draws inventory block
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="display"></param>
        /// <param name="texture"> inventory block texture </param>
        public void Draw(GameTime gameTime,DisplayManager display, Texture2D texture)
        {


            display.spriteBatch.Draw(texture,blockPos, Color.White);
            float scale = 1f;
            while ((item.itemTexture.Height * scale > texture.Height) || (item.itemTexture.Width * scale > texture.Width))
                scale -= 0.1f;
            scale -= 0.1f;
            if (item.GetType() == typeof(AnimalItem))
                display.spriteBatch.Draw(item.itemTexture, new Vector2(blockPos.X + 10, blockPos.Y + 20), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            else
                display.spriteBatch.Draw(item.itemTexture, new Vector2(blockPos.X + 5, blockPos.Y + 5), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            display.spriteBatch.DrawString(display.font(0), item.quantity.ToString(), new Vector2(blockPos.X + 40, blockPos.Y + 50), Color.Black, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 0);
            display.spriteBatch.DrawString(display.font(0), item.quantity.ToString(), new Vector2(blockPos.X + 46, blockPos.Y + 58), Color.Yellow);
        }

        /// <summary>
        /// Function draws inventory block button above inventory block.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="display"></param>
        public void DrawButton(GameTime gameTime, DisplayManager display)
        {
            if (item.quantity > 0 && button != null)
            {
                button.Draw(gameTime, display.spriteBatch);
                display.spriteBatch.DrawString(display.font(1), text, new Vector2(button.Position.X + 15, button.Position.Y + 10), Color.White);
            }
        }
    }
}
