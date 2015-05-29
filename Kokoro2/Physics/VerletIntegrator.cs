using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro2.Math;
using Kokoro2.Engine;

namespace Kokoro2.Physics
{
    /// <summary>
    /// Verlet physics integrator
    /// </summary>
    public class VerletIntegrator
    {
        /// <summary>
        /// A collection of all VerletObjects processed by this instance of the integrator
        /// </summary>
        public List<VObject> VerletObjects { get; internal set; }

        public VerletIntegrator()
        {
            VerletObjects = new List<VObject>();
        }

        public void Add(VObject vObj)
        {
            VerletObjects.Add(vObj);
        }

        public void Update(double interval, GraphicsContext context)
        {
            UpdatePoints(interval);
            UpdateEdges(interval);
            UpdatePoints(interval);
            UpdateEdges(interval);
        }

        public void UpdateEdges(double interval)
        {
            for (int i = 0; i < VerletObjects.Count; i++)
            {
                var tmpVObj = VerletObjects[i];

                for (int i1 = 0; i1 < tmpVObj.Edges.Length; i1++)
                {
                    var tmpVEdge = tmpVObj.Edges[i1];

                    Vector3 diffVec = tmpVEdge.A.Position - tmpVEdge.B.Position;
                    if (!(diffVec.LengthSquared < tmpVEdge.LengthSquared + tmpVEdge.ElasticitySquared && diffVec.LengthSquared > tmpVEdge.LengthSquared + tmpVEdge.ElasticitySquared))  //Is the length too short or too long?
                    {
                        //Calculate the difference from the original length
                        float Diff = 0;

                        diffVec.Normalize();

                        //Push both vertices apart by half of the difference respectively 
                        //so the distance between them equals the original length
                        float weightA = 0.5f, weightB = 0.5f;

                        if (tmpVEdge.A.Pin && !tmpVEdge.B.Pin)
                        {
                            weightA = 0;
                            weightB = 1;
                            Diff = (float)System.Math.Sqrt(diffVec.LengthSquared - tmpVEdge.LengthSquared);
                        }
                        else if (tmpVEdge.B.Pin && !tmpVEdge.A.Pin)
                        {
                            weightA = 1;
                            weightB = 0;
                            Diff = (float)System.Math.Sqrt(diffVec.LengthSquared - tmpVEdge.LengthSquared);
                        }
                        else if (tmpVEdge.B.Pin && tmpVEdge.A.Pin)
                        {
                            weightB = weightA = 0;
                            Diff = (float)System.Math.Sqrt(diffVec.LengthSquared - tmpVEdge.LengthSquared);
                        }

                        tmpVEdge.A.Position += diffVec * Diff * weightA;
                        tmpVEdge.B.Position -= diffVec * Diff * weightB;
                        //TODO Adjust the vectors so the length is correct again taking into account the pinning of each point, we can't complain if both points are pinned, but if one point is pinned, the other has to be moved more to compensate
                    }

                }
            }
        }

        public void UpdatePoints(double interval)
        {
            for (int i = 0; i < VerletObjects.Count; i++)
            {
                var tmpVObj = VerletObjects[i];

                for (int i1 = 0; i1 < tmpVObj.Edges.Length; i1++)
                {
                    var tmpVEdge = tmpVObj.Edges[i1];

                    if (!tmpVEdge.A.Pin)  //Update point 'A's position if it isn't pinned
                    {
                        tmpVEdge.A.Position += tmpVEdge.A.Position - tmpVEdge.A.OldPosition + tmpVEdge.A.Acceleration * (float)(interval * interval);
                    }

                    if (!tmpVEdge.B.Pin)
                    {
                        tmpVEdge.B.Position += tmpVEdge.B.Position - tmpVEdge.B.OldPosition + tmpVEdge.B.Acceleration * (float)(interval * interval);
                    }
                }
            }
        }

    }
}
