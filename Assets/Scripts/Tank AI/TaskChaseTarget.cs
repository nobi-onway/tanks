using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskChaseTarget : Node
{
    private BehaviourTree.Tree _tree;
    private NavMeshAgent _navMeshAgent;

    public TaskChaseTarget(BehaviourTree.Tree tree)
    {
        _tree = tree;

        _navMeshAgent = _tree.GetComponent<NavMeshAgent>();
    }
    public override ENodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(target.position);

        state = ENodeState.RUNNING;
        return state;
    }
}
