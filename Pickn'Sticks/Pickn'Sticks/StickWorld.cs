using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Input;
using Pickn_Sticks.Entities.Systems;
using Pickn_Sticks.Entities.Templates;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Dynamics.Contacts;
using GameLibrary.Helpers.Debug;

namespace Pickn_Sticks
{
    public class StickWorld : World
    {
        public StickWorld(Game game)
            : base(game)
        {
        }

        #region Initialization

        protected override void BuildSystems()
        {
            playerControlSystem = this.SystemManager.SetSystem(new PlayerControlSystem(2.235f), GameLibrary.Dependencies.Entities.ExecutionType.Update);

            base.BuildSystems();
        }

        protected override void BuildTemplates(Microsoft.Xna.Framework.Content.ContentManager Content, params object[] args)
        {
            this.SetEntityTemplate("Player", new PlayerTemplate(this));
            this.SetEntityTemplate("Stick", new StickTemplate(this));
            this.SetEntityGroupTemplate("Terrain", new TerrainGroupTemplate(SpriteBatch.GraphicsDevice));

            base.BuildTemplates(Content, args);
        }

        int STICKCOUNT = 0;

        protected override void BuildEntities(Microsoft.Xna.Framework.Content.ContentManager Content, params object[] args)
        {
            //Player
            Entity player = this.CreateEntity("Player", Content.Load<Texture2D>("player"), new Rectangle(15, 30, 50, 30));
            player.Refresh();


            player.GetComponent<Body>().OnCollision +=
               new GameLibrary.Dependencies.Physics.Dynamics.OnCollisionEventHandler(
                   delegate(Fixture a, Fixture b, Contact c)
                   {
                       if (b.Body.UserData != null && (b.Body.UserData as Entity).Group == "Sticks")
                       {
                           Console.WriteLine("\n["+ ++STICKCOUNT +"]Stick touched at" + (b.Body.UserData as Entity).GetComponent<Body>().Position.ToString());
                           Random r = new Random();
                           (b.Body.UserData as Entity).GetComponent<Body>().Position = ConvertUnits.ToSimUnits(new Vector2(r.Next(0, ScreenHelper.GraphicsDevice.Viewport.Width), r.Next(0, ScreenHelper.GraphicsDevice.Viewport.Height))
                               - new Vector2(ScreenHelper.GraphicsDevice.Viewport.Bounds.Center.X, ScreenHelper.GraphicsDevice.Viewport.Bounds.Center.Y) + new Vector2(15));
                           return false;
                       }
                       else
                           return true;
                   });
            //Test stick
            Entity stick = this.CreateEntity("Stick", Content.Load<Texture2D>("stick"), new Rectangle(4, 2, 28 - 4, 28 - 2));
            stick.Refresh();

            //Camera
            Camera.TrackingBody = player.GetComponent<Body>();
            Camera.EnableTracking = true;
            Camera.EnableRotationTracking = false;
            //Camera.MaxPosition = ConvertUnits.ToSimUnits(
            //    new Vector2(SpriteBatch.GraphicsDevice.Viewport.Bounds.Right, SpriteBatch.GraphicsDevice.Viewport.Bounds.Bottom));
            //Camera.MinPosition = ConvertUnits.ToSimUnits(
            //    new Vector2(SpriteBatch.GraphicsDevice.Viewport.Bounds.Left, SpriteBatch.GraphicsDevice.Viewport.Bounds.Top));
            Camera.Zoom = 1f;

            this.CreateEntityGroup("Terrain", "Terrain",
                Content.Load<Texture2D>("terrain"), new Rectangle(7, 5, 88 - 7, 88 - 5),
                Content.Load<Texture2D>("plants"), new Rectangle(352,0, 33,33));


#if DEBUG //Track player info
            this._DebugSystem.LoadContent(SpriteBatch.GraphicsDevice, Content,
                 new KeyValuePair<string, object>("Camera", this.Camera),
                 new KeyValuePair<string, object>("Player", player.GetComponent<Body>()));
#endif

            base.BuildEntities(Content, args);
        }

        #endregion

        #region Functioning Loop
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion

        #region Fields
        PlayerControlSystem playerControlSystem;
        #endregion

    }
}
