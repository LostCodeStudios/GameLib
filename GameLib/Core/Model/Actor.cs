using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameLib.Core.Controller;
using FarseerPhysics.Dynamics;
using GameLib.Core.View;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using GameLib.Helpers;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;

namespace GameLib.Core.Model
{
    public class Actor : Dictionary<string, Actor>
    {
        #region Constructors/Deconstructors
        /// <summary>
        /// Initializes the actor with an animator, a sprite, and a body.
        /// </summary>
        /// <param name="stage">The stage on which the actor is created.</param>
        /// <param name="animator">The animator to initialize</param>
        /// <param name="body">The body to initialize</param>
        /// <param name="sprite">The sprite itself.</param>
        public Actor(Stage stage, Animator animator, Body body, Sprite sprite) : base()
        {
            this.Animator = animator;
            body.UserData = this;
            this.Body = body;
            this.Sprite = sprite;
            this._Stage = stage;

            //Collision detection events
            
        }

        /// <summary>
        /// Creates a sprite WITHOUT animation.
        /// </summary>
        /// <param name="stage">The stage on which the actor is created.</param>
        /// <param name="source">The source on the sprite sheet on which to operate</param>
        /// <param name="body">The physics body of the sprite.</param>
        /// <param name="sprite">The sprite itself.</param>
        public Actor(Stage stage, Rectangle source, Body body, Sprite sprite)
            : this(stage, new Animator(Animation.Static, source), body, sprite)
        {
        }

        /// <summary>
        /// Creates a sprite FROM A TEXTURE.
        /// </summary>
        /// <param name="stage">The stage on which the actor is created.</param>
        /// <param name="animator">The animator to initialize</param>
        /// <param name="position">The position at which to place the actor.</param>
        /// <param name="sprite">The sprite itself.</param>
        public Actor(Stage stage, Animator animator, Vector2 position, Sprite sprite)
            : this(stage, animator, Actor.TextureToBody(stage.World, sprite.SpriteSheet, animator.Frames[0]),
            sprite)
        {
            this.Body.Position = position;
        }


        /// <summary>
        /// Deconstructor
        /// </summary>
        ~Actor()
        {
            //Remove the body and its parts
            _Stage.World.RemoveBody(this.Body);
        }

        #endregion

        #region Functioning Loop
        /// <summary>
        /// Updates the the actor and all sub-actors.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            this.Animator.Update(gameTime); //Animate the actor
            CallChildren(x => x.Update(gameTime)); //Call update on all children.
        }

        /// <summary>
        /// Draws the actor and all sub-actors.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch, ConvertUnits.ToDisplayUnits(Body.Position),Body.Rotation, Animator.CurrentFrame);
            CallChildren(x => x.Draw(gameTime, spriteBatch)); //Call Draw on all children.
        }
        #endregion

        #region Properties


        #endregion

        #region Fields
        /// <summary>
        /// The aminator of the actor
        /// </summary>
        public Animator Animator;

        /// <summary>
        /// The body of the actor
        /// </summary>
        public Body Body;

        /// <summary>
        /// The sprite of the actor
        /// </summary>
        public Sprite Sprite;

        protected Stage _Stage;
        protected Actor _Parent;
        #endregion

        #region Methods
        #region TreeSystem
        protected delegate void Call(Actor subject);
        protected void CallChildren(Call action)
        {
            foreach (Actor a in this.Values)
            {
                action(a);
            }
        }

        /// <summary>
        /// Adds another body underneath this body with an anchor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(string key, Actor value)
        {
            //Create a joint combining the two actors
            value.Body.BodyType = this.Body.BodyType;
            value._Parent = this;
            JointFactory.CreateWeldJoint(_Stage.World, this.Body, value.Body, this.Body.GetLocalPoint(value.Body.Position));
            base.Add(key, value);
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Converts a Texture to a body. Code derived from demo.
        /// </summary>
        /// <param name="world">World in which the body will reside.</param>
        /// <param name="texture">The texture from which to find the polygon.</param>
        /// <param name="source">The area on the texture in which to decompose a polygon.</param>
        /// <returns></returns>
        public static Body TextureToBody(World world, Texture2D texture, Rectangle source)
        {
            //Hold all of the texture data
            uint[] data = new uint[source.Height * source.Width];

            texture.GetData(texture.LevelCount, source, data, 0, source.Height * source.Width);

            //1. Create vertices outlining texture.
            Vertices textureVertices = PolygonTools.CreatePolygon(data, source.Width, false);

            //2. To translate the vertices so the polygon is centered around the centroid.
            Vector2 centroid = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroid);

            //3. We simplify the vertices found in the texture.
            textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f);

            //4. Since it is a concave polygon, we need to partition it into several smaller convex polygons
            List<Vertices> list = BayazitDecomposer.ConvexPartition(textureVertices);

            float _scale = 1f;

            //5. scale the vertices from graphics space to sim space
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1)) * _scale;
            foreach (Vertices vertices in list)
            {
                vertices.Scale(ref vertScale);
            }

            //Create a compound body and return it.
            Body compound = BodyFactory.CreateCompoundPolygon(world, list, 1f, BodyType.Dynamic);
            compound.BodyType = BodyType.Dynamic;

            return compound;
        }
        #endregion
        #endregion
    }
}
