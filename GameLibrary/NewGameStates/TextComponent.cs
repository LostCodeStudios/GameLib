using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.NewGameStates
{
    /// <summary>
    /// Contains all the information needed to draw a single string. All fields are public, and must be encapsulated by classes using TextComponents.
    /// </summary>
    public class TextComponent
    {
        #region Defaults

        /// <summary>
        /// The default font.
        /// </summary>
        public static SpriteFont DefaultFont;

        /// <summary>
        /// The default color.
        /// </summary>
        public static Color DefaultColor = Color.Black;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a TextComponent.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="spriteEffects"></param>
        /// <param name="layerDepth"></param>
        public TextComponent(string text, SpriteFont font, Vector2 position, float scale, Color color, float rotation, SpriteEffects spriteEffects, float layerDepth)
        {
            Text = text;
            Font = font;
            Position = position;
            Scale = scale;
            Color = color;
            Rotation = rotation;
            SpriteEffects = spriteEffects;
            LayerDepth = layerDepth;
        }

        /// <summary>
        /// Constructs a TextComponent by specifying only the text and the position, using defaults for all other fields.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        public TextComponent(string text, Vector2 position)
        {
            Text = text;
            Font = DefaultFont;
            Position = position;
            Color = DefaultColor;
        }

        #endregion

        #region Fields

        /// <summary>
        /// The text that will be drawn.
        /// </summary>
        public string Text;

        /// <summary>
        /// Where the text will be drawn.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The font that the text will be drawn in.
        /// </summary>
        public SpriteFont Font;

        /// <summary>
        /// The scale at which the text will draw.
        /// </summary>
        public float Scale = 1f;

        /// <summary>
        /// The text's alpha, from 0 to 1.
        /// </summary>
        public float Alpha = 1f;

        /// <summary>
        /// The text's color.
        /// </summary>
        public Color Color;

        /// <summary>
        /// The text's rotation.
        /// </summary>
        public float Rotation = 0f;

        /// <summary>
        /// Any SpriteEffects to be applied to the text.
        /// </summary>
        public SpriteEffects SpriteEffects = SpriteEffects.None;

        /// <summary>
        /// The text's layer depth.
        /// </summary>
        public float LayerDepth = 1f;

        #endregion

        #region Properties

        /// <summary>
        /// The text's origin.
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                return Font.MeasureString(Text) / 2;
            }
        }

        /// <summary>
        /// The size of the text.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return Font.MeasureString(Text) * Scale;  
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color * Alpha, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
        }

        #endregion
    }
}
