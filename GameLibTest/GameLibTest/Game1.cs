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
using GameLib.Controller.Collision.Bounderies;

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
        Polygon polygon1;
        Vector2 position1 = new Vector2(50);

        Rectangle source2 = new Rectangle(0, 20, 500, 500);
        Vector2 position2 = new Vector2(100);

        List<Polygon> polygons = new List<Polygon>();

        bool Colliding = false;
        float rot = 0;

        BasicEffect basicEffect;
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

            //create a shader so that primitives can be drawn
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
            (0, GraphicsDevice.Viewport.Width,     // left, right
            GraphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1); 

            vertexMap = Content.Load<Texture2D>("vertexMap");

            // TODO: use this.Content to load your game content here
            polygon1 = new Polygon(Vector2.Zero, vertexMap, Color.Magenta, source1, 10f);
            polygons.Add(new Polygon(Vector2.Zero, vertexMap, Color.Magenta, source2, 0f));
            polygons.Add(new Polygon(Vector2.Zero, vertexMap, Color.Magenta, source2, 40f));
            polygons.Add(new Polygon((Vector2.Zero), vertexMap, Color.Magenta, source2, 40f));
            polygons.Add(new Polygon(Vector2.Zero, vertexMap, Color.Magenta, source2, 40f));
            polygons.Add(new Polygon(Vector2.Zero, vertexMap, Color.Magenta, source2, 40f));




            polygons[0].Position = position2;
            polygons[1].Position = new Vector2(320);
            polygons[2].Position = new Vector2(420);
            polygons[3].Position = new Vector2(520);
            polygons[4].Position = new Vector2(620);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            polygon1.Position = position1;

            //if (Keyboard.GetState().IsKeyDown(Keys.E))
            //    polygon1.Origin += new Vector2(10, 0);
            //if (Keyboard.GetState().IsKeyDown(Keys.Q))
            //    polygon1.Origin -= new Vector2(10, 0);

           //  rot += (float)(Math.PI / 360);
            //polygon1.Rotation = rot;

            // TODO: Add your update logic here

            Colliding = false;
            for(int i = 0; i< polygons.Count(); i++)
            {
                for(int o = 0; o < polygons.Count(); o++){
                    if( o != i)
                        if(polygons[i].CheckCollision(Vector2.Zero, polygons[o]) && 
                            polygons[o].CheckCollision(Vector2.Zero, polygons[i])
                            ||
                            (polygons[i].CheckCollision(Vector2.Zero, polygon1) &&
                            polygon1.CheckCollision(Vector2.Zero, polygons[i])))
                            Colliding = true;
                }
            }
            

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

            basicEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Begin();
            
            //Draw the sprites
           // spriteBatch.Draw(vertexMap, position1, source1, Color.White, rot, Vector2.Zero,1f,SpriteEffects.None,0);
            spriteBatch.Draw(vertexMap, position2, source2, Color.White);

            polygon1.Draw(spriteBatch, Vector2.Zero);
            
            foreach(Polygon p in polygons)
                p.Draw(spriteBatch, Vector2.Zero);

            if(Colliding)
                spriteBatch.Draw(vertexMap, Vector2.Zero, source1, Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
