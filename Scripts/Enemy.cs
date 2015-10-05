using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity 
{
    public enum State { Idle, Chasing, Attacking };
    State currentState;

	NavMeshAgent pathFinder;
	Transform target;
    Material skinMaterial;
    Entity targetEntity;

    Color originalColor;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float collisionRadius;
    float targetCollisionRadius;
    float damage = 1;

    bool hasTarget;

	protected override void Start () 
	{
        base.Start();

        pathFinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<Entity>();
            targetEntity.OnDeath += OnTargetDeath;

            collisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        } 
	}

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update () 
	{
        if(hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + collisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
	}

    IEnumerator Attack()
    {
        pathFinder.enabled = false;
        currentState = State.Attacking;
        skinMaterial.color = Color.white;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * collisionRadius;   

        float attackSpeed = 3;
        float percent = 0;
        bool hasDealtDmg = false;

        while(percent <= 1)
        {
            if(percent >= .5f && !hasDealtDmg)
            {
                hasDealtDmg = true;
                targetEntity.TakeDmg(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interp = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interp);

            yield return null;
        }

        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathFinder.enabled = true;
    }

	IEnumerator UpdatePath()
	{
		float refreshRate = .25f;

		while(target != null)
		{
            if(currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (collisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }
            
			yield return new WaitForSeconds(refreshRate);
		}
	}
}
