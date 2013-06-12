using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.NewGameStates
{
    public abstract class Control
    {
        Transition transition;

        public virtual void Update(GameTime gameTime)
        {

        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
