using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using GameLibrary.Entities;
using GameLibrary.Physics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;

namespace CarGame.Entities.Systems
{
    public class PlayerControlSystem : TagSystem
    {
        ComponentMapper<Physical> physicalMapper;
        KeyboardState keyState;
        public float acceleration = 0;

        public PlayerControlSystem()
            : base("Player")
        {
        }

        public override void Initialize()
        {
            physicalMapper = new ComponentMapper<Physical>(world);
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
            Dictionary<string, Physical> bodies = physicalMapper.Get(e);

            LineJoint _springBack = (LineJoint)bodies["Chassis"].JointList.Joint;
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
