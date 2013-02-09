using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Collision.Shapes;
using GameLibrary.Dependencies.Physics.Common;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;

namespace CarGame.Entities.Templates.Car
{
    class ChassisTemplate : IEntityTemplate
    {
        World _World;
        public ChassisTemplate(World world)
        {
            this._World = world;
        }
        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Tag = "Chassis";

            #region Body
            Body Chassis = e.AddComponent<Body>(new Body(_World, e));
            {
                Vertices vertices = new Vertices(8);
                vertices.Add(new Vector2(-2.5f, 0.08f));
                vertices.Add(new Vector2(-2.375f, -0.46f));
                vertices.Add(new Vector2(-0.58f, -0.92f));
                vertices.Add(new Vector2(0.46f, -0.92f));
                vertices.Add(new Vector2(2.5f, -0.17f));
                vertices.Add(new Vector2(2.5f, 0.205f));
                vertices.Add(new Vector2(2.3f, 0.33f));
                vertices.Add(new Vector2(-2.25f, 0.35f));
                PolygonShape chassisShape = new PolygonShape(vertices, 2f);

                Chassis.BodyType = BodyType.Dynamic;
                Chassis.Position = new Vector2(0.0f, -1.0f);
                Chassis.CreateFixture(chassisShape);
            }
            #endregion

            #region Sprite
            e.AddComponent<Sprite>(new Sprite(args[0] as Texture2D, (Rectangle)args[1],
               Chassis, 1, Color.White, 0f));
            #endregion
            return e;
        }
    }
}
