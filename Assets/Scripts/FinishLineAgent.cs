using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineAgent : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameManager.Instance.Events.FightSetup();
        }
    }
}
