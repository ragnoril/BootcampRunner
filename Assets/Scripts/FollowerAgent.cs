using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAgent : MonoBehaviour
{
    public bool isAttacking;
    public bool isFollowing;
    public Color FollowingColor;
    public float FollowSpeed;
    public Vector3 FollowDistance;

    public int AttackPower;
    public int Health;

    public bool isInAttackRange;
    public float AttackRate;
    private float attackTimer;

    private void Start()
    {
        isInAttackRange = false;
        attackTimer = 0f;

        GameManager.Instance.Events.OnFightStarted += StartFighting;
    }
    private void OnDestroy()
    {
        GameManager.Instance.Events.OnFightStarted -= StartFighting;
    }

    public void GetHit(int damage)
    {
        Debug.Log("i am (" + gameObject + ") hit with " + damage + " remaining health: " + Health);
        Health -= damage;

        if (Health <= 0)
        {
            Debug.Log("I am dead: " + gameObject);
            GameManager.Instance.Events.OnFollowerKilled(this);
        }

    }

    private void StartFighting()
    {
        if (isFollowing)
        {
            isAttacking = true;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (!isFollowing)
            {
                StartFollowing();
                FollowDistance = GameManager.Instance.Player.transform.position - transform.position - (Vector3.forward * 0.5f);
            }
        }
        else if (collision.transform.tag == "Boss")
        {
            //GameManager.Instance.Boss.Health -= AttackPower;
            //Health -= GameManager.Instance.Boss.AttackPower;
            GameManager.Instance.Events.BossAttacked(AttackPower);
            isInAttackRange = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Boss")
        {
            isInAttackRange = false;
            attackTimer = 0f;
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (!isFollowing)
            {
                StartFollowing();

                FollowDistance = GameManager.Instance.Player.transform.position - transform.position - (Vector3.forward * 0.5f);
            }
        }
    }
    */

    public void StartFollowing()
    {
        GameManager.Instance.Events.FollowerGained(this);

        isFollowing = true;
        Renderer renderer = GetComponent<MeshRenderer>();
        renderer.material.color = FollowingColor;
    }

    private void Update()
    {
        if (isAttacking)
        {
            if (!isInAttackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.Levels.CurrentLevel.Boss.transform.position + FollowDistance, FollowSpeed * Time.deltaTime);
            }
            else
            {
                attackTimer -= Time.deltaTime;

                if (attackTimer < 0f)
                {
                    GameManager.Instance.Events.BossAttacked(AttackPower);
                    attackTimer = AttackRate;
                }
            }
        }

        if (isFollowing && !isAttacking)
            transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.Player.transform.position + FollowDistance, FollowSpeed * Time.deltaTime);

        if (Health <= 0)
        {
            Debug.Log("I am dead: " + gameObject);
            GameManager.Instance.Events.OnFollowerKilled(this);
        }
    }

}
