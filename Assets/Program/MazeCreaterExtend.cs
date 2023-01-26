using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>生成する度に構造が変化するマップを生成する</summary>
public class MazeCreaterExtend : MonoBehaviour
{
    /// <summary>壁生成開始地点</summary>
    List<(int, int)> _startPoint = new List<(int, int)>();
    /// <summary>拡張中の壁の情報を格納する</summary>
    Stack<(int, int)> _currentWall = new Stack<(int, int)>();
    /// <summary>任意のイベントを起こす座標を入れるリスト</summary>
    List<(int, int)> _coordinateList = new List<(int, int)>();

    #region Maze Generation Algorithm

    /// <summary>迷路を生成する</summary>
    public string GenerateMaze(int width, int height)
    {
        // 縦横の大きさが5未満だったら生成しない。
        if (width < 5 || height < 5) throw new System.ArgumentOutOfRangeException();
        // 縦(横)の値が偶数だったら、奇数に変換する。
        width = width % 2 == 0 ? width + 1 : width;
        height = height % 2 == 0 ? height + 1 : height;

        // 迷路の情報を格納する。
        string[,] maze = new string[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // 外周を壁で囲む。
                if (x * y == 0 || x == width - 1 || y == height - 1)
                {
                    maze[x, y] = "W";
                }
                // 外周以外は床で埋める。
                else
                {
                    maze[x, y] = "F";
                    // 壁生成開始座標候補をリストに追加する。
                    if (x % 2 == 0 && y % 2 == 0)
                    {
                        _startPoint.Add((x, y));
                    }
                }
            }
        }
        ExtendWall(maze, _startPoint);
        _coordinateList = FindCoordinate(maze);
        (int, int) point = SetSpotRandom(maze, _coordinateList, "S");
        FindFurthestPoint(maze, point, _coordinateList, "G");
        SetSpotRandom(maze, _coordinateList, "C");
        SetSpotRandom(maze, _coordinateList, "C");

        return ArrayToString(maze);
    }

    /// <summary>壁を拡張する</summary>
    private void ExtendWall(string[,] maze, List<(int, int)> startPoint)
    {
        int index = Random.Range(0, startPoint.Count);
        int x = startPoint[index].Item1;
        int y = startPoint[index].Item2;
        startPoint.RemoveAt(index);
        bool isFloor = true;

        while (isFloor)
        {
            // 拡張できる方向を格納するリスト
            List<string> dirs = new List<string>();

            if (maze[x, y - 1] == "F" && !IsCurrentWall(x, y - 2)) dirs.Add("Up");
            if (maze[x, y + 1] == "F" && !IsCurrentWall(x, y + 2)) dirs.Add("Down");
            if (maze[x - 1, y] == "F" && !IsCurrentWall(x - 2, y)) dirs.Add("Left");
            if (maze[x + 1, y] == "F" && !IsCurrentWall(x + 2, y)) dirs.Add("Right");
            // 拡張する方向が見つからなかったら、ループを抜ける
            if (dirs.Count == 0) break;
            // 壁を設置する
            SetWall(maze, x, y);
            int dirsIndex = Random.Range(0, dirs.Count);
            try
            {
                switch (dirs[dirsIndex])
                {
                    case "Up":
                        isFloor = maze[x, y - 2] == "F";
                        SetWall(maze, x, --y);
                        SetWall(maze, x, --y);
                        break;
                    case "Down":
                        isFloor = maze[x, y + 2] == "F";
                        SetWall(maze, x, ++y);
                        SetWall(maze, x, ++y);
                        break;
                    case "Left":
                        isFloor = maze[x - 2, y] == "F";
                        SetWall(maze, --x, y);
                        SetWall(maze, --x, y);
                        break;
                    case "Right":
                        isFloor = maze[x + 2, y] == "F";
                        SetWall(maze, ++x, y);
                        SetWall(maze, ++x, y);
                        break;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        // 拡張できるポイントがまだあったら拡張を続ける。
        if (startPoint.Count > 0)
        {
            _currentWall.Clear();
            ExtendWall(maze, startPoint);
        }
    }

    /// <summary>壁を設置する</summary>
    private void SetWall(string[,] maze, int x, int y)
    {
        maze[x, y] = "W";
        // x, yが共に偶数だったら、リストから削除し、スタックに格納する。
        if (x % 2 == 0 && y % 2 == 0)
        {
            _startPoint.Remove((x, y));
            _currentWall.Push((x, y));
        }
    }

    /// <summary>拡張中の壁かどうか判定する</summary>
    /// <returns>true -> 拡張中 | false -> 拡張済</returns>
    private bool IsCurrentWall(int x, int y)
    {
        return _currentWall.Contains((x, y));
    }

    #endregion

    #region Event Method

    /// <summary>3方向が壁になっている座標を見つける</summary>
    /// <returns>3方向が壁になっている座標のリスト「(int, int)型」</returns>
    private List<(int, int)> FindCoordinate(string[,] maze)
    {
        // 条件に合致した座標を格納するリスト
        List<(int, int)> coordinateList = new List<(int, int)>();

        // アルゴリズムの都合上、i * j == 奇数の場所しか条件に合う座標は存在しないので奇数番目の座標のみ検索する。
        for (int i = 1; i < maze.GetLength(1); i += 2)
        {
            for (int j = 1; j < maze.GetLength(0); j += 2)
            {
                // 隣接する4方向のどれかが壁だったら、カウントする。
                int count = 0;

                if (maze[i, j - 1] == "W") count++;
                if (maze[i, j + 1] == "W") count++;
                if (maze[i - 1, j] == "W") count++;
                if (maze[i + 1, j] == "W") count++;

                if (count == 3)
                {
                    coordinateList.Add((i, j));
                }
            }
        }
        return coordinateList;
    }

    /// <summary>特定の座標から最も遠い座標を見つけ、文字を配置する</summary>
    /// <param name="point">基準となる座標</param>
    /// <param name="coordinateList">文字を配置する候補座標のリスト</param>
    /// <param name="chara">配置する文字</param>
    private void FindFurthestPoint(string[,] maze, (int, int) point, List<(int, int)> coordinateList, string chara)
    {
        if (coordinateList.Count == 0)
        {
            Debug.LogWarning("候補地点が見つかりませんでした。");
            return;
        }

        int max = int.MinValue;
        (int, int) tapple = (0, 0);

        foreach ((int, int) n in coordinateList)
        {
            // ピタゴラスの定理を使って、最も遠い座標を検索する。
            int distance = 
                (n.Item1 - point.Item1) * (n.Item1 - point.Item1) + (n.Item2 - point.Item2) * (n.Item2 - point.Item2);

            // 最も遠い座標の暫定1位を更新していく。
            if(max < distance)
            {
                max = distance;
                tapple = n;
            }
        }
        // 特定の座標に文字を配置したら、その座標をリストから削除する。
        // これにより、同じリスト使っている限り、上書きされることは無くなる。
        maze[tapple.Item1, tapple.Item2] = chara;
        coordinateList.Remove(tapple);
    }

    /// <summary>任意の文字をランダムな座標に配置する</summary>
    /// <param name="coordinateList">文字を配置する候補座標のリスト</param>
    /// <param name="chara">配置する文字</param>
    private (int, int) SetSpotRandom(string[,] maze, List<(int, int)> coordinateList, string chara)
    {
        (int, int) tapple = (0, 0);

        // 候補座標のリストの要素にGUIDを一時的に割り当てて、ソートする。
        // GUIDの値はランダムなので、要素の順番がバラバラになる。
        foreach ((int, int) p in coordinateList.OrderBy(_ => System.Guid.NewGuid()))
        {
            maze[p.Item1, p.Item2] = chara;
            tapple = p;
            coordinateList.Remove((p.Item1, p.Item2));
            break;
        }
        return tapple;
    }

    #endregion

    /// <summary>迷路を文字列にして表示する</summary>
    /// <param name="maze">迷路</param>
    /// <returns>文字列化した迷路</returns>
    private string ArrayToString(string[,] maze)
    {
        string str = "";

        for(int i = 0; i < maze.GetLength(1); i++)
        {
            for (int j = 0; j < maze.GetLength(0); j++)
            {
                str += maze[i, j];
            }
            if (i < maze.Length - 1) str += "\n";
        }
        Debug.Log(str);
        return str;
    }
}
