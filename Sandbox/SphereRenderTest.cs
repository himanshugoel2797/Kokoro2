using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kokoro3.Engine;
using Kokoro3.Math;
using Kokoro3.Common;

namespace Sandbox
{
    public class SphereRenderTest : StateMachineBase
    {
        GameDevice device;

        public SphereRenderTest(GameDevice device)
        {
            this.device = device;
            device.ClearColor = Vector4.One;
        }

        public override void Render(double interval)
        {
            device.ClearColorBuffer();
            device.SwapBuffer();
        }

        public override void Update(double interval)
        {
            device.ClearColor = device.ClearColor - new Vector4(0.0001f, 0.0001f, 0.0001f, 0);
        }
    }
}
