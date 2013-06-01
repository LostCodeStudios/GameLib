

using GameLibrary.GameStates;
using GameLibrary.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using GameLibrary.Helpers;
namespace GameLib.GameStates.Screens
{
    public class TextScreen : GameScreen
    {
        string title;
        string[] lines;

        InputAction menuCancel;

        string cancelSound;

        public TextScreen(string title, params string[] lines)
        {
            this.title = title;
            this.lines = lines;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            menuCancel = new InputAction(
                new Buttons[] { Buttons.B, Buttons.Back },
                new Keys[] { Keys.Escape, Keys.Space },
                true);

            cancelSound = "MenuCancel";
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            PlayerIndex idx;

            if (menuCancel.Evaluate(input, null, out idx))
            {
                ExitScreen();
                CallExit();

                if (!string.IsNullOrEmpty(cancelSound))
                {
                    SoundManager.Play(cancelSound);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //Make the menu slide into place during transitions, using a power
            //curve to make things look more interesting (this makes the movement
            //slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            SpriteBatch spriteBatch = Manager.SpriteBatch;

            spriteBatch.Begin();

            Vector2 titlePosition = new Vector2(Manager.GraphicsDevice.Viewport.Width / 2, Manager.GraphicsDevice.Viewport.Height * 0.1736111111111111f);
            Vector2 titleOrigin = Manager.TitleFont.MeasureString(title) / 2;
            Color titleColor = new Color(100, 77, 45) * TransitionAlpha;
            float titleScale = 1f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(Manager.TitleFont, title, titlePosition, titleColor, 0,
                titleOrigin, titleScale, SpriteEffects.None, 0);

            Vector2 position = new Vector2(0f, ScreenHelper.Viewport.Height * 0.25f);

            for (int i = 0; i < lines.Length; i++)
            {
                //set the left margin
                position.X = Manager.GraphicsDevice.Viewport.Width / 2;
                position.X -= ((Manager.CreditsFont.MeasureString(lines[i]).X)) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                spriteBatch.DrawString(Manager.CreditsFont, lines[i], position, Color.White * TransitionAlpha);

                position.Y += Manager.Font.LineSpacing;
            }

            spriteBatch.End();
        }
    }
}