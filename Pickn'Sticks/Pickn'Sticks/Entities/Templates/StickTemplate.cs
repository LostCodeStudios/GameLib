using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework.Graphics;

namespace Pickn_Sticks.Entities.Templates
{
    class StickTemplate : IEntityTemplate
    {
        EntityWorld _World;
        public StickTemplate(EntityWorld world)
        {
            this._World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Sticks";
            Rectangle source = (Rectangle)args[1];

            FixtureFactory.AttachRectangle(
                ConvertUnits.ToSimUnits(source.Width),
                ConvertUnits.ToSimUnits(source.Height),
                1f, Vector2.Zero, e.AddComponent<Body>(new Body(_World, e))); //Body
            e.GetComponent<Body>().BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            e.GetComponent<Body>().LinearDamping = 4;
            e.GetComponent<Body>().AngularDamping = 4;
            e.GetComponent<Body>().Mass = 1;
            Random r = new Random();
            e.GetComponent<Body>().Position = ConvertUnits.ToSimUnits(new Vector2(r.Next(0, ScreenHelper.GraphicsDevice.Viewport.Width), r.Next(0, ScreenHelper.GraphicsDevice.Viewport.Height))
                    - new Vector2(ScreenHelper.GraphicsDevice.Viewport.Bounds.Center.X, ScreenHelper.GraphicsDevice.Viewport.Bounds.Center.Y) + new Vector2(15));
            e.AddComponent<Sprite>(new Sprite(args[0] as Texture2D, source, e.GetComponent<Body>(),1f,Color.White,0.1f)); //Sprite

            return e;
        }
    }
}
