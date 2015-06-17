using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.Common
{
    public class StateMachine : StateMachineBase
    {
        private Dictionary<string, StateMachineBase> children;
        private StateMachineBase curScene;

        public override void Initialize(GameDevice device)
        {
            children = new Dictionary<string, StateMachineBase>();
            curScene = null;
            base.Initialize(device);
        }

        public void Add(string name, StateMachineBase scene)
        {
            children[name] = scene;
        }

        public void Remove(string name)
        {
            children.Remove(name);
        }

        public void Activate(string name)
        {
            curScene = children[name];
        }

        public StateMachineBase this[string name]
        {
            get
            {
                return children[name];
            }
            set
            {
                children[name] = value;
            }
        }

        public override void Render(double interval)
        {
            if (curScene != null)
            {
                if (!curScene.Initialized) curScene.Initialize(Device);
                curScene.Render(interval);
            }
        }

        public override void Update(double interval)
        {
            if (curScene != null && curScene.Initialized) curScene.Update(interval);
        }

    }
}
