using UnityEngine;
using System.Collections;

namespace ToolbAR.Math.Shapes
{
    /************************************************************************/
    /*  Helper Class for rectangle shapes                                                        */
    /************************************************************************/
    public class Rectangle
    {
        public Vector2 A, B, C, D;
        public Rectangle(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public Vector2 AB
        {
            get { return B - A; }
        }

        public Vector2 BC
        {
            get { return C - B; }
        }

        public Vector2 CD
        {
            get { return D - C; }
        }

        public Vector2 DA
        {
            get { return A - D; }
        }

        /**
         * Constructs a Rectangle along a pivotal line
         * @param name="lineExtent" extends the pivotal line by the given length
         * @param name="normalExtent" defines the half-"height" of the Rectangle (amount of extension normal to the pivotal line)
         */
        public Rectangle(Vector2 startPivot, Vector2 endPivot, float lineExtent, float normalExtent)
        {
            Vector2 pivotalLine = (endPivot - startPivot).normalized;
            Vector2 normalLine = new Vector2(-pivotalLine.y, pivotalLine.x);
            pivotalLine *= lineExtent;
            normalLine *= normalExtent;
            A = startPivot - pivotalLine + normalLine;
            B = endPivot + pivotalLine + normalLine;
            C = endPivot + pivotalLine - normalLine;
            D = startPivot - pivotalLine - normalLine;
        }

        public bool containsPoint(Vector2 point)
        {
            //Check for each side if point is on the inner side
            if (
                Line.testPoint(A, B, point) <= 0 &&
                Line.testPoint(B, C, point) <= 0 &&
                Line.testPoint(C, D, point) <= 0 &&
                Line.testPoint(D, A, point) <= 0
                ) return true;
            else
                return false;
        }

    }

}