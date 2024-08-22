using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
using System.Collections.Generic;

public class TankBT : BehaviourTree.Tree
{
    private Transform _target;
    private Transform[] _waypoints;
    private readonly float FIRE_RADIUS = 30.0f;

    protected override Node SetUpTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInFireRange(this, FIRE_RADIUS),
                new TaskFire(this)
            }),
            new TaskChaseTarget(this),
        });

        root.SetData("target", _target);
        root.SetData("waypoints", _waypoints);
        root.SetData("destination", this.transform.position);

        return root;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetWayPoints(Transform[] waypoints)
    {
        _waypoints = waypoints;
    }
}
