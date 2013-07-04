#if XBOX

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;

namespace GameLibrary.Helpers
{
    /// <summary>
    /// Helper for all tasks related to file storage on Xbox.
    /// </summary>
    public static class StorageHelper
    {
        /// <summary>
        /// The name of the storage container that will be used in your game. Remember to set this before initializing StorageHelper.
        /// </summary>
        public static string ContainerName;

        static StorageDevice storageDevice;
        static StorageContainer storageContainer;

        static IAsyncResult result;
        
        static bool needResult = true;

        /// <summary>
        /// Initializes the StorageHelper, instantiating the StorageDevice by showing the device selection screen using the given PlayerIndex.
        /// </summary>
        /// <param name="controllingIndex"></param>
        /// <returns></returns>
        public static bool Initialize(PlayerIndex controllingIndex)
        {
            try
            {
                if (!Guide.IsVisible && needResult)
                {
                    result = StorageDevice.BeginShowSelector(controllingIndex, null, null);
                    needResult = false;
                }

                if (result != null && result.IsCompleted)
                {
                    StorageDevice device = StorageDevice.EndShowSelector(result);
                    if (device != null && device.IsConnected)
                    {
                        storageDevice = device;

                        result = null;
                        needResult = true;

                        return true;
                    }
                    else
                    {
                        result = null;
                        needResult = true;
                    }
                }

                return false;
            }
            catch (GuideAlreadyVisibleException)
            {
                return false;
            }
        }

        /// <summary>
        /// Opens the StorageContainer.
        /// </summary>
        /// <returns>Whether this operation was successful.</returns>
        public static bool OpenContainer()
        {
            try
            {
                IAsyncResult r = storageDevice.BeginOpenContainer(ContainerName, null, null);
                storageContainer = storageDevice.EndOpenContainer(r);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Disposes the StorageContainer, saving all file changes.
        /// </summary>
        public static void DisposeContainer()
        {
            try
            {
                storageContainer.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Disposes the StorageContainer and re-opens it in order to save all file changes.
        /// </summary>
        public static void SaveChanges()
        {
            DisposeContainer();
            OpenContainer();
        }

        /// <summary>
        /// Performs a test file operation in order to validate the current state of the storage container.
        /// </summary>
        /// <returns></returns>
        public static bool CheckStorage()
        {
            try
            {
                SaveChanges();

                if (!storageDevice.IsConnected)
                    return false;

                if (storageContainer == null || storageContainer.IsDisposed)
                    return false;

                storageContainer.OpenFile("!STORAGE_TEST!.txt", FileMode.OpenOrCreate).Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns whether the given file exists.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool FileExists(string filename)
        {
            return storageContainer.FileExists(filename);
        }

        /// <summary>
        /// Opens a file using the StorageContainer.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fileMode"></param>
        /// <returns></returns>
        public static Stream OpenFile(string filename, FileMode fileMode)
        {
            return storageContainer.OpenFile(filename, fileMode);
        }
    }
}

#endif