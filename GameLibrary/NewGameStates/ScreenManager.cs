using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.NewGameStates
{
    /// <summary>
    /// Manages all GameScreens in the game.
    /// </summary>
    public class ScreenManager
    {
        List<GameScreen> screens = new List<GameScreen>();
        Game game;

        /// <summary>
        /// Constructs a ScreenManager with a reference to the Game, for shutdown.
        /// </summary>
        /// <param name="game"></param>
        public ScreenManager(Game game)
        {
            this.game = game;
        }

        public void AddScreen(GameScreen screen)
        {


            screens.Add(screen);
        }

        #region Update & Draw

        /// <summary>
        /// Updates the top GameScreen.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            GameScreen topScreen = screens[screens.Count() - 1];
            
            TransitionEvent tEvent = topScreen.Update(gameTime);
            if (tEvent == TransitionEvent.BeginTransitionOff) //Screen started transitioning off.
            {
                if (screens.Count() > 0)
                {
                    GameScreen nextScreen = screens[screens.Count() - 2];
                    nextScreen.TransitionOn(); //Next screen of the stack needs to transition.
                }
                else //If there's no more screens, it's time to quit
                {
                    game.Exit();
                }
            }
            if (tEvent == TransitionEvent.EndTransitionOff)
            {
                screens.Remove(topScreen);
            }
        }

        /// <summary>
        /// Draws all GameScreens that are visible.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = screens.Count() - 1; i >= 0; --i) //Loop from the top of the "stack"
            {
                screens[i].Draw(spriteBatch);
                if (screens[i].FullScreen)
                    break; //Screen covers all below it.
            }
        }

        #endregion
    }
}
