using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.NewGameStates.Controls
{
    /// <summary>
    /// A standard UI label.
    /// </summary>
    public class Label : Control
    {
        /// <summary>
        /// Constructs a label.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="transitionOnTime"></param>
        /// <param name="transitionOffTime"></param>
        /// <param name="transitionOnSide"></param>
        /// <param name="transitionOffSide"></param>
        public Label(
            string text,
            Vector2 position, 
            double transitionOnTime, 
            double transitionOffTime, 
            ScreenSide transitionOnSide,
            ScreenSide transitionOffSide)
            : base(transitionOnTime, transitionOffTime, transitionOnSide, transitionOffSide, null, position)
        {
            this.text = new TextComponent(text, position);
        }

        /// <summary>
        /// The label's TextComponent.
        /// </summary>
        public TextComponent Text
        {
            get { return text; }
        }

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            text.Alpha = transition.TransitionFraction;
            text.Position = DrawPosition;
            text.Draw(spriteBatch);
        }
    }
}
