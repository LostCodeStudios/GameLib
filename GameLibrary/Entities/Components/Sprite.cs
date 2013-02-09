using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components.Physics;

namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// A sprite component
    /// </summary>
    public struct Sprite : Component
    {
        public Sprite(SpriteSheet spriteSheet, string spriteKey, Vector2 origin, float scale, Color color, float layer, TimeSpan frameTime)
        {
            this.Source = spriteSheet.Animations[spriteKey];
            this.SpriteSheet = spriteSheet;
            this.Origin = origin;
            this.Scale = scale;
            this.Color = color;
            this.Layer = layer;
            this.index = 0;
            this.frameTime = frameTime;
            this.elapsed = TimeSpan.Zero;
        }

        public Sprite(SpriteSheet spriteSheet, string spriteKey, Body body, float scale, Color color, float layer, TimeSpan frameTime) :
            this(spriteSheet, spriteKey, AssetCreator.CalculateOrigin(body) / scale, scale, color, layer, frameTime)
        {
        }

        public Sprite(SpriteSheet spriteSheet, string spriteKey) : this(
            spriteSheet, 
            spriteKey, 
            new Vector2(
                spriteSheet[spriteKey][0].Width/2f, spriteSheet[spriteKey][0].Height/2f),
            1,
            Color.White,
            0f,
            TimeSpan.Zero)
        {
        }

        public float Layer;
        public Color Color;
        public float Scale;
        public SpriteSheet SpriteSheet;
        public Vector2 Origin;
        public Rectangle[] Source;
        
        int index;
        public int FrameIndex
        {
            get { return index; }
            set
            {
                if (value < 0)
                    index = 0;

                else
                    index = value % (Source.Count() - 1);
            }
        }

        TimeSpan frameTime;
        public double FrameDelay
        {
            get { return frameTime.TotalSeconds; }
            set
            {
                frameTime = TimeSpan.FromSeconds(Math.Max(0, value));
            }
        }
        TimeSpan elapsed;
        public TimeSpan Elapsed
        {
            get { return elapsed; }
            set { elapsed = value; }
        }

        public Rectangle CurrentRectangle
        {
            get { return Source[FrameIndex]; }
        }
    }
}
