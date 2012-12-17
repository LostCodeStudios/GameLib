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
using GameLib.View;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using GameLib.Helpers;
using FarseerPhysics.Collision;
using FarseerPhysics;
using GameLib.Helpers;


namespace GameLibTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        Rectangle source1 = new Rectangle(0,0,26,24);
        Vector2 position1 = new Vector2(50);

        Rectangle source2 = new Rectangle(0, 20, 500, 500);
        Vector2 position2 = new Vector2(100);

        World Stage = new World(new Vector2(0, 1f));

        DebugViewXNA _debugView;

        Body test;
        





        Texture2D vertexMap;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            Screen.Initialize(graphics.GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64);
            _debugView = new DebugViewXNA(Stage);
            _debugView.LoadContent(GraphicsDevice, Content);

            vertexMap = Content.Load<Texture2D>("spriteSheet");
            
            test = new Body(Stage);
            test.BodyType = BodyType.Dynamic;
            test.Position = ConvertUnits.ToSimUnits(position1);
            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(30), ConvertUnits.ToSimUnits(30), 1, Vector2.Zero, test);

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
            
            if (last != Keyboard.GetState())
            {
            }
            last = Keyboard.GetState();

            //movement
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                position1 -= new Vector2(0, 5);
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                position1 -= new Vector2(5, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                position1 += new Vector2(0, 5);
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                position1 += new Vector2(5, 0);


            //if (Keyboard.GetState().IsKeyDown(Keys.E))
            //    polygon1.Origin += new Vector2(10, 0);
            //if (Keyboard.GetState().IsKeyDown(Keys.Q))
            //    polygon1.Origin -= new Vector2(10, 0);

            test.Position = ConvertUnits.ToSimUnits(position1);
            
            Stage.Step(0.03333f);
            
            
            base.Update(gameTime);
        }
        KeyboardState last = new KeyboardState();

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            

            spriteBatch.Begin();
            //Draw the sprites
            spriteBatch.Draw(vertexMap, ConvertUnits.ToDisplayUnits(test.Position), source1, Color.White, 0f, new Vector2(15), 1f, SpriteEffects.None, 0);
            
            spriteBatch.Draw(vertexMap, position2, source2, Color.White);

            spriteBatch.End();

            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, GraphicsDevice.Viewport.Width / ConvertUnits.ToDisplayUnits(1f),
                                                  GraphicsDevice.Viewport.Height / ConvertUnits.ToDisplayUnits(1f), 0f, 0f,
                                                  1f);
            Matrix view = Matrix.CreateTranslation(new Vector3((Vector2.Zero / ConvertUnits.ToDisplayUnits(1f)) - (new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f) / ConvertUnits.ToDisplayUnits(1f)), 0f)) * Matrix.CreateTranslation(new Vector3((new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f) / ConvertUnits.ToDisplayUnits(1f)), 0f));
            // draw the debug view
            _debugView.RenderDebugData(ref projection, ref view);

            base.Draw(gameTime);
        }
    }
}

