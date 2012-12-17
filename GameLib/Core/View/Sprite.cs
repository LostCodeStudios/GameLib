using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameLib
{
    /// <summary>
    /// Drawable sprite class
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// Constructs a sprite with a texture and a color.
        /// </summary>
        /// <param name="spriteSheet">The texture on which the sprite resides.</param>
        /// <param name="color">The color of the sprite.</param>
        public Sprite(Texture2D spriteSheet, Color color)
        {
            _SpriteSheet = spriteSheet;
            this.Color = color;
        }

        #region Functioning Loop
        /// <summary>
        /// Draws the sprite to a spriteBatch
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="source"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float rotation, Rectangle source)
        {
            spriteBatch.Draw(SpriteSheet, position, source, Color, rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
        #endregion

        #region Fields
        Texture2D _SpriteSheet;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the sprites SpriteSheet
        /// </summary>
        public Texture2D SpriteSheet
        {
            get
            {
                return _SpriteSheet;
            }
        }

        /// <summary>
        /// Sets or gets the color of this sprite.
        /// </summary>
        public Color Color
        {
            set;
            get;
        }
        #endregion

        #region Methods

        #endregion
    }
}
