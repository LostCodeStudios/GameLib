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
    /// A standard UI button.
    /// </summary>
    public class Button : Control
    {
        #region Constructor

        /// <summary>
        /// Constructs a button.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="transitionOnTime"></param>
        /// <param name="transitionOffTime"></param>
        /// <param name="transitionOnSide"></param>
        /// <param name="transitionOffSide"></param>
        public Button(
            string text, 
            Vector2 position, 
            Color color,
            Color selectedColor,
            float scale,
            float selectedScale,
            double selectionTransitionOnTime,
            double selectionTransitionOffTime,
            double transitionOnTime, 
            double transitionOffTime, 
            ScreenSide transitionOnSide, 
            ScreenSide transitionOffSide)
            : base(transitionOnTime, transitionOffTime, transitionOnSide, transitionOffSide, null, position)
        {
            this.text = new TextComponent(text, position);

            this.color = color;
            this.selectedColor = selectedColor;

            this.scale = scale;
            this.selectedScale = selectedScale;

            this.selectionTransition = new Transition(selectionTransitionOnTime, selectionTransitionOffTime);

            this.OnSelection += Button_Selection;
            this.OnDeselection += Button_Deselection;

            this.TabStop = true;
        }

        #endregion

        #region Fields

        Color color;
        Color selectedColor;

        float scale;
        float selectedScale;

        Transition selectionTransition;

        /// <summary>
        /// Event handler to be fired when the button is pressed by the user.
        /// </summary>
        public event EventHandler OnTrigger;

        #endregion

        /// <summary>
        /// The button's TextComponent
        /// </summary>
        public TextComponent Text
        {
            get { return text; }
        }

        #region Events

        /// <summary>
        /// Begins the transition from deselected to selected.
        /// </summary>
        void Button_Selection()
        {
            selectionTransition.TransitionOn();
        }

        /// <summary>
        /// Begins the transition from selected to deselected.
        /// </summary>
        void Button_Deselection()
        {
            selectionTransition.TransitionOff();
        }

        #endregion

        public override void Update(GameTime gameTime, ControlManager manager)
        {
            base.Update(gameTime, manager);

            selectionTransition.Update(gameTime);
        }

        /// <summary>
        /// Draws the button.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            text.Alpha = transition.TransitionFraction;
            text.Position = DrawPosition;

            switch (selectionTransition.State)
            {
                case TransitionState.Off:
                    text.Scale = scale;
                    text.Color = color;
                    break;

                case TransitionState.On:
                    text.Scale = selectedScale;
                    text.Color = selectedColor;
                    break;

                case TransitionState.TransitionOn:
                    text.Scale = scale + (selectedScale - scale) * selectionTransition.TransitionFraction;
                    text.Color = Color.Lerp(color, selectedColor, selectionTransition.TransitionFraction);
                    break;

                case TransitionState.TransitionOff:
                    text.Scale = selectedScale + (scale - selectedScale) * selectionTransition.TransitionFraction;
                    text.Color = Color.Lerp(selectedColor, color, selectionTransition.TransitionFraction);
                    break;
            }

            text.Draw(spriteBatch);
        }
    }
}
