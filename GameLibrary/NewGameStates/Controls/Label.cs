using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Helpers;

namespace GameLibrary.NewGameStates.Controls
{
    /// <summary>
    /// A standard UI label.
    /// </summary>
    public class Label : Control
    {
        #region Fields

        Color color;
        SpriteFont font;
        protected float scale;
        protected Vector2 position;
        protected Vector2 origin;

        protected string text;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a UI label.
        /// </summary>
        /// <param name="text">The label's text.</param>
        /// <param name="color">The label's color.</param>
        /// <param name="font">The SpriteFont that will be used for drawing this label.</param>
        /// <param name="scale">The scale of the label's text. Higher values will reduce text clarity.</param>
        /// <param name="fromScreenSide">The side of the screen from which the label will appear.</param>
        /// <param name="toScreenSide">The side of the screen to which the label will disappear.</param>
        /// <param name="transitionOnTime">The time it will take the label to transition onto the screen.</param>
        /// <param name="transitionOffTime">The time it will take the label to transition off the screen.</param>
        public Label(
            string text, 
            Vector2 position,
            Color color, 
            SpriteFont font, 
            float scale, 
            TransitionDirection fromScreenSide, 
            TransitionDirection toScreenSide,
            double transitionOnTime, 
            double transitionOffTime)
            : base(transitionOnTime, transitionOffTime, fromScreenSide, toScreenSide)
        {
            tabStop = false;

            this.text = text;
            this.position = position;
            this.color = color;
            this.font = font;
            this.scale = scale;

            SetOrigin();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Helper for recalculating the label's origin based on scale. Remember to call this when modifying scale in any subclass.
        /// </summary>
        protected void SetOrigin()
        {
            origin = TextSize / 2;
        }

        /// <summary>
        /// The dimmensions the label will be rendered with.
        /// </summary>
        protected Vector2 TextSize
        {
            get { return font.MeasureString(Text) * scale; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The label's text.
        /// </summary>
        public string Text
        {
            get { return text; }
        }

        #endregion

        #region Draw

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 pos = position;

            if (State == TransitionState.TransitionOff)
            {
                Vector2 transitionDestination = Vector2.Zero;

                switch (transitionOffDirection)
                {
                    case TransitionDirection.Top:

                        transitionDestination = new Vector2(position.X, -TextSize.Y);

                        break;

                    case TransitionDirection.Right:

                        transitionDestination = new Vector2(ScreenHelper.Viewport.Width + TextSize.X, position.Y);

                        break;

                    case TransitionDirection.Bottom:

                        transitionDestination = new Vector2(position.X, ScreenHelper.Viewport.Height + TextSize.Y);

                        break;

                    case TransitionDirection.Left:

                        transitionDestination = new Vector2(-TextSize.X, position.Y);

                        break;
                }

                pos = (transitionDestination - position) * TransitionFraction;
            }

            else if (State == TransitionState.TransitionOn)
            {
                Vector2 transitionStart = Vector2.Zero;

                switch (transitionOnDirection)
                {
                    case TransitionDirection.Top:

                        transitionStart = new Vector2(position.X, -TextSize.Y);

                        break;
                        
                    case TransitionDirection.Right:

                        transitionStart = new Vector2(ScreenHelper.Viewport.Width + TextSize.X, position.Y);

                        break;

                    case TransitionDirection.Bottom:

                        transitionStart = new Vector2(position.X, ScreenHelper.Viewport.Height + TextSize.Y);

                        break;

                    case TransitionDirection.Left:

                        transitionStart = new Vector2(-TextSize.X, position.Y);

                        break;
                }

                pos = (position - transitionStart) * TransitionFraction;
            }

            spriteBatch.DrawString(font, text, pos, color, 0f, origin, scale, SpriteEffects.None, 1f);
        }

        #endregion
    }
}
