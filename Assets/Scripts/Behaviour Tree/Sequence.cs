using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override ENodeState Evaluate()
        {
            bool isAnyChildRunning = false;

            foreach (Node child in children)
            {
                switch (child.Evaluate())
                {
                    case ENodeState.FAILURE:
                        state = ENodeState.FAILURE;
                        return state;
                    case ENodeState.RUNNING:
                        isAnyChildRunning = true;
                        continue;
                    case ENodeState.SUCCESS:
                        continue;
                }
            }    

            state = isAnyChildRunning ? ENodeState.RUNNING : ENodeState.SUCCESS;
            return state;
        }
    }
}
