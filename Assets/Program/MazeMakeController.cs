using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>生成する度に変化するマップを生み出す</summary>
public class MazeMakeController : MonoBehaviour
{
    /// <summary>迷路を生成する</summary>
    public void GenerateMaze(int width, int height)
    {
        if (width < 5 || height < 5) throw new System.ArgumentOutOfRangeException();
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;
    }

    /// <summary>迷路を文字列にして表示する</summary>
    /// <param name="maze">迷路の全長</param>
    /// <returns>文字列化した迷路</returns>
    private string ArrayToString(string[,] maze)
    {
        string str = "";

        for(int i = 0; i < maze.Length; i++)
        {
            for (int j = 0; j < maze.Length; j++) str += maze[i, j];
            if (i < maze.Length - 1) str += "\n";
        }
        Debug.Log(str);
        return str;
    }
}
