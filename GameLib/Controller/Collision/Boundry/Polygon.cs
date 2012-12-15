using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLib.View;
using System.Collections;
using GameLib.View.Helpers;

namespace GameLib.Controller.Collision.Bounderies
{
    /// <summary>
    /// Polygon class. Stores points and serves as a helper (useful for SAT and things).
    /// All polygons are created concavely.
    /// </summary>
    public class Polygon : IComparer<Vector2>, IBoundry
    {

        #region Constructor
        /// <summary>
        /// Creates an empty polygon with an origin.
        /// </summary>
        /// <param name="origin">Origin</param>
        public Polygon(Vector2 origin){
            _Origin = origin;
            _Edges = new List<Vector2>();
            _Vertices = new List<Vector2>();
        }

        /// <summary>
        /// Constructs the polygon based on specific points.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="vertice"></param>
        public Polygon(Vector2 origin, params Vector2[] vertices)
            : this(origin)
        {
            BuildVertices(4, vertices);
        }

        /// <summary>
        /// Creates a polygon based off of a verticy map.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="verticieMap"></param>
        /// <param name="source">Rectangle on the verticieMap to grab the points from</param>
        public Polygon(Vector2 origin, Texture2D vertexMap, Color verticeColor, Rectangle verticeSource, float VertexFrequency) : this(origin)
        {
            List<Vector2> vertices = new List<Vector2>(); //List storing found vertices.

            Color[,] colorMap = Screen.TextureTo2DArray(vertexMap);
            //Loop through coordinates in source on verticeMap
            for (int x = verticeSource.X; x < vertexMap.Width && x < verticeSource.Width + verticeSource.X; x++)
                for (int y = verticeSource.Y; y < vertexMap.Height && y < verticeSource.Height + verticeSource.Y; y++)
                    if (colorMap[x, y] != Color.Transparent) //If a vertice point has been found,
                        vertices.Add(new Vector2( //Add that point (relative to its origin) to the vertice lsit
                            (x - (verticeSource.X + origin.X)),
                            (y - (verticeSource.Y + origin.Y))
                            ));

            BuildVertices(VertexFrequency, vertices.ToArray());
        }
        #endregion

        #region Functioning Loop

        /// <summary>
        /// Draws the polygon to the spritebatch
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to be drawn on</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 Position)
        {
            Geometry.DrawHull<VertexPositionColor>(spriteBatch.GraphicsDevice, Geometry.CreatePositionColorVertices(Color.Red, Position,  _Vertices.ToArray()));
        }
        
        #endregion

        #region Methods

        #region Vertex/Edge
        /// <summary>
        /// Compares two vectors angles in relation to the extreme point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Vector2 x, Vector2 y)
        {
            //Sort
            float crossproduct = Collisions.Cross(ExtremeVertex, x, y);

           
            if (crossproduct < 0) //if angle is larger
                return 1;
            else if (crossproduct == 0) //if angle is the same
                return Vector2.DistanceSquared(x, ExtremeVertex) < Vector2.DistanceSquared(y, ExtremeVertex) ? -1 : 1;
            else //if angle is smaller
                return -1;
            
            
        }

        /// <summary>
        /// Updates the vertices in the polygon by finding the minimum convex hull.
        /// </summary>
        /// <param name="vertices">The vertices with which to find the minimum convex hull.</param>
        public void BuildVertices(float frequency, params Vector2[] vertices)
        {
            
            if (vertices.Count() > 2)
            {
                #region Graham Scan
                //Update the verts list and sort by angle
                _Vertices = vertices.ToList();
                
                //sort for lowest point
                for (int i = 1; i < _Vertices.Count(); i++)
                {
                    if (_Vertices[i].Y < _Vertices[0].Y || (_Vertices[i].Y == _Vertices[0].Y && _Vertices[i].X < _Vertices[0].X))
                    { //Swap
                        Vector2 tmp = _Vertices[0];
                        _Vertices[0] = _Vertices[i];
                        _Vertices[i] = tmp;
                    }
                }

                _ExtremeVertex = _Vertices[0];
                _Vertices.Sort(this.Compare);

                //Graham SCAN O(n log n NIQQA)
                List<Vector2> minimumConvexHull = new List<Vector2>();

                //initialize stack
                minimumConvexHull.Add(_Vertices[0]); //point 1
                minimumConvexHull.Add(_Vertices[1]); //point 2
                minimumConvexHull.Add(_Vertices[2]); //point 3

                for (int i = 3; i < _Vertices.Count(); ++i)
                {
                    while (minimumConvexHull.Count() >= 2
                        && Collisions.Cross(
                            minimumConvexHull[minimumConvexHull.Count() - 2],
                            minimumConvexHull[minimumConvexHull.Count() - 1],
                            _Vertices[i]) <= 0
                            && i < _Vertices.Count()) //While concave
                    {
                        minimumConvexHull.RemoveAt(minimumConvexHull.Count() - 1);
                    }
                    if (Vector2.Distance(minimumConvexHull[minimumConvexHull.Count() - 1], _Vertices[i]) > frequency) //Makes point only add every so often.
                        minimumConvexHull.Add(_Vertices[i]);

                }
                #endregion
                _Vertices = minimumConvexHull;
                //Build the edges and we're finished.
                BuildEdges();
            }
        }

        /// <summary>
        /// Builds the edges of the polygon.
        /// </summary>
        public void BuildEdges()
        {
            Vector2 start;
            Vector2 end;
            _Edges.Clear();
            for (int i = 0; i < _Vertices.Count; i++)
            {
                start = _Vertices[i];
                if (i + 1 >= _Vertices.Count)
                {
                    end = _Vertices[0];
                }
                else
                {
                    end = _Vertices[i + 1];
                }
                _Edges.Add(end - start);
                
            }
        }
        #endregion

        #region SAT Collision 
        /// <summary>
        /// Checks collision between this boundry and another boundry
        /// </summary>
        /// <param name="originOffset">The offset distance from the other polygon</param>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CheckCollision(Vector2 originOffset, IBoundry other)
        {
            //Check all sides:
            for (int i = 0; i < Edges.Length; i++)
            {
                //Perform separating axis theorem

                //1. FIND PERPENDICULAR AXIS TO CURRENT EDGE
                Vector2 axis = new Vector2(-Edges[i].Y, Edges[i].X);
                axis.Normalize();

                //--FIND PROJECTION OF BOTH BOUNDRIES ON THE AXIS.
                float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
                this.Project(axis, out minA, out maxA); //Zero offset (we are not away from our own position).
                other.Project(axis, out minB, out maxB); //What ever the distance to their origin is.

                //Check if the axis min/max is overlapping
                if (Collisions.IntervalDistance(minA, maxA, minB, maxB) > 0)
                    return false;
                else
                    ;
            }
            return true;
        }

        /// <summary>
        /// Projects this polygon onto an axis.
        /// </summary>
        /// <param name="axis">The axis onto which the polygon will be projected</param>
        /// <param name="min">The minimum point on the axis</param>
        /// <param name="max">The maximum point on the axis</param>
        public void Project(Vector2 axis,  out float min, out float max)
        {
            Collisions.Project(axis, out min, out max, Vertices);
        }
        #endregion

        #region Translational/Rotational
        /// <summary>
        /// Rotates the polygone a certain value in respects to the origin.
        /// </summary>
        /// <param name="degrees">How far to rotate the polygon</param>
        protected void Rotate(float degrees)
        {
            for (int i = 0; i < _Vertices.Count; i++)
            {
                _Vertices[i] = Vector2.Transform(_Vertices[i] - Origin, Matrix.CreateRotationZ(degrees)) + Origin;
            }
        }
        #endregion
        #endregion

        #region Fields

        protected List<Vector2> _Vertices;
        protected List<Vector2> _Edges;

        #endregion

        #region Properties

        #region Translational/Rotational
       /// <summary>
        /// Origin
        /// </summary>
        public Vector2 Origin
        {
            set
            {
                for (int i = 0; i < _Vertices.Count; i++){
                    _Vertices[i]+= (_Origin - value);
                }
                _Origin = value;
            }
            get
            {
                return _Origin;
            }
        }
        protected Vector2 _Origin;

        public Vector2 Center
        {
            get
            {
                Vector2 center = new Vector2(0, 0);
                for (int i = 0; i < Vertices.Length; i++)
                {
                    center += Vertices[i];
                }
                return center / new Vector2(Vertices.Length); //Average center
            }
        }

        /// <summary>
        /// Rotation of the polygon (radians)
        /// </summary>
        public float Rotation
        {
            set
            {
                Rotate(value - _Rotation);
                _Rotation = value;

            }
            get
            {
                return _Rotation;
            }
        }
        protected float _Rotation;

        public Vector2 Position
        {
            set
            {
                for (int i = 0; i < _Vertices.Count(); i++)
                {
                    _Vertices[i] += (value - _Position);
                }
                BuildEdges();
                _Position = value;

            }
            get
            {
                return _Position;
            }
        }
        protected Vector2 _Position;
       #endregion

        #region Vertex/Edge
        /// <summary>
        /// Returns the most extreme point on the polygon.
        /// </summary>
        public Vector2 ExtremeVertex
        {
            get
            {
                return _ExtremeVertex;
            }
        }
        private Vector2 _ExtremeVertex;

        /// <summary>
        /// Returns a list of vertices (in order) about the polygon
        /// </summary>
        public Vector2[] Vertices
        {
            get
            {
                return _Vertices.ToArray();
            }
        }

        /// <summary>
        /// Returns a list of edges (in order) about the polygon.
        /// </summary>
        public Vector2[] Edges
        {
            get
            {
                return _Edges.ToArray();
            }
        }
        #endregion
        #endregion

    }
}
