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

    private void Start()
    {
        GameManager.Instance.Events.OnFightStarted += StartFighting;
    }
    private void OnDestroy()
    {
        GameManager.Instance.Events.OnFightStarted -= StartFighting;
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
            GameManager.Instance.Boss.Health -= AttackPower;
            Health -= GameManager.Instance.Boss.AttackPower;
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
            if (Vector3.Distance(transform.position, GameManager.Instance.Boss.transform.position) > 0.1f)
                transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.Boss.transform.position + FollowDistance, FollowSpeed * Time.deltaTime);
        }

        if (isFollowing && !isAttacking)
            transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.Player.transform.position + FollowDistance, FollowSpeed * Time.deltaTime);

        if (Health < 0)
        {
            GameManager.Instance.Events.OnFollowerKilled(this);
        }
    }

}
