using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.NewGameStates
{
    /// <summary>
    /// Enum representing a Transitionable object's current state of transition.
    /// </summary>
    public enum TransitionState
    {
        Off,
        On,
        TransitionOn,
        TransitionOff
    }

    /// <summary>
    /// Represents a side of the screen from which a transitioning control will appear.
    /// </summary>
    public enum ScreenSide
    {
        Top,
        Bottom,
        Left,
        Right
    }

    /// <summary>
    /// Helper to be held by any class that needs to handle a transition of any kind. 
    /// A class could theoretically contain multiple transitions if necessary.
    /// </summary>
    public class Transition
    {
        #region Fields

        TransitionState state;

        TimeSpan transitionOn;
        TimeSpan transitionOff;
        TimeSpan elapsedTransition;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a Transitionable with the given transition times and screen directions.
        /// </summary>
        /// <param name="transitionOnTime"></param>
        /// <param name="transitionOffTime"></param>
        public Transition(double transitionOnTime, double transitionOffTime)
        {
            elapsedTransition = TimeSpan.Zero;
            state = TransitionState.Off;

            transitionOn = TimeSpan.FromSeconds(transitionOnTime);
            transitionOff = TimeSpan.FromSeconds(transitionOffTime);
        }

        #endregion

        #region Helper Properties

        /// <summary>
        /// Simple helper to determine if the Transitionable is in a transition.
        /// </summary>
        public bool Transitioning
        {
            get
            { 
                return state == TransitionState.TransitionOff 
                    || state == TransitionState.TransitionOn; 
            }
        }

        /// <summary>
        /// The time for the current transition.
        /// </summary>
        protected TimeSpan TransitionTime
        {
            get
            {
                if (state == TransitionState.Off || state == TransitionState.On)
                {
                    return TimeSpan.Zero;
                }

                return state == TransitionState.Off ? transitionOff : transitionOn;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The object's current state of transition.
        /// </summary>
        public TransitionState State
        {
            get { return state; }
        }

        /// <summary>
        /// A decimal representation of the object's progress through transitioning, between 0 and 1.
        /// </summary>
        public float TransitionFraction
        {
            get
            {
                if (!Transitioning)
                {
                    return 1f;
                }

                else
                {
                    return (float)(elapsedTransition.TotalSeconds / TransitionTime.TotalSeconds);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initiates the opening transition, if necessary.
        /// </summary>
        /// <returns>Whether or not a transition was actually necessary.</returns>
        public bool TransitionOn()
        {
            if (state == TransitionState.On || state == TransitionState.TransitionOn || transitionOn == TimeSpan.Zero)
                return false;

            if (state == TransitionState.TransitionOff)
            {
                float fraction = 1f - TransitionFraction;
                double seconds = (double)fraction * transitionOn.TotalSeconds;
                elapsedTransition = TimeSpan.FromSeconds(seconds);
            }
            else
                elapsedTransition = TimeSpan.Zero;

            state = TransitionState.TransitionOn;

            return true;
        }

        /// <summary>
        /// Initiates the closing transition, if necessary.
        /// </summary>
        /// <returns>Whether or not a transition was actually necessary.</returns>
        public bool TransitionOff()
        {
            if (state == TransitionState.Off || state == TransitionState.TransitionOff || transitionOff == TimeSpan.Zero)
                return false;

            if (state == TransitionState.TransitionOn)
            {
                float fraction = 1f - TransitionFraction;
                double seconds = (double)fraction * transitionOn.TotalSeconds;
                elapsedTransition = TimeSpan.FromSeconds(seconds);
            }
            else
                elapsedTransition = TimeSpan.Zero;

            state = TransitionState.TransitionOff;

            return true;
        }

        #endregion

        #region Update/Draw

        /// <summary>
        /// Updates the Transitionable's transition.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            if (Transitioning)
            {
                elapsedTransition += gameTime.ElapsedGameTime;

                if (elapsedTransition >= TransitionTime)
                {
                    elapsedTransition = TimeSpan.Zero;

                    switch (state)
                    {
                        case TransitionState.TransitionOff:
                            state = TransitionState.Off;
                            break;

                        case TransitionState.TransitionOn:
                            state = TransitionState.On;
                            break;
                    }
                }
            }
        }

        #endregion
    }
}
