using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    namespace Math
    {
        namespace Curves
        {
            public class Cubic3
            {
                protected Vector3[] mPoints;

                public Cubic3(Vector3 tangentPoint1, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 tangentPoint2)
                {
                    mPoints = new Vector3[4];
                    mPoints[0] = tangentPoint1;
                    mPoints[1] = controlPoint1;
                    mPoints[2] = controlPoint2;
                    mPoints[3] = tangentPoint2;
                }

                public Vector3 TangentPointStart
                {
                    get { return mPoints[0]; }
                }

                public Vector3 TangentPointEnd
                {
                    get { return mPoints[3]; }
                }

                public Vector3 ControlPointStart
                {
                    get { return mPoints[1]; }
                }

                public Vector3 ControlPointEnd
                {
                    get { return mPoints[2]; }
                }

                public int Length
                {
                    get { return mPoints.Length; }
                }

                public Vector3 getPoint(int i)
                {
                    return mPoints[i];
                }
                public void setPoint(int i, Vector3 point)
                {
                    mPoints[i] = point;
                }

                public Vector3 getPosition(float t)
                {
                    float u = 1f - t;
                    float tt = t * t;
                    float uu = u * u;
                    float uuu = uu * u;
                    float ttt = tt * t;

                    Vector3 pos = uuu * mPoints[0]; //first term
                    pos += 3 * uu * t * mPoints[1]; //second term
                    pos += 3 * u * tt * mPoints[2]; //third term
                    pos += ttt * mPoints[3]; //fourth term

                    return pos;
                }

                public delegate void WalkHandler(float t, int i, Vector3 position);
                //Steps are inclusive, meaning that steps=10 runs from [0, 10], and they have to be > 0
                public void walk(int steps, WalkHandler handler)
                {
                    if (steps <= 0)
                        return;

                    float resolution = 1f / (float)steps;
                    for (int i = 0; i <= steps; i++)
                    {
                        float t = resolution * i;
                        handler(t, i, getPosition(t));
                    }
                }
            }
        }
    }
}