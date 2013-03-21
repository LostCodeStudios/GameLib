using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Dependencies.Physics.Dynamics;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Physics.Collision.Shapes;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework.Graphics;

namespace CarGame.Entities.Templates.Car
{
    class WheelTemplate : IEntityTemplate
    {
        World _World;
        public WheelTemplate(World world)
        {
            this._World = world;
        }
        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Tag = "Wheel" + e.Id;

            #region Body
            Body Wheel = e.AddComponent<Body>(new Body(_World, e));
            {
                Wheel.BodyType = BodyType.Dynamic;
                Wheel.Position = (Vector2)args[2];
                Fixture fix = Wheel.CreateFixture(args[3] as CircleShape);
                if (args.Length > 4 && args[4] != null)
                    fix.Friction = (float)args[4];
            }

            #endregion

            #region Sprite
            e.AddComponent<Sprite>(new Sprite(args[0] as Texture2D, (Rectangle)args[1]));
            #endregion
            return e;
        }
    }
}
