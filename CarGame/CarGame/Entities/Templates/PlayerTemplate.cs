using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities;
using GameLibrary.Physics.Common;
using GameLibrary.Physics.Collision.Shapes;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Physics.Dynamics.Joints;
using GameLibrary.Entities.Components;
using GameLibrary;

namespace CarGame.Entities.Templates
{
    public class PlayerTemplate : IEntityTemplate
    {
        private World world;
        public PlayerTemplate(World world)
        {
            this.world = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "PLAYERS";
            e.Tag = "Player";

            #region Physical
            e.AddComponent<Physical>("WheelFront", new Physical(world, e));
            e.AddComponent<Physical>("WheelBack", new Physical(world, e));
            e.AddComponent<Physical>("Chassis", new Physical(world, e));

            Physical Chassis = e.GetComponent<Physical>("Chassis");
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
                Chassis.Position= new Vector2(0.0f, -1.0f);
                Chassis.CreateFixture(chassisShape);
            }
            Physical WheelBack = e.GetComponent<Physical>("WheelBack");
            {
                WheelBack.BodyType = BodyType.Dynamic;
                WheelBack.Position = new Vector2(-1.709f, -0.78f);
                Fixture fix = WheelBack.CreateFixture(new CircleShape(0.5f, 0.8f));
                fix.Friction = 0.9f;
            }


            Physical WheelFront = e.GetComponent<Physical>("WheelFront");
            {
                WheelFront.BodyType = BodyType.Dynamic;
                WheelFront.Position = new Vector2(1.54f, -0.8f);
                WheelFront.CreateFixture(new CircleShape(0.5f, 1f));
            }


            //Springs and stuff
            LineJoint _springBack, _springFront;
            Vector2 axis = new Vector2(0.0f, -1.2f);
            _springFront = new LineJoint(Chassis, WheelFront, WheelFront.Position, axis);
            _springFront.MotorSpeed = 0.0f;
            _springFront.MaxMotorTorque = 10.0f;
            _springFront.MotorEnabled = false;
            _springFront.Frequency = 8.5f;
            _springFront.DampingRatio = 0.85f;
            world.AddJoint(_springFront);

            _springBack = new LineJoint(Chassis, WheelBack, WheelBack.Position, axis);
            _springBack.MotorSpeed = 0.0f;
            _springBack.MaxMotorTorque = 20.0f;
            _springBack.MotorEnabled = true;
            _springBack.Frequency = 5.0f;
            _springBack.DampingRatio = 0.85f;
            world.AddJoint(_springBack);

            #endregion

            #region Sprites
            e.AddComponent<Sprite>("WheelFront", new Sprite(args[2] as Texture2D, (Rectangle)args[3]));
            e.AddComponent<Sprite>("WheelBack", new Sprite(args[2] as Texture2D, (Rectangle)args[3]));
            e.AddComponent<Sprite>("Chassis", new Sprite(args[0] as Texture2D, (Rectangle)args[1],
                Chassis, 1, Color.White,0f));

            #endregion

            return e;
        }


    }
}
