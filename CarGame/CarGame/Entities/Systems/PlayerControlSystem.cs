using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;

namespace CarGame.Entities.Systems
{
    public class PlayerControlSystem : TagSystem
    {
        ComponentMapper<Body> physicalMapper;
        KeyboardState keyState;
        public float acceleration = 0;

        public PlayerControlSystem()
            : base("Chassis")
        {
        }

        public override void Initialize()
        {
            physicalMapper = new ComponentMapper<Body>(world);
        }

        public void UpdateInput(KeyboardState ks)
        {
            keyState = ks;
        }

        public override void Process(Entity e)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                this.acceleration = Math.Min(this.acceleration + (float)(2.0 * (float)world.Delta/1000f), 1f);
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
                this.acceleration = Math.Min(this.acceleration + (float)(2.0 * (float)world.Delta/1000f), -1f);
            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                this.acceleration = 0;
            else
                this.acceleration -= Math.Sign(this.acceleration) * (float)(2.0 * (float)world.Delta/1000f);

            //Process motor
            Body body = physicalMapper.Get(e);

            LineJoint _springBack = (LineJoint)body.JointList.Joint;
            _springBack.MotorSpeed = Math.Sign(acceleration) *
                         MathHelper.SmoothStep(0f, 50, Math.Abs(acceleration));
            if (Math.Abs(_springBack.MotorSpeed) < 50 * 0.06f)
            {
                _springBack.MotorEnabled = false;
            }
            else
            {
                _springBack.MotorEnabled = true;
            }
        }



        
    }
}
