using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPO
{
    /// <summary>
    /// Represents diffrent items.
    /// </summary>
    public class Item
    {
        protected string name;
        protected int cost;

        /// <summary>
        /// Init item.
        /// </summary>
        /// <param name="cost"> Cost of the item </param>
        /// <param name="name"> Name of the item </param>
        public Item(int cost, string name)
        {
            this.cost = cost;
            this.name = name;
  
        }
        /// <summary>
        /// Get name of the item.
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Get the cost of the item.
        /// </summary>
        public int Cost
        {
            get { return cost; }
        }
    }

    /// <summary>
    /// Item in the shop.
    /// </summary>
    public class ShopItem:Item
    {
        private SpriteFont font;
        private Vector2 fontPosition;
        private Button button;
        /// <summary>
        /// Init shop item.
        /// </summary>
        /// <param name="cost"> Cost of the item </param>
        /// <param name="name"> Name of the item </param>
        /// <param name="text"> Texture of the item </param>
        /// <param name="buttPos"> Position of the item button in the shop </param>
        public ShopItem(int cost, string name, Texture2D text, Vector2 buttPos) :base(cost,name)
        {
            this.name = name;
            button = new Button(text);
            button.Position = buttPos;
            fontPosition = new Vector2(buttPos.X + 200 , buttPos.Y + 50);
        }
        /// <summary>
        /// Draw  informations for the shop.
        /// </summary>
        /// <param name="sb"></param>
        public void drawFont(SpriteBatch sb)
        {
            sb.DrawString(font, "Cena: " + this.cost.ToString() + " zlota", fontPosition, Color.Yellow);
        }
        /// <summary>
        /// Get button of the item.
        /// </summary>
        public Button ItemButton
        {
            get { return button; }
        }
        /// <summary>
        /// Set font of the information of the item in shop.
        /// </summary>
        public SpriteFont CostFont
        {
            set { font = value; }
        }

    }
    /// <summary>
    /// Represents items which belong to player.
    /// </summary>
    public class PlayerItem : Item
    {
        public int quantity;
        public Texture2D itemTexture;
        public Vector2 itemPos;
        public Button button;
        /// <summary>
        /// Init player's item.
        /// </summary>
        /// <param name="cost"> Cost of the item </param>
        /// <param name="name"> Name of the item </param>
        /// <param name="item"> Texture of the item </param>
        public PlayerItem(int cost, string name, Texture2D item) : base(cost, name)
        {
            quantity = 0;
            itemTexture = item;
        }

    }

    /// <summary>
    /// Represents seed in  player's items.
    /// </summary>
public class SeedItem : PlayerItem
    {
        public int timeToGrowth;
        public PlayerItem GrownUp;
        /// <summary>
        /// Get rectangle of the seed item.
        /// </summary>
        public Rectangle GrownUpRectangle
        {
            get
            {
                return new Rectangle((int)GrownUp.itemPos.X, (int)GrownUp.itemPos.Y, GrownUp.itemTexture.Width, GrownUp.itemTexture.Height);
            }
        }
        /// <summary>
        /// Init seed belonging to player,
        /// </summary>
        /// <param name="cost"> Cost of the item. </param>
        /// <param name="name"> Name of the item. </param>
        /// <param name="item"> Texture of the item. </param>
        /// <param name="button"> Texture of the button to use item. </param>
        /// <param name="time"> Time to growing out </param>
        /// <param name="pitem"> Plant after growing out </param>
        public SeedItem(int cost, string name, Texture2D item, Texture2D button, int time, PlayerItem pitem) : base(cost, name,item)
        {
            this.button = new Button(button);
            timeToGrowth = time;
            GrownUp = pitem;

        }
    }

    /// <summary>
    /// Represent animal in the player's items.
    /// </summary>
    public class AnimalItem : PlayerItem
    {
        private string foodName;
        public Animal animal;
        /// <summary>
        /// Init animal belonging to player,
        /// </summary>
        /// <param name="cost"> Cost of the item. </param>
        /// <param name="name"> Name of the item. </param>
        /// <param name="item"> Texture of the item. </param>
        /// <param name="button"> Texture of the button to use item. </param>
        /// <param name="time"> Time to produce item. </param>
        /// <param name="pitem"> Item that is produced by the animal. </param>
        /// <param name="foodName"> Name of the food that is eaten by the animal </param>
        public AnimalItem(int cost, string name, Texture2D item, Texture2D button, int time, PlayerItem pitem, string foodName) : base(cost, name, item)
        {
            this.name = name;
            this.button = new Button(button);
            quantity = 0;
            itemTexture = item;
            this.foodName = foodName;
            animal = new Animal(name,item, pitem,time,foodName);
        }


    }

}


