using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.NewGameStates
{
    public abstract class Control
    {
        #region Fields

        /// <summary>
        /// The control's transition onto the screen.
        /// </summary>
        protected Transition transition;

        /// <summary>
        /// The side of the screen that the control will transition on from.
        /// </summary>
        protected ScreenSide transitionOnSide;

        /// <summary>
        /// The side of the screen that the control will transition off to.
        /// </summary>
        protected ScreenSide transitionOffSide;

        /// <summary>
        /// The control's text.
        /// </summary>
        protected TextComponent text;

        /// <summary>
        /// The control's position, when fully transitioned on-screen.
        /// </summary>
        public Vector2 Position
        {
            get;
            protected set;
        }

        /// <summary>
        /// Whether this control can be selected by the user.
        /// </summary>
        public bool TabStop
        {
            get;
            protected set;
        }

        /// <summary>
        /// Whether this control is enabled.
        /// </summary>
        public bool Enabled = true;

        /// <summary>
        /// Whether this control is visible.
        /// </summary>
        public bool Visible = true;

        /// <summary>
        /// Whether the control is selected.
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (value != selected) //Only use callbacks if the state has changed
                {
                    if (!selected && OnSelection != null)
                    {
                        OnSelection();
                    }
                    else if (OnDeselection != null)
                    {
                        OnDeselection();
                    }
                }
                selected = value;
            }
        }
        bool selected = false;

        /// <summary>
        /// Action to be triggered when this control becomes selected.
        /// </summary>
        public event Action OnSelection;

        /// <summary>
        /// Action to be triggered when this control is no longer selected.
        /// </summary>
        public event Action OnDeselection;

        /// <summary>
        /// The position where the control should draw at this frame in its transition.
        /// </summary>
        protected Vector2 DrawPosition;

        /// <summary>
        /// The rectangle for determining if the mouse is hovering over this control.
        /// </summary>
        protected Rectangle Bounds;

        #endregion

        #region Constructor

        public Control(
            double transitionOnTime, 
            double transitionOffTime,
            ScreenSide transitionOnSide, 
            ScreenSide transitionOffSide, 
            TextComponent text,
            Vector2 position)
        {
            transition = new Transition(transitionOnTime, transitionOffTime);
            this.transitionOnSide = transitionOnSide;
            this.transitionOffSide = transitionOffSide;
            this.text = text;
            Position = position;
        }

        #endregion

        #region Transition

        public virtual void TransitionOn()
        {
            transition.TransitionOn();
        }

        public virtual void TransitionOff()
        {
            transition.TransitionOff();
        }

        public virtual bool Transitioning
        {
            get
            {
                return (transition.State != TransitionState.Off && transition.State != TransitionState.On); 
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Runs the control's logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime, ControlManager manager)
        {
            transition.Update(gameTime);

            UpdatePosition();

            if (manager.Enabled && !Transitioning && TabStop) //Don't take input if the manager isn't.
            {
                UpdateBounds();

                MouseState ms = Mouse.GetState();

                bool selected;
                selected = (ms.X > Bounds.X && ms.X < Bounds.Right
                    && ms.Y > Bounds.Y && ms.Y < Bounds.Bottom);

                Selected = selected;
            }
        }

        /// <summary>
        /// Updates the position where the control will draw to. This changes as the control transitions.
        /// </summary>
        protected virtual void UpdatePosition()
        {

            if (transition.State == TransitionState.Off)
            {
                DrawPosition = new Vector2(-ScreenHelper.Viewport.Width, 0); //Safely offscreen
                return;
            }

            if (transition.State == TransitionState.On)
            {
                DrawPosition = Position; //Draw where it should be
                return;
            }

            Vector2 onScreen = Position;

            Vector2 offScreen = Vector2.Zero;

            switch (transitionOnSide)
            {
                case ScreenSide.Top:
                    offScreen = new Vector2(Position.X, -text.Size.Y);
                    break;
                    
                case ScreenSide.Right:
                    offScreen = new Vector2(ScreenHelper.Viewport.Width + text.Size.X, Position.Y);
                    break;

                case ScreenSide.Bottom:
                    offScreen = new Vector2(Position.X, ScreenHelper.Viewport.Height + text.Size.Y);
                    break;

                case ScreenSide.Left:
                    offScreen = new Vector2(-text.Size.X, Position.Y);
                    break;
            }

            Vector2 fromPosition; //The starting point of the current transition
            Vector2 toPosition; //The ending point of the current transition

            if (transition.State == TransitionState.TransitionOn)
            {
                fromPosition = offScreen;
                toPosition = onScreen;
            }

            else
            {
                fromPosition = onScreen;
                toPosition = offScreen;
            }

            DrawPosition = fromPosition + ((toPosition - fromPosition) * transition.TransitionFraction);
        }

        /// <summary>
        /// Calculates the bounds for determining if the mouse is hovering over this control.
        /// </summary>
        protected virtual void UpdateBounds()
        {
            int width, height;
            width = (int)text.Size.X;
            height = (int)text.Size.Y / 2;

            int x, y;
            x = (int)Position.X - width / 2;
            y = (int)Position.Y - height / 2;

            Bounds = new Rectangle(x, y, width, height);
        }

        #endregion

        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
