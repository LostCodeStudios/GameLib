using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.NewGameStates
{
    /// <summary>
    /// Manages a list of UI controls.
    /// </summary>
    public class ControlManager
    {
        List<Control> controls = new List<Control>();

        /// <summary>
        /// The index of the gamepad controlling this manager.
        /// </summary>
        public PlayerIndex ControllingIndex;

        /// <summary>
        /// Whether the ControlManager should update or accept input.
        /// </summary>
        public bool Enabled
        {
            get { return enabled && transitioned; }
            set { enabled = value; }
        }

        bool enabled;
        bool transitioned;

        /// <summary>
        /// Whether the ControlManager needs to draw.
        /// </summary>
        public bool Visible = false;

        int selectedIndex;

        bool hasSelectableControls = false;

        /// <summary>
        /// Adds a control to the manager's list.
        /// </summary>
        /// <param name="control"></param>
        public void Add(Control control)
        {
            controls.Add(control);

            if (control.TabStop)
                hasSelectableControls = true;
        }

        /// <summary>
        /// Tells all controls to transition on. Input will be enabled when all have transitioned on.
        /// </summary>
        public void TransitionOn()
        {
            foreach (Control control in controls)
            {
                control.TransitionOn();
            }

            Visible = true;
        }

        /// <summary>
        /// Tells all controls to transition off. Disables input.
        /// </summary>
        public void TransitionOff()
        {
            foreach (Control control in controls)
            {
                control.TransitionOff();
            }

            Enabled = false;
        }

        /// <summary>
        /// Handles input and updates every control in the manager's list.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Once all controls are done transitioning, input should be enabled.
            if (Visible && !transitioned)
            {
                transitioned = true;

                foreach (Control control in controls)
                {
                    if (control.Transitioning)
                    {
                        transitioned = false;
                    }
                }
            }

            GamePadState padState = GamePad.GetState(ControllingIndex);

            HandleInput(padState);

            foreach (Control control in controls)
            {
                control.Update(gameTime, this);
            }
        }

        /// <summary>
        /// Handles input. Can be overriden to create a different control scheme.
        /// </summary>
        /// <param name="padState"></param>
        public virtual void HandleInput(GamePadState padState)
        {
            if (!Enabled)
                return;

            if (hasSelectableControls)
            {
                int oldIndex = selectedIndex; //Keep the old selected index to check if it has changed

                if (padState.IsButtonDown(Buttons.LeftThumbstickUp | Buttons.DPadUp))
                {
                    do
                    {
                        --selectedIndex;

                        if (selectedIndex < 0)
                            selectedIndex = controls.Count() - 1;
                    } while (!controls[selectedIndex].TabStop); //Skip controls that cannot be selected.
                }

                if (padState.IsButtonDown(Buttons.LeftThumbstickDown | Buttons.DPadDown))
                {
                    ++selectedIndex;

                    if (selectedIndex >= controls.Count())
                        selectedIndex = 0;
                }

                if (oldIndex != selectedIndex)
                {
                    controls[oldIndex].Selected = false;
                    controls[selectedIndex].Selected = true;
                }
            }
        }

        /// <summary>
        /// Draws every control in the manager's list.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            foreach (Control control in controls)
            {
                control.Draw(spriteBatch);
            }
        }
    }
}
