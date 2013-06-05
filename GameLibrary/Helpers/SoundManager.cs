using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Helpers
{
    public static class SoundManager
    {
        #region Settings

        private static float volume;

        public static float Volume
        {
            get { return volume; }
            set
            {
                volume = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        private static float _Pitch;
        public static float Pitch
        {
            get { return _Pitch; }
            set
            {
                _Pitch = MathHelper.Clamp(value, -1f, 1f);
            }
        }

        public static bool Rumble = true;

        #endregion

        #region Rumble

        static float[] rumbleTime = new float[4];

        public static void UpdateRumble(GameTime gameTime)
        {
            for (int i = 0; i < 4; ++i)
            {
                if (rumbleTime[i] > 0)
                {
                    rumbleTime[i] -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (rumbleTime[i] <= 0)
                    {
                        SetVibration((PlayerIndex)i, 0, 0);
                    }
                }
            }
        }

        public static void SetVibration(PlayerIndex index, float leftMotor, float rightMotor, float time)
        {
            if (!Rumble)
                return;

            GamePad.SetVibration(index, leftMotor, rightMotor);

            for (int i = 0; i < 4; ++i)
            {
                rumbleTime[i] = time;
            }
        }

        public static void SetVibration(PlayerIndex index, float amount, float time)
        {
            SetVibration(index, amount, amount, time);
        }

        public static void SetVibration(float leftMotor, float rightMotor, float time)
        {
            for (int i = 0; i < 4; ++i)
            {
                SetVibration((PlayerIndex)i, leftMotor, rightMotor, time);
            }
        }

        public static void SetVibration(float amount, float time)
        {
            SetVibration(amount, amount, time);
        }

        #endregion

        #region Playback

        public static void Play(string soundKey)
        {
            Play(soundKey, 1);
        }

        public static void Play(string soundKey, float volume)
        {
            if (sounds.ContainsKey(soundKey) && Volume > 0f)
                sounds[soundKey].Play(Volume * volume, Pitch, 0f);
        }

        #endregion

        #region Collection

        private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        public static void Add(string soundKey, SoundEffect sound)
        {
            sounds.Add(soundKey, sound);
        }

        public static void Remove(string soundKey)
        {
            sounds.Remove(soundKey);
        }

        #endregion
    }
}