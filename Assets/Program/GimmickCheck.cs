using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickCheck : MonoBehaviour
{
    [SerializeField]
    private float _checkRange = 1f;
    [SerializeField]
    private GameObject _gimmickObj = null;

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _checkRange))
            {
                if (hit.collider.gameObject.TryGetComponent(out GimmickController gimmick))
                {
                    gimmick.GimmickWakeUp(() =>
                    {
                        Debug.Log("Called Only Once");
                    });
                }
                else
                {
                    Debug.Log("Useless Act");
                }
            }
        }
    }
}
