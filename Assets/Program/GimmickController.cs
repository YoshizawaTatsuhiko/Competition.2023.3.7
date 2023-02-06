using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickController : MonoBehaviour
{
    private bool _isOnce = true;

    public int GimmickWakeUp()
    {
        if (_isOnce)
        {
            Debug.Log("Called Only Once");
            if (gameObject.TryGetComponent(out Renderer r)) r.material.color = Color.red;
            _isOnce = false;
            return 1;
        }
        return 0;
    }
}
