using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Physics.Prefabs;
using Kokoro2.Math;

namespace Kokoro2.Physics
{
    public class CollisionSolver
    {
        //TODO Finish implementing all collision handlers

        #region Is Colliding
        public bool IsColliding(PhysicsState a, PhysicsState b)
        {
            //Determine which ICollisionBody's are involved, pick an appropriate test and calculate
            Type aType = a.CollisionBody.GetType();
            Type bType = b.CollisionBody.GetType();

            if (aType == typeof(PhysSphere))
            {
                if (bType == typeof(PhysSphere)) //Sphere vs Sphere test
                {
                    return IsSphereVsSphere((PhysSphere)a.CollisionBody, (PhysSphere)b.CollisionBody);
                }
                else if (bType == typeof(Ray))  //Ray vs Sphere test
                {
                    return IsSphereVsRay((PhysSphere)a.CollisionBody, (Ray)b.CollisionBody);
                }
                else if (bType == typeof(AABB)) //Sphere vs AABB
                {

                }
                else if (bType == typeof(PhysVertexMesh))   //Sphere vs Mesh
                {

                }
            }
            else if (aType == typeof(Ray))
            {
                if (bType == typeof(PhysSphere)) //Ray vs Sphere test
                {
                    return IsSphereVsRay((PhysSphere)b.CollisionBody, (Ray)a.CollisionBody);
                }
                else if (bType == typeof(Ray))  //Ray vs Ray test
                {

                }
                else if (bType == typeof(AABB)) //Ray vs AABB
                {

                }
                else if (bType == typeof(PhysVertexMesh))   //Ray vs Mesh
                {

                }
            }
            else if (aType == typeof(AABB))
            {
                if (bType == typeof(PhysSphere)) //AABB vs Sphere test
                {

                }
                else if (bType == typeof(Ray))  //AABB vs Ray test
                {

                }
                else if (bType == typeof(AABB)) //AABB vs AABB
                {

                }
                else if (bType == typeof(PhysVertexMesh))   //AABB vs Mesh
                {

                }
            }
            else if (aType == typeof(PhysVertexMesh))
            {
                if (bType == typeof(PhysSphere)) //Mesh vs Sphere test
                {

                }
                else if (bType == typeof(Ray))  //Mesh vs Ray test
                {

                }
                else if (bType == typeof(AABB)) //Mesh vs AABB
                {

                }
                else if (bType == typeof(PhysVertexMesh))   //Mesh vs Mesh
                {

                }
            }

            return true;
        }
        #endregion

        #region Collision Point
        public Vector3 CollidingAt(PhysicsState a, PhysicsState b)
        {
            //Determine which ICollisionBody's are involved, pick an appropriate test and calculate
            Type aType = a.CollisionBody.GetType();
            Type bType = b.CollisionBody.GetType();

            if (aType == typeof(PhysSphere))
            {
                if (bType == typeof(Ray))  //Ray vs Sphere test
                {
                    return ((Ray)b.CollisionBody).Direction * DistSphereVsRay((PhysSphere)a.CollisionBody, (Ray)b.CollisionBody); ;
                }
                else if (bType == typeof(AABB)) //Sphere vs AABB
                {

                }
                else if (bType == typeof(PhysVertexMesh))   //Sphere vs Mesh
                {

                }
            }
            else if (aType == typeof(Ray))
            {
                if (bType == typeof(PhysSphere)) //Ray vs Sphere test
                {
                    return ((Ray)a.CollisionBody).Direction * DistSphereVsRay((PhysSphere)b.CollisionBody, (Ray)a.CollisionBody); ;
                }
                else if (bType == typeof(Ray))  //Ray vs Ray test
                {

                }
                else if (bType == typeof(AABB)) //Ray vs AABB
                {

                }
                else if (bType == typeof(PhysVertexMesh))   //Ray vs Mesh
                {

                }
            }
            else if (aType == typeof(AABB))
            {
                if (bType == typeof(PhysSphere)) //AABB vs Sphere test
                {

                }
                else if (bType == typeof(Ray))  //AABB vs Ray test
                {

                }
                else if (bType == typeof(AABB)) //AABB vs AABB
                {

                }
                else if (bType == typeof(PhysVertexMesh))   //AABB vs Mesh
                {

                }
            }
            else if (aType == typeof(PhysVertexMesh))
            {
                if (bType == typeof(PhysSphere)) //Mesh vs Sphere test
                {

                }
                else if (bType == typeof(Ray))  //Mesh vs Ray test
                {

                }
                else if (bType == typeof(AABB)) //Mesh vs AABB
                {

                }
                else if (bType == typeof(PhysVertexMesh))   //Mesh vs Mesh
                {

                }
            }

            return Vector3.Zero;
        }
        #endregion

        #region Sphere vs Sphere
        public bool IsSphereVsSphere(PhysSphere a, PhysSphere b)
        {
            var diff = a.Center - b.Center;
            return (diff.LengthSquared <= (a.Radius + b.Radius) * (a.Radius + b.Radius));
        }
        #endregion

        #region Sphere vs Ray
        public bool IsSphereVsRay(PhysSphere a, Ray b)
        {
            var diff = a.Center - b.Origin;
            if (diff.LengthSquared <= a.Radius * a.Radius) return true;
            else
            {
                var dirDot = Vector3.Dot(diff, b.Direction);
                if (dirDot < 0) return false;
                float tmp = Vector3.Dot(diff, diff) - (dirDot * dirDot);
                return (tmp < a.Radius);
            }
        }

        public float DistSphereVsRay(PhysSphere a, Ray b)
        {
            var diff = a.Center - b.Origin;
            if (diff.LengthSquared <= a.Radius * a.Radius) return diff.Length;
            else
            {
                var dirDot = Vector3.Dot(diff, b.Direction);
                if (dirDot < 0) return -1;
                float tmp = Vector3.Dot(diff, diff) - (dirDot * dirDot);
                if (tmp > a.Radius) return -1;
                float thc = (float)System.Math.Sqrt(a.Radius - tmp);
                float t0 = dirDot - thc;
                return t0;
            }
        }
        #endregion

        #region Sphere vs AABB

        #endregion

        #region Ray vs AABB

        #endregion

        #region AABB vs AABB

        #endregion

        #region Ray vs Ray

        #endregion

    }
}
