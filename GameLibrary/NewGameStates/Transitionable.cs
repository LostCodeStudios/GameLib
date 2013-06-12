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
    public enum TransitionDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }

    /// <summary>
    /// Base class for anything that transitions on and off of the screen. 
    /// Subclasses will have to override the Draw call to handle the visual transition.
    /// </summary>
    public abstract class Transitionable
    {
        #region Fields

        TransitionState state;

        TimeSpan transitionOn;
        TimeSpan transitionOff;
        TimeSpan elapsedTransition;

        protected TransitionDirection transitionOnDirection;
        protected TransitionDirection transitionOffDirection;



        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a Transitionable with the given transition times and screen directions.
        /// </summary>
        /// <param name="transitionOnTime"></param>
        /// <param name="transitionOffTime"></param>
        public Transitionable(double transitionOnTime, double transitionOffTime, TransitionDirection transitionOnDirection, TransitionDirection transitionOffDirection)
        {
            elapsedTransition = TimeSpan.Zero;
            state = TransitionState.Off;

            transitionOn = TimeSpan.FromSeconds(transitionOnTime);
            transitionOff = TimeSpan.FromSeconds(transitionOffTime);

            this.transitionOnDirection = transitionOnDirection;
            this.transitionOffDirection = transitionOffDirection;
        }

        #endregion

        #region Helper Properties

        /// <summary>
        /// Simple helper to determine if the Transitionable is in a transition.
        /// </summary>
        protected bool Transitioning
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

        public bool TransitionOn()
        {
            if (state != TransitionState.Off)
                return false;

            elapsedTransition = TimeSpan.Zero;
            state = TransitionState.TransitionOn;

            return true;
        }

        public bool TransitionOff()
        {
            if (state != TransitionState.On)
                return false;
            
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

        /// <summary>
        /// Pure virtual Draw method to be defined by subclasses.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);

        #endregion
    }
}
