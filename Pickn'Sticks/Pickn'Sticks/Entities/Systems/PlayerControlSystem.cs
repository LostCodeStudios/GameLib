using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Dynamics.Contacts;

namespace Pickn_Sticks.Entities.Systems
{
    public class PlayerControlSystem : TagSystem
    {
        KeyboardState keyState;
        ComponentMapper<Body> bodyMapper;
        float _Velocity;
        bool WasMoving = false;
        int AnimationHeight = 30;

        public PlayerControlSystem(float velocity)
            : base("Player")
        {
            this._Velocity = velocity;
        }

        public override void Initialize()
        {
            bodyMapper = new ComponentMapper<Body>(world);
        }

        public override void Process(Entity e)
        {
            Body b = bodyMapper.Get(e);

            #region UserMovement
            if (WasMoving) //Stops movement
            {
                b.LinearDamping = (float)Math.Pow(_Velocity, _Velocity*4);
                WasMoving = false;
            }
            else
                b.LinearDamping = 0;

            Vector2 target = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.D)){ //Right
                target += Vector2.UnitX;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A)){ //Left
                target += -Vector2.UnitX;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S)){ //Down
                target += Vector2.UnitY;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W)){ //Up?
                target += -Vector2.UnitY;
            }

            if (target != Vector2.Zero) //If being moved by player
            {
                WasMoving = true;
                b.LinearDamping = _Velocity*2;
            }

            //Rotation
            if (b.LinearVelocity != Vector2.Zero)
                //b.Rotation = MathHelper.SmoothStep(b.Rotation, (float)Math.Atan2(b.LinearVelocity.Y, b.LinearVelocity.X) + (float)Math.PI/2f, 0.1f);
                b.Rotation = (float)Math.Atan2(b.LinearVelocity.Y, b.LinearVelocity.X) + (float)Math.PI / 2f;


            //update position
            b.ApplyLinearImpulse((target)*new Vector2(_Velocity));
            #endregion

            #region Animation
            if (target != Vector2.Zero && b.LinearVelocity.Length() != 0 && (int)(5/Math.Pow(b.LinearVelocity.Length(), 1 / 2)) != 0)
            { //if player is being moved.
                if (world.StepCount % (int)(5 / Math.Pow(b.LinearVelocity.Length(), 1 / 2)) == 0)
                {
                    AnimationHeight += 30;
                    AnimationHeight %= 90; //Max height on spritesheet.
                }
            }
            else
                AnimationHeight = 30;
            Sprite s = e.GetComponent<Sprite>();
            s = new Sprite(s.SpriteSheet, new Rectangle(15, AnimationHeight, 50, 30), s.Origin, s.Scale, s.Color, s.Layer);
            e.RemoveComponent(ComponentTypeManager.GetTypeFor<Sprite>());
            e.AddComponent<Sprite>(s);
            #endregion

        }
    }
}
