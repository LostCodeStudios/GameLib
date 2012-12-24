using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using GameLibrary.Entities;
using CarGame.Entities.Systems;
using CarGame.Entities.Templates;
using GameLibrary.Entities.Components;

namespace CarGame
{
    /// <summary>
    /// The car world for the car.
    /// </summary>
    class CarWorld : World
    {
        public CarWorld(Camera camera, SpriteBatch spriteBatch)
            : base(camera, spriteBatch, new Vector2(0f, 10f))
        {
        }

        #region Functioning Loop
        public override void Initialize()
        {
            #region Systems
            _groundRenderSystem = this.SystemManager.SetSystem(new GroundRenderSystem(Camera, _SpriteBatch.GraphicsDevice), ExecutionType.Draw);
            PlayerControlSystem = this.SystemManager.SetSystem(new PlayerControlSystem(), ExecutionType.Update);

            #endregion

            base.Initialize();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, params object[] args)
        {
            #region Templates
            this.SetEntityTemplate("Ground", new GroundTemplate(this));
            this.SetEntityTemplate("Player", new PlayerTemplate(this));
            this.SetEntityTemplate("Bridge", new BridgeTemplate(this));
            #endregion

            #region Entities
            Entity ground = this.CreateEntity("Ground");
            ground.Refresh();

            Player = this.CreateEntity("Player",
                Content.Load<Texture2D>("car"),
                new Rectangle(0, 0, 120, 32),
                Content.Load<Texture2D>("wheel"),
                new Rectangle(0, 0, 23, 24));
            Player.Refresh();

            Entity bridge = this.CreateEntity("Bridge", ground.GetComponent<Physical>("Ground"));
            bridge.Refresh();

            #endregion

            Camera.ResetCamera();
            Camera.MinRotation = -0.05f;
            Camera.MaxRotation = 0.05f;

            Camera.TrackingBody = Player.GetComponent<Physical>("Chassis");
            Camera.EnableRotationTracking = true;
            Camera.EnablePositionTracking = true;
#if DEBUG
            this._DebugRenderSystem.LoadContent(_SpriteBatch.GraphicsDevice, Content,
                 new KeyValuePair<string, object>("Camera", this.Camera),
                 new KeyValuePair<string, object>("Player", this.Player.GetComponent<Physical>("Chassis")));
#endif

            base.LoadContent(Content, args);
        }
        #endregion

        #region Fields
        public Entity Player;

        GroundRenderSystem _groundRenderSystem;
        public PlayerControlSystem PlayerControlSystem;
        #endregion
    }
}
