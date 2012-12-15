using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameLib.View.Helpers
{
    /// <summary>
    /// A helper class for geometric drawing
    /// </summary>
    public static class Geometry
    {
        #region Methods

        #region Drawing

        /// <summary>
        /// Draws points to the screen
        /// </summary>
        /// <typeparam name="T">Vertex type</typeparam>
        /// <param name="graphics"></param>
        /// <param name="vertices">The vertices to draw</param>
        public static void DrawPoints<T>(GraphicsDevice graphics, params T[] vertices) where T: struct, IVertexType
        {
            graphics.DrawUserIndexedPrimitives<T>(PrimitiveType.TriangleList, vertices, 0, vertices.Length, CreatePointIndices<T>(vertices), 0, vertices.Length);
        }

        public static void DrawLine<T>(GraphicsDevice graphics, T start, T end) where T : struct, IVertexType
        {
            
        }

        /// <summary>
        /// Draws a hull through lines (wireframe)
        /// </summary>
        /// <typeparam name="T">Vertex type</typeparam>
        /// <param name="graphics"></param>
        /// <param name="vertices">Vertices</param>
        public static void DrawHull<T>(GraphicsDevice graphics, params T[] vertices) where T : struct, IVertexType
        {
            graphics.DrawUserIndexedPrimitives<T>(PrimitiveType.LineStrip, vertices, 0, vertices.Length, CreateHullIndices<T>(vertices), 0, vertices.Length);
        }


        #endregion

        #region Helpers

        #region Indices
        /// <summary>
        /// Creates point indices for triangles.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vertices"></param>
        /// <returns></returns>
        private static short[] CreatePointIndices<T>(params T[] vertices) where T : struct, IVertexType
        {
            short[] indices = new short[vertices.Length*3];
            for (int i = 0; i < indices.Length; i+=3)
            {
                indices[i] = (short)i;
                indices[i+1] = (short)i;
                indices[i+2] = (short)i;


            }
            return indices;
        }

        /// <summary>
        /// Creates the indices of a hull.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vertices"></param>
        /// <returns></returns>
        private static short[] CreateHullIndices<T>(params T[] vertices) where T : struct, IVertexType
        {
            short[] indices = new short[vertices.Length+1];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = (short)i;
            }
            indices[vertices.Length] = 0;
            return indices;
        }
        #endregion

        #region Vertex

        /// <summary>
        /// Creates vertices based off of a lsit of vector2s
        /// </summary>
        /// <param name="points">The points</param>
        /// <returns></returns>
        public static VertexPositionColor[] CreatePositionColorVertices(Color color, Vector2 position, params Vector2[] points)
        {
            VertexPositionColor[] VPC = new VertexPositionColor[points.Length]; //Creates the list of primitives to be returned.\

            for (int i = 0; i < points.Length; i++)
            {
                VPC[i] = new VertexPositionColor(new Vector3(points[i] + position, 0), color);

            }
            return VPC;
        }


        #endregion

        #endregion


        #endregion

    }
}
