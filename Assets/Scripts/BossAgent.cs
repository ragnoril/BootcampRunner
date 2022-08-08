using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAgent : MonoBehaviour
{
    public bool isAttacking;
    public Transform Target;
    public float MoveSpeed;

    public float AttackRate;
    private float attackTimer;
    public int AttackPower;
    public int Health;

    public bool isInAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0f;
        
        GameManager.Instance.Events.OnFightStarted += StartFighting;
        GameManager.Instance.Events.OnBossAttacked += GetHit;
    }

    private void OnDestroy()
    {
        GameManager.Instance.Events.OnFightStarted -= StartFighting;
        GameManager.Instance.Events.OnBossAttacked -= GetHit;
    }

    private void GetHit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            isAttacking = false;
            GameManager.Instance.Events.LevelFinished(true);
        }
    }

    private void StartFighting()
    {
        isInAttackRange = false;
        isAttacking = true;
        ChooseTarget();
    }

    private void ChooseTarget()
    {
        if (GameManager.Instance.Player.Followers.Count > 0)
        {
            int randId = UnityEngine.Random.Range(0, GameManager.Instance.Player.Followers.Count);
            Target = GameManager.Instance.Player.Followers[randId].transform;
        }
        else
        {
            Target = GameManager.Instance.Player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            if (Target == null) ChooseTarget();
            if (!isInAttackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, Target.position, MoveSpeed * Time.deltaTime);
            }
            else
            {
                attackTimer -= Time.deltaTime;

                if (attackTimer < 0f)
                {
                    transform.forward = -Target.forward;

                    //Debug.Log("Attacking this guy: " + Target.gameObject + " with tag: " + Target.tag);

                    if (Target.tag == "Player")
                    {
                        GameManager.Instance.Events.PlayerAttacked(AttackPower);
                    }
                    else if (Target.tag == "Follower")
                    {
                        FollowerAgent follower = Target.GetComponent<FollowerAgent>();
                        GameManager.Instance.Events.FollowerAttacked(follower, AttackPower);
                    }


                    attackTimer = AttackRate;
                }
            }
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("target tag: " + Target.tag + "  collision tag: " + collision.transform.tag);
        if (collision.transform.tag == "Player")
        {

            if (Target.tag == collision.transform.tag)
                isInAttackRange = true;
            
        }

        if (collision.transform.tag == "Follower")
        {
            if (Target.tag == collision.transform.tag)
            {
                isInAttackRange = true;
                Target = collision.transform;
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (Target.tag == collision.transform.tag)
            {
                isInAttackRange = false;
                attackTimer = 0f;
            }
        }

        if (collision.transform.tag == "Follower")
        {
            if (Target.tag == collision.transform.tag)
            {
                isInAttackRange = false;
                attackTimer = 0f;
            }
        }
    }


}
