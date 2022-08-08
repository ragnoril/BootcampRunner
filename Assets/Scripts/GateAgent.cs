using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GATETYPE
{
    Add,
    Multiply,
    Minus,
    Divide
}

public class GateAgent : MonoBehaviour
{
    public GATETYPE GateType;
    public int Amount;

    public TMPro.TMP_Text GateText;

    private void Start()
    {
        switch (GateType)
        {
            case GATETYPE.Add:
                GateText.text = "+ ";
                break;
            case GATETYPE.Multiply:
                GateText.text = "* ";
                break;
            case GATETYPE.Minus:
                GateText.text = "- ";
                break;
            case GATETYPE.Divide:
                GateText.text = "% ";
                break;
            default:
                break;
        }

        GateText.text += Amount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameManager.Instance.Events.PlayerPassThruGate(this);
        }
    }
}
