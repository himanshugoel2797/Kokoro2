using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kokoro2.Engine.Prefabs;
using System.Threading.Tasks;

namespace Kokoro2.Engine.HighLevel.Voxel
{
    public class VoxelWorld
    {
        VoxelCollection voxelTypes;

        Dictionary<Vector3d, VoxelChunk> Chunks;

        const int SideLength = 16;
        const int rendererCount = 16;
        const int drawRadius = 4;

        int seed;

        VoxelRenderer extractor;    //This is mainly used to extract the surface data, we use InputDataMesh to render the meshes
        InputDataMesh[] renderers;

        public VoxelWorld(VoxelCollection voxelTypes, int seed, GraphicsContext c)
        {
            this.seed = seed;
            this.voxelTypes = voxelTypes;
            Chunks = new Dictionary<Vector3d, VoxelChunk>();
            extractor = new VoxelRenderer(SideLength, voxelTypes, c);

            renderers = new InputDataMesh[rendererCount];
            for (int i = 0; i < rendererCount; i++)
            {
                renderers[i] = new InputDataMesh(SideLength * SideLength * SideLength * 14 * 4, c);    //Define the maximum possible stored amount

                //TODO Fix the shader setup for the voxelworld

                renderers[i].Material.AlbedoMap = voxelTypes.Atlas;
            }
        }

        public void Render(GraphicsContext context)
        {
            //Draw in a circle around the player
            Vector3d blockPos = new Vector3d(context.Camera.Position.X - (context.Camera.Position.X % SideLength), context.Camera.Position.Y - (context.Camera.Position.Y % SideLength), context.Camera.Position.Z - (context.Camera.Position.Z % SideLength));

            //blockPos = Vector3d.Zero;

            for (int x = -drawRadius / 2; x < drawRadius / 2; x++)
            {
                for (int y = -drawRadius / 2; y < drawRadius / 2; y++)
                {
                    Vector3d key = blockPos + new Vector3d(x * SideLength, 0, y * SideLength);

                    if (!Chunks.ContainsKey(key))
                    {
                        Chunks[key] = new VoxelChunk(SideLength, seed, (Vector3)key);
                    }

                    //Generate the data for the chunk and back it up
                    if (Chunks[key].Dirty)
                    {
                        extractor.GenerateMeshData(Chunks[key]);
                        Chunks[key].Dirty = false;
                        Chunks[key].Vertices = extractor.Vertices.ToList();
                        Chunks[key].Normals = extractor.Normals.ToList();
                        Chunks[key].UVs = extractor.UVs.ToList();
                        Chunks[key].Indices = extractor.Indices.ToList();
                    }

                    renderers[(x + drawRadius / 2) + (y + drawRadius / 2)].GenerateMeshData(Chunks[key].Vertices.ToArray(), Chunks[key].UVs.ToArray(), Chunks[key].Normals.ToArray(), Chunks[key].Indices.ToArray());
                    renderers[(x + drawRadius / 2) + (y + drawRadius / 2)].RenderInfo.World = Matrix4.CreateTranslation((float)blockPos.X + x * SideLength, 0, (float)blockPos.Y + y * SideLength);
                    context.Draw(renderers[(x + drawRadius / 2) + (y + drawRadius / 2)]);

                }
            }


        }

    }
}
