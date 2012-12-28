using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;

namespace GameLibrary.Entities.Systems
{
    public class ParticleMovementSystem : ParallelEntityProcessingSystem
    {
        ComponentMapper<Particle> particleMapper;
        public ParticleMovementSystem()
            : base(typeof(Particle))
        {
        }
        

        public override void Initialize()
        {
            particleMapper = new ComponentMapper<Particle>(world);
        }

        public override void Process(Entity e)
        {
            Particle particle = particleMapper.Get(e);

            //Add the velocity of a particle to its transform
            particle.Position += particle.LinearVelocity * new Vector2(World.Delta / 1000f); //x = int(dx/dt)[delta t]
            particle.Rotation += particle.AngularVelocity * (World.Delta / 1000f); //theta = int(dtheta/dt)[delta t]
        }
    }
}
