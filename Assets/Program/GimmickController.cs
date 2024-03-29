﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickController : MonoBehaviour
{
    /// <summary>一度しか起動されないようにするための制御フロー変数</summary>
    private bool _isOnce = true;

    /// <summary>ギミックが起動したかどうかをint型で返す</summary>
    /// <returns>起動 => 1 | 未起動 or 起動済み => 0</returns>
    public int GimmickWakeUp()
    {
        if (_isOnce)
        {
            if (gameObject.TryGetComponent(out Renderer r)) r.material.color = Color.red;
            _isOnce = false;
            return 1;
        }
        return 0;
    }
}
