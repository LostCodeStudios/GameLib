using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLib.Controller.Collision
{
    public interface IBoundry
    {
        bool CheckCollision(Vector2 originOffset, IBoundry another);
        void Project(Vector2 axis, out float min, out float max);
    }
}