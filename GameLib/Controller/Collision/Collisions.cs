using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameLib.View.Helpers;
using Microsoft.Xna.Framework.Graphics;
using GameLib.View;

namespace GameLib.Controller.Collision
{
    /// <summary>
    /// Helper class for collision detection using seperating axis theorem.
    /// </summary>
    public static class Collisions
    {

        #region Collision Detection Helpers
        /// <summary>
        /// Checks if the three points are making a clockwise/counterclockwise/colinear
        /// </summary>
        /// <param name="vertex1">First vertex</param>
        /// <param name="vertex2">Second vertex</param>
        /// <param name="vertex3">Third vertex</param>
        /// <returns>An integer which is positive for counter-clockwise, negative for clockwise, and 0 for collinear.</returns>
        public static float Cross(Vector2 vertex1, Vector2 vertex2, Vector2 vertex3)
        {
            return (vertex2.X - vertex1.X) * (vertex3.Y - vertex1.Y) - (vertex2.Y - vertex1.Y) * (vertex3.X - vertex1.X);
        }

        /// <summary>
        /// Projects the specified vector2 points on an axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="points"></param>
        public static void Project(Vector2 axis, out float min, out float max, params Vector2[] points)
        {
            //Use a dot product to project a polygon onto a line
            float dot = Vector2.Dot(axis, points[0]);
            min = dot;
            max = dot;



            foreach (Vector2 vertex in points)
            {
              
                dot = Vector2.Dot(vertex, axis);
                if (dot < min)
                    min = dot;
                else if (dot > max)
                    max = dot;
            }
        }

        /// <summary>
        /// Gets distance between two min/max dot products.
        /// </summary>
        /// <param name="minA"></param>
        /// <param name="maxA"></param>
        /// <param name="minB"></param>
        /// <param name="maxB"></param>
        /// <returns></returns>
        public static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }
        #endregion
    }
}
