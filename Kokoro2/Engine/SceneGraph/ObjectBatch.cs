using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro2.Engine.SceneGraph
{
    public class ObjectBatch
    {

        //This is used to batch together draw calls, the idea is that each one of these has its own GPUBuffer objects. They represent small collections of all the data necessary for that piece
        //These pieces are then drawn based on a quick visibility ray cast, each one of these is setup so that all parts of it can be submitted in one draw call
        //The actual visibility testing is up to the implementation, although the world class will provide a version which uses octrees of ObjectBatches to represent levels of detail
        //We will also want to see how fast it is to generate data for GPUBuffers on the fly for procedural generation.
    }
}
