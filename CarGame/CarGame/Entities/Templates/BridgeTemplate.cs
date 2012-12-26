using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Physics.Collision.Shapes;
using Microsoft.Xna.Framework;
using GameLibrary.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary;

namespace CarGame.Entities.Templates
{
    class BridgeTemplate : IEntityTemplate
    {
        private World world;
        public BridgeTemplate(World world)
        {
            this.world = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Props";
            e.Tag = "Bridge";

            const int segmentCount = 20;
            PolygonShape shape = new PolygonShape(1f);
            shape.SetAsBox(1.0f, 0.125f);

            Physical prevBody = args[0] as Physical;
            for (int i = 0; i < segmentCount; ++i)
            {
                Physical body = e.AddComponent<Physical>("segment" + i, new Physical(world, e, "segment" + i));
                body.BodyType = BodyType.Dynamic;
                body.Position = new Vector2(161f + 2f * i, 0.125f);
                Fixture fix = body.CreateFixture(shape);
                fix.Friction = 0.6f;
                JointFactory.CreateRevoluteJoint(world, prevBody, body, -Vector2.UnitX);

                prevBody = body;
            }
            JointFactory.CreateRevoluteJoint(world, args[0] as Physical, prevBody, Vector2.UnitX);

            return e;
        }
    }
}
