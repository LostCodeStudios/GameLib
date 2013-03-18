using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Dependencies.Physics.Factories;

namespace Pickn_Sticks.Entities.Templates
{
    class TerrainGroupTemplate : IEntityGroupTemplate //TODO: MAKE A TILE ENGINE :P
    {
        Microsoft.Xna.Framework.Graphics.GraphicsDevice graphics;
        public TerrainGroupTemplate(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphics)
        {
            this.graphics = graphics;
        }

        public Entity[] BuildEntityGroup(EntityWorld world, params object[] args)
        {
            Rectangle tileSource = (Rectangle)args[1];
            int maxWidth = (graphics.Viewport.Width * 2 / tileSource.Width) + 1;
            int maxHeight = (graphics.Viewport.Height * 2 / tileSource.Height) + 1; 

            Entity[] tiles = new Entity[maxHeight * maxWidth];
            //Make all of the tiles
            for (int i = 0; i < tiles.Length; i++)
            {
                int height = i / maxWidth;
                int width = i - height * maxWidth;


                tiles[i] = world.CreateEntity();
                tiles[i].AddComponent<Sprite>(new Sprite(args[0] as Texture2D, tileSource, Vector2.Zero, 1f, Color.White,0.5f));
                tiles[i].AddComponent<ITransform>(new Transform(
                    ConvertUnits.ToSimUnits(new Vector2(
                        width * tileSource.Width - maxWidth * tileSource.Width/2, //Offset width
                        height * tileSource.Height - maxHeight * tileSource.Height / 2)), //Offset height
                    0f));
                tiles[i].Refresh();
            }

            Rectangle treeSource = (Rectangle)args[3];
            List<Entity> treeList = new List<Entity>();
            

            for(int y = 0; y <= graphics.Viewport.Height / treeSource.Height + 1; y++)
                for (int x = 0; ((y == 0 || y == graphics.Viewport.Height / treeSource.Height + 1) ? x < graphics.Viewport.Width / treeSource.Width + 2 : x <1); x++)
                {
                    Entity e = world.CreateEntity();
                    //Body
                    Body treeBody = e.AddComponent<Body>(new Body(world, e));
                    treeBody.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Static;
                    FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(treeSource.Width / 2), 1f, treeBody);
                    
                    treeBody.Position = ConvertUnits.ToSimUnits(new Vector2(x * treeSource.Width, y * treeSource.Height)
                        - new Vector2(graphics.Viewport.Bounds.Center.X, graphics.Viewport.Bounds.Center.Y) + new Vector2(15));
                   
                    //sprite
                    e.AddComponent<Sprite>(new Sprite(args[2] as Texture2D, treeSource, treeBody, 1f, Color.White, 0.2f));
                    e.Refresh();
                    treeList.Add(e);
                }

            Random r = new Random();
            for (int i = 0; i < 50; i++)
            {
                Entity e = world.CreateEntity();
                //Body
                Body treeBody = e.AddComponent<Body>(new Body(world, e));
                treeBody.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Static;
                FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(treeSource.Width / 2), 1f, treeBody);
                treeBody.Position =
                    ConvertUnits.ToSimUnits(new Vector2(r.Next(0, graphics.Viewport.Width), r.Next(0, graphics.Viewport.Height))
                    - new Vector2(graphics.Viewport.Bounds.Center.X, graphics.Viewport.Bounds.Center.Y) + new Vector2(15));

                //sprite
                e.AddComponent<Sprite>(new Sprite(args[2] as Texture2D, treeSource, treeBody, 1f, Color.White, 0.2f));
                e.Refresh();
                treeList.Add(e);

            }

            Entity[] Terrain = new Entity[treeList.Count + tiles.Length];
            tiles.CopyTo(Terrain, 0);
            treeList.CopyTo(Terrain, tiles.Length);
            return Terrain;
        }
    }
}
