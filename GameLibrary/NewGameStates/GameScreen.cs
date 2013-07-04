using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.NewGameStates
{
    /// <summary>
    /// Enum for determining when any significant change happens to a transition.
    /// </summary>
    public enum TransitionEvent
    {
        BeginTransitionOn,
        EndTransitionOn,
        BeginTransitionOff,
        EndTransitionOff,
        None
    }

    /// <summary>
    /// Abstract superclass for all GameScreens in the game's menu interface.
    /// </summary>
    public abstract class GameScreen
    {
        #region Fields

        /// <summary>
        /// The screen's transition on- and off-screen.
        /// </summary>
        protected Transition transition;

        /// <summary>
        /// The screen's ControlManager.
        /// </summary>
        protected ControlManager Controls;

        /// <summary>
        /// Whether this screen covers all screens beneath it.
        /// </summary>
        public bool FullScreen
        {
            get { return fullscreen; }
        }
        bool fullscreen;

        #endregion

        #region Initialization

        /// <summary>
        /// Construct a GameScreen.
        /// </summary>
        /// <param name="transitionOnTime"></param>
        /// <param name="transitionOffTime"></param>
        public GameScreen(double transitionOnTime, double transitionOffTime, bool fullscreen, PlayerIndex controllingIndex)
        {
            transition = new Transition(transitionOnTime, transitionOffTime);

            this.fullscreen = fullscreen;

            Controls = new ControlManager();
            Controls.ControllingIndex = controllingIndex;
            AddControls(transitionOnTime, transitionOffTime);
        }

        /// <summary>
        /// Adds all of the GameScreen's controls to the ControlManager, setting their transitionOn and transitionOff times according to the parameters.
        /// </summary>
        /// <param name="transitionOnTime"></param>
        /// <param name="transitionOffTime"></param>
        public abstract void AddControls(double transitionOnTime, double transitionOffTime);

        #endregion

        #region Transition

        /// <summary>
        /// Transitions the GameScreen onto the screen.
        /// </summary>
        public void TransitionOn()
        {
            transition.TransitionOn();

            Controls.TransitionOn();
        }

        /// <summary>
        /// Transitions the GameScreen off the screen.
        /// </summary>
        public void TransitionOff()
        {
            transition.TransitionOff();

            Controls.TransitionOff();
        }

        #endregion

        #region Update & Draw

        /// <summary>
        /// Updates the GameScreen.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>The TransitionEvent describing what has happened with the transition.</returns>
        public virtual TransitionEvent Update(GameTime gameTime)
        {
            Controls.Update(gameTime);

            TransitionState prevState = transition.State;

            transition.Update(gameTime);

            if (prevState == TransitionState.On && transition.State == TransitionState.TransitionOff)
            {
                return TransitionEvent.BeginTransitionOff;
            }
            else if (prevState == TransitionState.Off && transition.State == TransitionState.TransitionOn)
            {
                return TransitionEvent.BeginTransitionOn;
            }
            else if (prevState == TransitionState.TransitionOn && transition.State == TransitionState.On)
            {
                return TransitionEvent.EndTransitionOn;
            }
            else if (prevState == TransitionState.TransitionOff && transition.State == TransitionState.Off)
            {
                return TransitionEvent.EndTransitionOff;
            }
            else
            {
                return TransitionEvent.None;
            }
        }

        /// <summary>
        /// Draws the GameScreen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Controls.Draw(spriteBatch);
        }

        #endregion
    }
}
