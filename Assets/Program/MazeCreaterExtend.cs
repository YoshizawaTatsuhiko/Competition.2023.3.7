using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>生成する度に変化するマップを生み出す</summary>
public class MazeCreaterExtend : MonoBehaviour
{
    /// <summary>迷路を生成する</summary>
    public string GenerateMaze(int width, int height)
    {
        if (width < 5 || height < 5) throw new System.ArgumentOutOfRangeException();
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;

        string[,] maze = new string[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x * y == 0 || x == width - 1 || y == height - 1)
                {
                    maze[x, y] = "W";
                }
                else
                {
                    maze[x, y] = "F";
                }
            }
        }
        return ArrayToString(maze);
    }

    /// <summary>迷路を文字列にして表示する</summary>
    /// <param name="maze">迷路</param>
    /// <returns>文字列化した迷路</returns>
    private string ArrayToString(string[,] maze)
    {
        string str = "";

        for(int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                str += maze[i, j];
            }
            if (i < maze.Length - 1) str += "\n";
        }
        Debug.Log(str);
        return str;
    }
}
