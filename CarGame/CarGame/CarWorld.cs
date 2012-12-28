using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;
using CarGame.Entities.Systems;
using CarGame.Entities.Templates;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using CarGame.Entities.Templates.Car;

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
            this.SetEntityTemplate("Wheel", new WheelTemplate(this));
            this.SetEntityTemplate("Chassis", new ChassisTemplate(this));
            this.SetEntityGroupTemplate("Car", new CarGroupTemplate());
            this.SetEntityGroupTemplate("Bridge", new BridgeGroupTemplate());
            #endregion

            #region Entities
            Entity ground = this.CreateEntity("Ground");
            ground.Refresh();

            PlayerCar = this.CreateEntityGroup("Car", "PlayerCar",
                Content.Load<Texture2D>("car"),
                new Rectangle(0, 0, 120, 32),
                Content.Load<Texture2D>("wheel"),
                new Rectangle(0, 0, 23, 24))[0];

            //Bridge
            this.CreateEntityGroup("Bridge", "Bridge", ground.GetComponent<Body>());

            #endregion

            Camera.ResetCamera();
            Camera.MinRotation = -0.05f;
            Camera.MaxRotation = 0.05f;

            Camera.TrackingBody = PlayerCar.GetComponent<Body>();
            Camera.EnableRotationTracking = true;
            Camera.EnablePositionTracking = true;
#if DEBUG
            this._DebugRenderSystem.LoadContent(_SpriteBatch.GraphicsDevice, Content,
                 new KeyValuePair<string, object>("Camera", this.Camera),
                 new KeyValuePair<string, object>("Player", this.PlayerCar.GetComponent<Body>()));
#endif

            base.LoadContent(Content, args);
        }
        #endregion

        #region Fields
        public Entity PlayerCar;

        GroundRenderSystem _groundRenderSystem;
        public PlayerControlSystem PlayerControlSystem;
        #endregion
    }
}
