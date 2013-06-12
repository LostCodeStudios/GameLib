using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Input;

namespace GameLibrary.NewGameStates
{
    public abstract class Control : Transitionable, IHandlesInput
    {
        #region Fields

        protected bool tabStop;
        protected bool enabled;
        protected bool visible;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a control with the desired transition times.
        /// </summary>
        /// <param name="transitionOnTime"></param>
        /// <param name="transitionOffTime"></param>
        public Control(double transitionOnTime, double transitionOffTime, TransitionDirection transitionOnDirection, TransitionDirection transitionOffDirection)
            : base(transitionOnTime, transitionOffTime, transitionOnDirection, transitionOffDirection)
        {
        }

        #endregion

        #region Properties

        public bool TabStop
        {
            get { return tabStop; }
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        public bool Visible
        {
            get { return visible; }
        }

        #endregion

        #region Input

        public void HandleInput(InputState input)
        {
        }

        #endregion
    }
}
