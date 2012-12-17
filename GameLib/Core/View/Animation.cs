using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLib.Core.View.Animations;

namespace GameLib.Core.View
{
    /// <summary>
    /// The animation class
    /// </summary>
    public abstract class Animation
    {
        /// <summary>
        /// Steps the animation forward in whichever manner specific to the animation.
        /// </summary>
        /// <param name="max">The maximum frame value that the animation can go to.</param>
        /// <returns></returns>
        public abstract int Step(int max);

        /// <summary>
        /// Gets the current index to which the animation has stepped.
        /// </summary>
        public abstract int Index { get; }


        #region Presets
        public static Animation Static
        {
            get
            {
                return new StaticAnimation(0);
            }
        }
        #endregion
    }
}
