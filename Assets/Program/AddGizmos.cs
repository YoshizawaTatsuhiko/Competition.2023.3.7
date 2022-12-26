using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参考資料
//https://github.com/code-beans/GizmoExtensions/blob/master/src/GizmosExtensions.cs

namespace MyGizmos
{
    /// <summary>新しいGizmosを追加する</summary>
    public static class AddGizmos
    {
        public static void DrawWireCone(Vector3 coneTip,
            Vector3 direction, float radius, Quaternion rotation, float segments = 20f)
        {
            //Gizmosの行列を保存しておく
            Matrix4x4 mat = Gizmos.matrix;
            float angle = Mathf.PI * 2 * Mathf.Rad2Deg;
            //if (rotation == default) rotation = Quaternion.identity;

            //Gizmosの行列を変換する
            Gizmos.matrix = Matrix4x4.TRS(direction, rotation, Vector3.one);

            //図形を描き始める地点
            Vector3 from = direction * radius;
            //Gizmos.DrawLine(coneTip, from);

            //増加量を求める
            int addition = Mathf.RoundToInt(angle / segments);
            

            for (int i = 0; i <= angle; i += addition)  //円を描く
            {
                float cos = radius * Mathf.Cos(i * Mathf.Deg2Rad);
                Vector3 to = new Vector3(cos, radius * Mathf.Sin(i * Mathf.Deg2Rad), 0f);
                Gizmos.DrawLine(from, to);
                Gizmos.DrawLine(coneTip, to);
                from = to;
            }

            //Gizmosの行列を元に戻す
            Gizmos.matrix = mat;
        }
    }
}
