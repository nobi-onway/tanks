using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class TaskFire : Node
{
    private TankShooting _tankShooting;
    private BehaviourTree.Tree _tree;

    public TaskFire(BehaviourTree.Tree tree)
    {
        _tree = tree;
        
        _tankShooting = _tree.GetComponent<TankShooting>();
    }

    public override ENodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        Vector3 direction = target.position - _tree.transform.position;

        if (target == null) { state = ENodeState.FAILURE; return state; }

        _tankShooting.AIFire(target.position);

        Quaternion rotation = Quaternion.LookRotation(direction);
        _tree.transform.rotation = rotation;

        state = ENodeState.RUNNING;
        return state;
    }
}
