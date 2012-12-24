using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CarGame.Entities.Systems;
using GameLibrary.Helpers;
using GameLibrary.Entities;
using CarGame.Entities.Templates;
using CarGame.Entities.Components;

namespace CarGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private PhysicsSystem physicsSystem;
        private RenderSystem renderSystem;
        private DebugRenderSystem debugRenderSystem;
        private GroundRenderSystem groundRenderSysten;
        private PlayerControlSystem playerControlSystem;
        private Camera camera;

        private EntityWorld entityWorld;
        private Entity player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.Title = "CarGame EF test";
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            IsFixedTimeStep = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);
            ScreenHelper.Initialize(graphics.GraphicsDevice);

            camera = new Camera(graphics.GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            entityWorld = new EntityWorld(new Vector2(0, 10f));

            var systemManager = entityWorld.SystemManager;
            entityWorld.SetEntityTemplate("Ground", new GroundTemplate(entityWorld));
            entityWorld.SetEntityTemplate("Player", new PlayerTemplate(entityWorld));
            entityWorld.SetEntityTemplate("Bridge", new BridgeTemplate(entityWorld));
            physicsSystem = systemManager.SetSystem(new PhysicsSystem(),
                ExecutionType.Update);
           
#if DEBUG
            debugRenderSystem = systemManager.SetSystem(new DebugRenderSystem(camera), ExecutionType.Draw);
#else
            renderSystem = systemManager.SetSystem(new RenderSystem(graphics.GraphicsDevice, spriteBatch), ExecutionType.Draw);
#endif
            groundRenderSysten = systemManager.SetSystem(new GroundRenderSystem(camera, GraphicsDevice), ExecutionType.Draw);
            playerControlSystem = systemManager.SetSystem(new PlayerControlSystem(), ExecutionType.Update);

            systemManager.InitializeAll();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            Entity ground = entityWorld.CreateEntity("Ground");
            ground.Refresh();

            player = entityWorld.CreateEntity("Player",
                Content.Load<Texture2D>("car"),
                new Rectangle(0, 0, 120, 32),
                Content.Load<Texture2D>("wheel"),
                new Rectangle(0, 0, 23, 24));
            player.Refresh();

            Entity bridge = entityWorld.CreateEntity("Bridge", ground.GetComponent<Physical>("Ground"));
            bridge.Refresh();

            camera.ResetCamera();
            camera.MinRotation = -0.05f;
            camera.MaxRotation = 0.05f;

            camera.TrackingBody = player.GetComponent<Physical>("Chassis");
            camera.EnableRotationTracking = true;
            camera.EnablePositionTracking = true;
            
#if DEBUG
            debugRenderSystem.LoadContent(GraphicsDevice, Content);
#endif
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            entityWorld.LoopStart();
            // TODO: Add your update logic here
            entityWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            entityWorld.Delta = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            entityWorld.SystemManager.UpdateSynchronous(ExecutionType.Update);
            camera.Update(gameTime);


            if (Keyboard.GetState().IsKeyDown(Keys.D))
                playerControlSystem.acceleration = Math.Min(playerControlSystem.acceleration + (float)(2.0 * (float)gameTime.ElapsedGameTime.TotalSeconds), 1f);
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
                playerControlSystem.acceleration = Math.Min(playerControlSystem.acceleration + (float)(2.0 *(float)gameTime.ElapsedGameTime.TotalSeconds), -1f);
            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                playerControlSystem.acceleration = 0;
            else
                playerControlSystem.acceleration -= Math.Sign(playerControlSystem.acceleration) * (float)(2.0 * (float)gameTime.ElapsedGameTime.TotalSeconds);

            playerControlSystem.UpdateInput(Keyboard.GetState());
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(0, null, null, null, null, null, camera.View);
            entityWorld.SystemManager.UpdateSynchronous(ExecutionType.Draw);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
