using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.NewGameStates.Controls
{
    public class Label : Control
    {
        public Label(string text, Vector2 position, double transitionOnTime, double transitionOffTime, ScreenSide transitionOnSide, ScreenSide transitionOffSide)
            : base(transitionOnTime, transitionOffTime, transitionOnSide, transitionOffSide, null, position)
        {
            this.text = new TextComponent(text, position);
        }


        public TextComponent Text
        {
            get { return text; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            text.Alpha = transition.TransitionFraction;
            text.Position = DrawPosition;
            text.Draw(spriteBatch);
        }
    }
}
