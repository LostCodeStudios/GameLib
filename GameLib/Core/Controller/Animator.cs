using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLib.Core.View;
using Microsoft.Xna.Framework;

namespace GameLib.Core.Controller
{
    /// <summary>
    /// An animator controller
    /// </summary>
    public class Animator
    {
        #region Constructors
        public Animator(Animation animation, params Rectangle[] frames)
        {
            this.Animation = animation;
            this._Frames = frames;
        }
        #endregion

        #region Functioning Loop
        public void Update(GameTime gameTime)
        {
            //Animate forward.
            Animation.Step(_Frames.Length-1);
        }
        #endregion

        #region Fields

        /// <summary>
        /// The internal frames.
        /// </summary>
        private Rectangle[] _Frames;

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current animation
        /// </summary>
        public Animation Animation{ set; get; }
        /// <summary>
        /// Gets the frames on which the animator is acting.
        /// </summary>
        public Rectangle[] Frames
        {
            get
            {
                return _Frames;
            }
        }

        public Rectangle CurrentFrame
        {
            get
            {
                if (Animation.Index < Frames.Length && Animation.Index > 0)
                    return Frames[Animation.Index];
                else
                    return new Rectangle();
            }
        }

        #endregion

        #region Methods
        #endregion
    }
}
