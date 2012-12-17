using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Core.View.Animations
{
    /// <summary>
    /// Animation that will always stay on the first frame.
    /// </summary>
    class StaticAnimation : Animation
    {
        public StaticAnimation(int firstFrame)
        {
            this._FirstFrame = firstFrame;
        }
        /// <summary>
        /// Does nothing to the static animation.
        /// </summary>
        /// <param name="max"></param>
        /// <returns>(The first frame)</returns>
        public override int Step(int max)
        {
            return _FirstFrame;
        }

        /// <summary>
        /// Gets the first frame
        /// </summary>
        public override int Index
        {
            get { return _FirstFrame; }
        }

        #region Fields
        int _FirstFrame;
        #endregion
    }
}
