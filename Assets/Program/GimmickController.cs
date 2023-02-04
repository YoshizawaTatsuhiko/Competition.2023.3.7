using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickController : MonoBehaviour
{
    public void GimmickWakeUp(System.Action action)
    {
        bool isOnce = true;

        if (isOnce)
        {
            action();
            isOnce = false;
        }
    }
}
