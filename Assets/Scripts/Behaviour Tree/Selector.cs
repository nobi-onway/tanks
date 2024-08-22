using System.Collections.Generic;

namespace BehaviourTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> nodes) : base(nodes) { }

        public override ENodeState Evaluate()
        {
            foreach(Node child in children)
            {
                switch(child.Evaluate())
                {
                    case ENodeState.FAILURE:
                        continue;
                    case ENodeState.RUNNING:
                        state = ENodeState.RUNNING;
                        return state;
                    case ENodeState.SUCCESS:
                        state = ENodeState.SUCCESS;
                        return state;
                }
            }

            state = ENodeState.FAILURE;
            return state;
        }
    }

}
