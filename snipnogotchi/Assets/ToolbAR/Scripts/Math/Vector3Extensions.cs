using UnityEngine;

namespace ToolbAR
{
    namespace Math
    {
        public static class Vector3Extensions
        {
            //In difference to Vector3.Angle this will get the full signed angle
            public static float getSignedAngleTo(this Vector3 vec, Vector3 other, Vector3 normal)
            {
                float dot = vec.dot(other);
                float det = vec.getDeterminant(other, normal);
                return Mathf.Atan2(det, dot) * Mathf.Rad2Deg;
            }

            public static float getDeterminant(this Vector3 vec, Vector3 other, Vector3 normal)
            {
                return vec.x * other.y * normal.z + other.x * normal.y * vec.z + normal.x * vec.y * other.z - vec.z * other.y * normal.x - other.z * normal.y * vec.x - normal.z * vec.y * other.x;
            }

            public static float dot(this Vector3 vec, Vector3 other)
            {
                return Vector3.Dot(vec, other);
            }

            public static Vector2 getXY(this Vector3 vec)
            {
                return new Vector2(vec.x, vec.y);
            }
            public static Vector2 getXZ(this Vector3 vec)
            {
                return new Vector2(vec.x, vec.z);
            }
            public static Vector2 getYZ(this Vector3 vec)
            {
                return new Vector2(vec.y, vec.z);
            }
            public static Vector2 getYX(this Vector3 vec)
            {
                return new Vector2(vec.y, vec.x);
            }
            public static Vector2 getZY(this Vector3 vec)
            {
                return new Vector2(vec.z, vec.y);
            }
            public static Vector2 getZX(this Vector3 vec)
            {
                return new Vector2(vec.z, vec.x);
            }
        }
    }
}