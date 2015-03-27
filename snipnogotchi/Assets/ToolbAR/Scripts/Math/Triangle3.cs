using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    namespace Math
    {
        public class Triangle3
        {
            Vector3 mA;
            Vector3 mB;
            Vector3 mC;
            Vector3 mNormal;
            
            public Vector3 A
            {
                get {
                    return mA;
                }
                set {
                    mA = value;
                    recalculateNormal();
                }
            }
            public Vector3 B
            {
                get {
                    return mB;
                }
                set {
                    mB = value;
                    recalculateNormal();
                }
            }
            public Vector3 C
            {
                get
                {
                    return mC;
                }
                set
                {
                    mC = value;
                    recalculateNormal();
                }
            }

            public Vector3[] ABC
            {
                get
                {
                    return new Vector3[] { mA, mB, mC };
                }
                set
                {
                    if (value.Length < 3)
                        throw new System.Exception("Triangle3.ABC received Array too small");

                    mA = value[0];
                    mB = value[1];
                    mC = value[2];
                    recalculateNormal();
                }
            }

            public Vector3 Normal
            {
                get
                {
                    return mNormal;
                }
            }

            public Triangle3(Vector3 p1, Vector3 p2, Vector3 p3)
            {
                this.mA = p1;
                this.mB = p2;
                this.mC = p3;
                this.recalculateNormal();
            }
            public Triangle3(float Ax, float Ay, float Az, float Bx, float By, float Bz, float Cx, float Cy, float Cz)
            {
                this.mA = new Vector3(Ax, Ay, Az);
                this.mB = new Vector3(Bx, By, Bz);
                this.mC = new Vector3(Cx, Cy, Cz);
                this.recalculateNormal();
            }

            void recalculateNormal()
            {
                Vector3 dir = Vector3.Cross(mB - mA, mC - mA);
                mNormal = Vector3.Normalize(dir);
            }
        }
    }
}

