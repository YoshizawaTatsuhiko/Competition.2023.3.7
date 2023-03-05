using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickCheck : MonoBehaviour
{
    [SerializeField]
    private float _checkRange = 1f;

    public int GimmickWakeUpCount { get; private set; }

    public void GimmickJudgement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _checkRange))
            {
                if (hit.collider.gameObject.TryGetComponent(out GimmickController gimmick))
                {
                    GimmickWakeUpCount += gimmick.GimmickWakeUp();
                }
                else
                {
                    Debug.Log("Useless Act");
                }
            }
        }
    }
}
