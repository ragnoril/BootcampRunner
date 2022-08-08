using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAgent : MonoBehaviour
{
    public bool isAttacking;
    public Transform Target;
    public float MoveSpeed;

    public int AttackPower;
    public int Health;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Events.OnFightStarted += StartFighting;
    }

    private void OnDestroy()
    {
        GameManager.Instance.Events.OnFightStarted -= StartFighting;
    }

    private void StartFighting()
    {
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
            if (Vector3.Distance(transform.position, Target.position) > 0.1f)
                transform.position = Vector3.MoveTowards(transform.position, Target.position, MoveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            GameManager.Instance.Player.Health -= AttackPower;
            Health -= GameManager.Instance.Player.AttackPower;
        }
    }


}
