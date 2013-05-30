using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework.Input;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;

namespace CarGame.Entities.Systems
{
    class GroundRenderSystem : TagSystem
    {
        ComponentMapper<Body> physicalMapper;
        Camera camera;
        GraphicsDevice graphicsDevice;
        LineBatch LineBatch;

        public GroundRenderSystem(Camera camera, GraphicsDevice graphics)
            : base("Ground")
        {
            this.camera = camera;
            this.graphicsDevice = graphics;
            this.LineBatch = new LineBatch(graphicsDevice);
        }

        public override void Initialize()
        {
            physicalMapper = new ComponentMapper<Body>(world);
        }

        public override void Process(Entity e)
        {
            Body ground = physicalMapper.Get(e);

            this.LineBatch.Begin(camera.SimProjection, camera.SimView);
            // draw ground
            for (int i = 0; i < ground.FixtureList.Count; ++i)
            {
                this.LineBatch.DrawLineShape(ground.FixtureList[i].Shape, Color.Black);
            }
            this.LineBatch.End();

        }
    }
}
