using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.GameStates.Screens
{
    /// <summary>
    /// Text screen that will be shown to the user when they attempt to open a GameScreen that requires storage, if they do not have access to it.
    /// </summary>
    public class StorageMessageScreen : TextScreen
    {
        public StorageMessageScreen()
            : base(
                "Storage Unavailable",

                "This feature is currently unavailable",
                "because you do not have access to file storage",
                "on this Xbox 360. In order to enable this feature,",
                "Make sure you are signed into an Xbox profile",
                "and have selected a valid Storage Device.")
        {
        }
    }
}
