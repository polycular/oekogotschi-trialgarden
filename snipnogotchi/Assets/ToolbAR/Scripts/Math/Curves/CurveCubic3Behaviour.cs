using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ToolbAR
{
    namespace Math
    {
        namespace Curves
        {

            public class CurveCubic3Behaviour : MonoBehaviour
            {
                public int Resolution = 10;
                public Color Color = Color.cyan;

                public Transform T1 = null;
                public Transform C1 = null;
                public Transform C2 = null;
                public Transform T2 = null;

                public bool IsAutoUpdating = true;


                Cubic3 mCurve = null;

                void Start()
                {
                    updateCurve();
                }
                void Update()
                {
                    updateCurve();
                }

                public Curves.Cubic3 Curve
                {
                    get { return mCurve; }
                }

                void OnDrawGizmos()
                {
                    if (IsAutoUpdating && isValid())
                    {
                        updateCurve();

                        Gizmos.color = Color;

                        Vector3 lp = Vector3.zero;
                        mCurve.walk(Resolution, delegate(float t, int i, Vector3 position)
                        {
                            if (i > 0)
                            {
                                Gizmos.DrawLine(lp, position);
                            }
                            lp = position;
                        });
                    }
                }

                public void updateCurve()
                {
                    if (isValid())
                    mCurve = new Curves.Cubic3(
                        T1.transform.position,
                        C1.transform.position,
                        C2.transform.position,
                        T2.transform.position
                    );
                }

                public bool isValid()
                {
                    return (T1 != null && T2 != null && C1 != null && C2 != null);
                }
            }

        }
    }
}