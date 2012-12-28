using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace GameLibrary.Entities.Components.Physics
{
    /// <summary>
    /// A non physical body which does not interact with the farseer physics library
    /// </summary>
    public class Particle : Component, ITransform, IVelocity
    {
        public Particle(Entity e, Vector2 position, float rotation, Vector2 linearVelocity, float angularVelocity)
        {
            e.AddComponent<ITransform>(this);
            e.AddComponent<IVelocity>(this);

            this.Position = position;
            this.Rotation = rotation;
            this.LinearVelocity = linearVelocity;
            this.AngularVelocity = angularVelocity;
        }

        public Particle(Entity e)
            : this(e, Vector2.Zero, 0f, Vector2.Zero, 0.0f)
        {
        }

        #region ITransform
        /// <summary>
        /// The world position of a particle.
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 Position
        {
            set;
            get;
        }

        /// <summary>
        /// The rotation of a particle.
        /// </summary>
        public float Rotation
        {
            get;
            set;
        }
        #endregion

        #region Velocity
        /// <summary>
        /// The linear velocity of a particle.
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 LinearVelocity
        {
            set;
            get;
        }

        /// <summary>
        /// The angular velocity of a particle.
        /// </summary>
        public float AngularVelocity
        {
            set;
            get;
        }
        #endregion
    }
}
