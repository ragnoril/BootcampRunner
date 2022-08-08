using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    public bool isRunning;
    public bool isGettingReadyToFight;
    public float runningSpeed;
    public float strifeSpeed;

    private Rigidbody rBody;

    private Vector3 swipeStartPosition, swipeEndPosition;
    private float swipeStartTime;

    private float swipeTime, swipeDistance;
    public float MaxSwipeTime, MinSwipeDistance;

    public int AttackPower;
    public int Health;
    public bool isAttacking;
    public bool isInAttackRange;
    public float AttackRate;
    private float attackTimer;

    public List<FollowerAgent> Followers;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
        Followers = new List<FollowerAgent>();

        GameManager.Instance.Events.OnFollowerGained += GainNewFollower;
        GameManager.Instance.Events.OnFollowerKilled += KillFollower;
        GameManager.Instance.Events.OnPlayerPassThruGate += ManageGateEffect;
        GameManager.Instance.Events.OnFightSetup += SetupForFight;
        GameManager.Instance.Events.OnFightStarted += StartFighting;
        GameManager.Instance.Events.OnFollowerAttacked += FollowerHit;
        GameManager.Instance.Events.OnPlayerAttacked += GetHit;

        GameManager.Instance.Events.OnLevelStarted += LevelStarted;
        Init();
    }

    private void Init()
    {
        isInAttackRange = false;
        attackTimer = 0f;

        isRunning = true;
        isGettingReadyToFight = false;
        isAttacking = false;
        
    }

    private void OnDestroy()
    {
        GameManager.Instance.Events.OnFollowerGained -= GainNewFollower;
        GameManager.Instance.Events.OnFollowerKilled -= KillFollower;
        GameManager.Instance.Events.OnPlayerPassThruGate -= ManageGateEffect;
        GameManager.Instance.Events.OnFightSetup -= SetupForFight;
        GameManager.Instance.Events.OnFightStarted -= StartFighting;
        GameManager.Instance.Events.OnFollowerAttacked -= FollowerHit;
        GameManager.Instance.Events.OnPlayerAttacked -= GetHit;

        GameManager.Instance.Events.OnLevelStarted -= LevelStarted;
    }

    public void LevelStarted()
    {
        foreach (FollowerAgent follower in Followers)
            Destroy(follower.gameObject);

        Followers.Clear();

        transform.position = GameManager.Instance.Levels.CurrentLevel.LevelPlayerStartPoint.position;
        Health = 20 + (GameManager.Instance.Levels.CurrentLevelId * 10);
        Init();
    }

    private void GetHit(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            isAttacking = false;
            GameManager.Instance.Events.LevelFinished(false);
        }
    }

    private void FollowerHit(FollowerAgent follower, int damage)
    {
        //Debug.Log("follower " + follower.name + " dmg: " + damage);
        follower.GetHit(damage);
    }

    private void KillFollower(FollowerAgent follower)
    {
        Destroy(follower.gameObject);
        Followers.Remove(follower);
    }

    private void StartFighting()
    {
        isAttacking = true;
    }

    private void SetupForFight()
    {
        isRunning = false;
        isGettingReadyToFight = true;
    }

    private void ManageGateEffect(GateAgent gate)
    {
        switch(gate.GateType)
        {
            case GATETYPE.Add:
                SpawnFollowers(gate.Amount);
                break;
            case GATETYPE.Multiply:
                SpawnFollowers((gate.Amount - 1) * Followers.Count);
                break;
            case GATETYPE.Minus:
                RemoveFollowers(gate.Amount);
                break;
            case GATETYPE.Divide:
                RemoveFollowers(Followers.Count - (Followers.Count / gate.Amount));
                break;
            default:
                break;
        }
    }

    private void RemoveFollowers(int amount)
    {
        int newFollowerAmount = Followers.Count - amount;
        if (newFollowerAmount < 0) newFollowerAmount = 0;
        int lastIndex = Followers.Count - 1;
        for (int i = lastIndex; i >= newFollowerAmount; i--)
        {
            GameManager.Instance.Events.FollowerKilled(Followers[i]);
        }
    }

    private void SpawnFollowers(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(GameManager.Instance.FollowerPrefab, transform.position, Quaternion.identity);
            FollowerAgent follower = go.GetComponent<FollowerAgent>();
            follower.StartFollowing();
            follower.FollowDistance = transform.position - follower.transform.position - (Vector3.forward * 1.5f) - (Vector3.right * UnityEngine.Random.Range(-0.2f,0.2f) * i);
        }
    }

    private void GainNewFollower(FollowerAgent follower)
    {
        if (!Followers.Contains(follower))
        {
            Followers.Add(follower);
        }
    }

    private void FixedUpdate()
    {
        // (x,y,z) (0, 0, 1)
        if (!isRunning)
        {
            rBody.velocity = Vector3.zero;
            rBody.angularVelocity = Vector3.zero;
            return;
        }
        rBody.velocity = Vector3.forward * runningSpeed * Time.fixedDeltaTime;

        float moveX = 0f;
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartPosition = Input.mousePosition;
            swipeStartTime = Time.time;
        }
        else if (Input.GetMouseButton(0))
        {
            swipeEndPosition = Input.mousePosition;
            //swipeEndTime = Time.time;
            swipeDistance = (swipeEndPosition - swipeStartPosition).magnitude;
            swipeTime = Time.time - swipeStartTime;

            if (swipeTime < MaxSwipeTime && swipeDistance > MinSwipeDistance)
            {
                Vector2 swipe = swipeEndPosition - swipeStartPosition;

                moveX = swipe.x;
            }
        }

        moveX = Mathf.Clamp(moveX, -1f, 1f);

        Vector3 vel = rBody.velocity;
        vel.x = moveX * strifeSpeed * Time.fixedDeltaTime;
        rBody.velocity = vel;

    }

    private void Update()
    {
        if (isGettingReadyToFight)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.Levels.CurrentLevel.LevelPlayerFightStartPoint.position, 0.01f * runningSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, GameManager.Instance.Levels.CurrentLevel.LevelPlayerFightStartPoint.position) < 0.1f)
            {
                isGettingReadyToFight = false;
                GameManager.Instance.Events.FightStarted();
            }
        }

        if (isAttacking)
        {
            if (!isInAttackRange)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, GameManager.Instance.Levels.CurrentLevel.Boss.transform.position, 0.01f * runningSpeed * Time.deltaTime);
                newPos.y = transform.position.y;
                transform.position = newPos;
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



    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Boss")
        {
            isInAttackRange = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Boss")
        {
            isInAttackRange = false;
            attackTimer = 0f;
        }
    }
}
