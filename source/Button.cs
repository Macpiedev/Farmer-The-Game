using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPO
{
    /// <summary>
    /// Class represents buttons.
    /// </summary>
    public class Button
    {
        private MouseState currentMouse;
        private MouseState previousMouse;
        private bool isHovering;
        public Texture2D texture;

        public event EventHandler Click;
        /// <summary>
        ///  Get and set information if button is licked.
        /// </summary>
        public bool Clicked { get; private set; }
        /// <summary>
        /// Get and set position of the button.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Button's rectangle
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }
        /// <summary>
        /// Set texture of the button.
        /// </summary>
        /// <param name="texture"></param>
        public Button(Texture2D texture)
        {
            this.texture = texture;

        }
        /// <summary>
        /// Draws button.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime game, SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if(isHovering)
            {
                color = Color.Gray;
            }
            spriteBatch.Draw(texture, Rectangle, color);
        }
        /// <summary>
        /// Check if button is clicked and then invoke function assigned to the button.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            isHovering = false;

            if(mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;
                if(currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());

                }
            }
        }
    }
}
