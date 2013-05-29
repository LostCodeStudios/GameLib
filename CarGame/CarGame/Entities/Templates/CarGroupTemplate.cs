using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Physics.Collision.Shapes;
using GameLibrary.Dependencies.Physics.Dynamics.Joints;
using GameLibrary.Entities.Components.Physics;

namespace CarGame.Entities.Templates
{
    class CarGroupTemplate : IEntityGroupTemplate
    {
        public Entity[] BuildEntityGroup(EntityWorld world, params object[] args)
        {
            Entity[] Car = new Entity[3];

            //Chassis
            Car[2] = world.CreateEntity("Chassis", args[0], args[1]);

            //Back wheel
            Car[1] = world.CreateEntity("Wheel", args[2], args[3],
                new Vector2(-1.709f, -0.78f), //pos
                new CircleShape(0.5f, 0.8f), //shape
                0.9f); //friction
            //Front wheel
            Car[0] = world.CreateEntity("Wheel", args[2], args[3],
                new Vector2(1.54f, -0.8f), //pos
                new CircleShape(0.5f, 1f)); //shape

            //Now to create the springs.
            LineJoint _springBack, _springFront;
            Vector2 axis = new Vector2(0.0f, -1.2f);
            //front spring
            _springFront = new LineJoint(Car[2].GetComponent<Body>(),
                Car[0].GetComponent<Body>(), Car[0].GetComponent<Body>().Position, axis);
            _springFront.MotorSpeed = 0.0f;
            _springFront.MaxMotorTorque = 10.0f;
            _springFront.MotorEnabled = false;
            _springFront.Frequency = 8.5f;
            _springFront.DampingRatio = 0.85f;
            world.AddJoint(_springFront);

            // back spring with a motor
            _springBack = new LineJoint(Car[2].GetComponent<Body>(),
               Car[1].GetComponent<Body>(), Car[1].GetComponent<Body>().Position, axis);
            _springBack.MotorSpeed = 0.0f;
            _springBack.MaxMotorTorque = 20.0f;
            _springBack.MotorEnabled = true;
            _springBack.Frequency = 5.0f;
            _springBack.DampingRatio = 0.85f;
            world.AddJoint(_springBack);

            //Send it off!
            return Car;
        }
    }
}
