using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Input;

namespace GameLibrary.GameStates2
{
    public abstract class Control : Transitionable, IHandlesInput
    {
        #region Fields

        bool tabStop;
        protected bool enabled;
        protected bool visible;

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
