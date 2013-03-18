using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Collision.Shapes;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Factories;
using Microsoft.Xna.Framework;

namespace CarGame.Entities.Templates
{
    class BridgeGroupTemplate : IEntityGroupTemplate
    {
        public Entity[] BuildEntityGroup(EntityWorld world, params object[] args)
        {
            const int segmentCount = 20;
            Entity[] segments = new Entity[segmentCount];

            //Make teh bridge segments and bind dem togeder
            PolygonShape shape = new PolygonShape(1f);
            shape.SetAsBox(1.0f, 0.125f);
            Body prevBody = args[0] as Body;
            for (int i = 0; i < segmentCount; ++i)
            {
                Entity segment = world.CreateEntity();
                segment.Tag = "segment" + i;
                Body body = segment.AddComponent<Body>(new Body(world, segment)); 
     
                body.BodyType = BodyType.Dynamic;
                body.Position = new Vector2(161f + 2f * i, 0.125f);
                Fixture fix = body.CreateFixture(shape);
                fix.Friction = 0.6f;
                JointFactory.CreateRevoluteJoint(world, prevBody, body, -Vector2.UnitX);

                segments[i] = segment;
                prevBody = body;
            }
            JointFactory.CreateRevoluteJoint(world, args[0] as Body, prevBody, Vector2.UnitX);

            return segments;
        }
    }
}
