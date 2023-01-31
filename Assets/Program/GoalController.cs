using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class GoalController : MonoBehaviour
{
    private BoxCollider _boxCol = null;
    public bool GoalJudge { get; set; }

    private void Start()
    {
        _boxCol = GetComponent<BoxCollider>();
        _boxCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GoalJudge = true;
            Debug.Log("Call");
        }
        else
        {
            GoalJudge = false;
        }
    }
}
