using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    List<(int, int)> _startPoint = new List<(int, int)>();
    List<(int, int)> _goalPoint = new List<(int, int)>();
    /// <summary>マップをランダム生成する</summary>
    /// <param name="width">横幅</param>
    /// <param name="height">縦幅</param>
    public void CreateMap(int width, int height)
    {
        //渡された値が奇数なら、偶数にして返す
        int w = width % 2 == 0 ? width : width - 1;
        int h = height % 2 == 0 ? height : height - 1;

        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
            {

            }
    }
}
