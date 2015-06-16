using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.Common
{
    public class StackedStateMachine : StateMachineBase
    {
        private Stack<StateMachineBase> children;

        public override void Initialize(GameDevice device)
        {

            children = new Stack<StateMachineBase>();
            base.Initialize(device);
        }

        public void Push(StateMachineBase s)
        {
            children.Push(s);
        }

        public StateMachineBase Pop()
        {
            return children.Pop();
        }

        public override void Render(double interval)
        {
            if (children.Count > 0)
            {
                var curScene = children.Peek();
                if (!curScene.Initialized) curScene.Initialize(Device);
                curScene.Render(interval);
            }
        }

        public override void Update(double interval)
        {
            if (children.Count > 0)
            {
                var curScene = children.Peek();
                if (curScene.Initialized) curScene.Update(interval);
            }
        }

    }
}
