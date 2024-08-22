using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class CheckEnemyInFireRange : Node
{
    private float _fireRadius;
    private BehaviourTree.Tree _tree;
    private NavMeshAgent _navMeshAgent;
    private TankShooting _tankShooting;

    public CheckEnemyInFireRange(BehaviourTree.Tree tree, float fireRadius)
    {
        _fireRadius = fireRadius;
        _tree = tree;

        _navMeshAgent = _tree.GetComponent<NavMeshAgent>();
        _tankShooting = _tree.GetComponent<TankShooting>();
    }

    public override ENodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        Vector3 direction = target.position - _tree.transform.position;

        bool isInFireRange = Vector3.Distance(_tree.transform.position, target.position) <= _fireRadius;
        bool hasNoneObstacle = Physics.Raycast(_tree.transform.position, direction, _fireRadius, LayerMask.GetMask("Players"));

        bool canFire = isInFireRange && hasNoneObstacle;

        if (canFire)
        {
            _navMeshAgent.isStopped = true;

            state = ENodeState.SUCCESS;
            return state;
        }

        _tankShooting.ResetLauch();

        state = ENodeState.FAILURE;
        return state;
    }
}
