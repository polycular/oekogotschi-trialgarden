using UnityEngine;
using System.Collections;


namespace ToolbAR.Math.Shapes
{

    /************************************************************************/
    /* Helper class for line shape
     * - check closest points/closest lines to a specific point
     * - check on which side of a line a point lies
    /************************************************************************/
    public class Line
    {

        /// <summary>
        /// Checks on which side the point lies         
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="point"></param>
        /// <returns>0 on the line, +1 on one side, -1 on the other side</returns>
        public static float testPoint(Vector2 a, Vector2 b, Vector2 point)
        {
            return ((b.x - a.x) * (point.y - a.y) - (b.y - a.y) * (point.x - a.x));
        }


        /// <summary>
        /// Gets the closest point on the line or segment
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <param name="segmentClamp">true if you want the closest point on the segment, not just the line.</param>
        /// <returns></returns>
        public static Vector2 getClosestPointOn(Vector2 A, Vector2 B, Vector2 P, bool segmentClamp)
        {
            Vector2 AP = P - A;
            Vector2 AB = B - A;
            float ab2 = AB.x * AB.x + AB.y * AB.y;
            float ap_ab = AP.x * AB.x + AP.y * AB.y;
            float t = ap_ab / ab2;
            if (segmentClamp)
            {
                if (t < 0.0f) t = 0.0f;
                else if (t > 1.0f) t = 1.0f;
            }
            Vector2 Closest = A + AB * t;
            return Closest;
        }
    }

}