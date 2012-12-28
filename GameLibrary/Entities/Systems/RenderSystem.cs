using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;

namespace GameLibrary.Entities.Systems
{
    public class RenderSystem : EntityProcessingSystem
    {
        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<Sprite> spriteMapper;

        private SpriteBatch spriteBatch;

        public RenderSystem(SpriteBatch spritebatch):
            base(typeof(Sprite), typeof(Transform))
        {
            this.spriteBatch = spritebatch;
        }

        public override void Initialize()
        {
            spriteMapper = new ComponentMapper<Sprite>(world);
            transformMapper = new ComponentMapper<Transform>(world);
        }

        /// <summary>
        /// Renders all entities with a sprite and a transform to the screen.
        /// </summary>
        /// <param name="e"></param>
        public override void Process(Entity e)
        {
            //Get sprite data and transform
            Transform transform  = transformMapper.Get(e);
            Sprite  sprite = spriteMapper.Get(e);

            //Draw to sprite batch
            spriteBatch.Draw(sprite.SpriteSheet, ConvertUnits.ToDisplayUnits(transform.Position),
                sprite.Source,
                sprite.Color,
                transform.Rotation,
                sprite.Origin,
                sprite.Scale,
                SpriteEffects.None, 0f);
        }
    }
}
