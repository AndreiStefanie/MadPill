using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity 
{
    public enum State { Idle, Chasing, Attacking };
    State currentState;

	NavMeshAgent pathFinder;
	Transform target;

    float attackDistanceThreshold = 1.5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float collisionRadius;
    float targetCollisionRadius;

	protected override void Start () 
	{
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = State.Chasing;
        collisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

		StartCoroutine(UpdatePath());
	}

	void Update () 
	{
        if(Time.time > nextAttackTime)
        {
            float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

            if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold, 2))
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
        
	}

    IEnumerator Attack()
    {
        pathFinder.enabled = false;
        currentState = State.Attacking;

        Vector3 originalPosition = transform.position;
        Vector3 attackPosition = target.position;

        float attackSpeed = 3;
        float percent = 0;

        while(percent <= 1)
        {
            percent += Time.time * attackSpeed;
            float interp = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interp);

            yield return null;
        }

        currentState = State.Chasing;
        pathFinder.enabled = true;
    }

	IEnumerator UpdatePath()
	{
		float refreshRate = 0.25f;

		while(target != null)
		{
            if(currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (collisionRadius + targetCollisionRadius);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }
            
			yield return new WaitForSeconds(refreshRate);
		}
	}
}
