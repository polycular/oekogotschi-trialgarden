using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ToolbAR
{
    namespace Math
    {
        public static class Intersection3
        {


            public static bool existsBetween(Collider collider, Triangle3 triangle)
            {
                RaycastHit info;
                return (
                    collider.Raycast(new Ray(triangle.A, triangle.B - triangle.A), out info, (triangle.B - triangle.A).magnitude) ||
                    collider.Raycast(new Ray(triangle.B, triangle.C - triangle.B), out info, (triangle.C - triangle.B).magnitude) ||
                    collider.Raycast(new Ray(triangle.C, triangle.A - triangle.C), out info, (triangle.A - triangle.C).magnitude)
                    );
            }

            //Checks for containment first, then for intersection
            public static bool existsBetweenOrInside(Bounds box, Triangle3 triangle)
            {
                foreach (Vector3 p in triangle.ABC)
                {
                    if(box.Contains(p))
                        return true;
                }
                return existsBetween(box, triangle);
            }

            //from http://stackoverflow.com/a/17503268/931669
            public static bool existsBetween(Bounds box, Triangle3 triangle)
            {
                double triangleMin, triangleMax;
                double boxMin, boxMax;

                // Test the box normals (x-, y- and z-axes)
                var boxNormals = new Vector3[] {
                    new Vector3(1,0,0),
                    new Vector3(0,1,0),
                    new Vector3(0,0,1)
                };

                //Get the box vertices as array for project()
                var boxVertices = new Vector3[] {
                    box.center + new Vector3(box.extents.x, box.extents.y, box.extents.z),
                    box.center + new Vector3(box.extents.x, -box.extents.y, box.extents.z),
                    box.center + new Vector3(-box.extents.x, -box.extents.y, box.extents.z),
                    box.center + new Vector3(-box.extents.x, box.extents.y, box.extents.z),
                    box.center + new Vector3(box.extents.x, box.extents.y, -box.extents.z),
                    box.center + new Vector3(box.extents.x, -box.extents.y, -box.extents.z),
                    box.center + new Vector3(-box.extents.x, -box.extents.y, -box.extents.z),
                    box.center + new Vector3(-box.extents.x, box.extents.y, -box.extents.z),
                };


                for (int i = 0; i < 3; i++)
                {
                    project(triangle.ABC, boxNormals[i], out triangleMin, out triangleMax);
                    if (triangleMax < box.min[i] || triangleMin > box.max[i])
                        return false; // No intersection possible.
                }

                // Test the triangle normal
                double triangleOffset = Vector3.Dot(triangle.Normal, triangle.A);
                project(boxVertices, triangle.Normal, out boxMin, out boxMax);
                if (boxMax < triangleOffset || boxMin > triangleOffset)
                    return false; // No intersection possible.

                // Test the nine edge cross-products
                Vector3[] triangleEdges = new Vector3[] {
                    triangle.A - (triangle.B),
                    triangle.B - (triangle.C),
                    triangle.C - (triangle.A)
                };
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // The box normals are the same as it's edge tangents
                        Vector3 axis = Vector3.Cross(triangleEdges[i], boxNormals[j]);
                        project(boxVertices, axis, out boxMin, out boxMax);
                        project(triangle.ABC, axis, out triangleMin, out triangleMax);
                        if (boxMax <= triangleMin || boxMin >= triangleMax)
                            return false; // No intersection possible
                    }
                }

                // No separating axis found.
                return true;
            }

            static void project(IEnumerable<Vector3> points, Vector3 axis, out double min, out double max)
            {
                min = double.PositiveInfinity;
                max = double.NegativeInfinity;
                foreach (var p in points)
                {
                    double val = Vector3.Dot(axis,p);
                    if (val < min) min = val;
                    if (val > max) max = val;
                }
            }
        }
    }
}