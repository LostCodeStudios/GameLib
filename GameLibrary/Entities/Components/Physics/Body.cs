using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Physics.Dynamics;


namespace GameLibrary.Entities.Components.Physics
{
    /// <summary>
    /// A physical component is a body. Physical components are always coupled with Transform & Velocity
    /// </summary>
    public class Body : GameLibrary.Dependencies.Physics.Dynamics.PhysicsBody, Component, Transform, Velocity
    {
        public Body(EntityWorld world, Entity e)
            : base(world, e)
        {
            //Add transform and velocity
            e.AddComponent<Transform>(this);
            e.AddComponent<Velocity>(this);
        }
        ~Body()
        {
            this.World.RemoveBody(this);
            
        }

        #region Transform
        /// <summary>
        /// The position of an entity.
        /// </summary>
        Vector2 Transform.Position
        {
            get
            {
                return this.Position;
            }
            set
            {
                this.Position = value;
            }
        }

        /// <summary>
        /// The rotation of an entity.
        /// </summary>
        float Transform.Rotation
        {
            get
            {
                return this.Rotation;
            }
            set
            {
                this.Rotation = Rotation;
            }
        }
        #endregion

        #region Velocity
        /// <summary>
        /// The linear velocity of an entity.
        /// </summary>
        Vector2 Velocity.LinearVelocity
        {
            get
            {
                return this.LinearVelocity;
            }
            set
            {
                this.LinearVelocity = value;
            }
        }

        /// <summary>
        /// The angular velocity of an entity.
        /// </summary>
        float Velocity.AngularVelocity
        {
            get
            {
                return this.AngularVelocity;
            }
            set
            {
                this.AngularVelocity = value;
            }
        }
        #endregion

        #region Helpers
        public override string ToString()
        {
            return "[(Pos=" + this.Position
                + "),\n                (LVel=" + this.LinearVelocity
                + "),\n                (AVel=" + this.AngularVelocity
                + "),\n                (Ent=" + this.UserData + ")]";
        }
        #endregion

    }
}
