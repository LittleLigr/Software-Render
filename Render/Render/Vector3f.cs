using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    /// <summary>
    /// Задает трёхмерный вектор с плавающей запятой
    /// </summary>
    class Vector3f
    {
        public float x, y, z;

        public Vector3f() { }

        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3f operator *(Vector3f v1, float value)
        {
            return new Vector3f(v1.x *value, v1.y *value, v1.z * value);
        }

        public static Vector3f operator /(Vector3f v1, float value)
        {
            return new Vector3f(v1.x / value, v1.y / value, v1.z / value);
        }

        public static Vector3f operator +(Vector3f v1, float value)
        {
            return new Vector3f(v1.x + value, v1.y + value, v1.z + value);
        }

        public static Vector3f operator +(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3f operator -(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public override string ToString()
        {
            return "("+x+"; "+y+"; "+z+")";
        }
    }
}
