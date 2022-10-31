using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>生成する度に変化するマップを生み出す</summary>
public class RandomMapController : MonoBehaviour
{
    /// <summary>壁生成開始地点候補</summary>
    List<(int, int)> _startPoint = new List<(int, int)>();
    /// <summary>ゴール地点候補の座標を格納する</summary>
    List<(int, int)> _goalPosition = new List<(int, int)>();

    /// <summary>マップを自動生成する</summary>
    /// <param name="width">横幅</param>
    /// <param name="height">縦幅</param>
    public string GenerateMap(int width, int height)
    {
        //渡された値が 5未満だったら、エラーを返す
        if (width < 5 || height < 5) throw new System.ArgumentOutOfRangeException();
        //渡された値が奇数なら、偶数にして返す
        int w = width % 2 == 0 ? width : width - 1;
        int h = height % 2 == 0 ? height : height - 1;

        //マップの外周を壁にし、その内側を全て通路で埋める。
        string[,] maze = new string[w, h];
        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
            {
                maze[i, j] = i * j == 0 || i == w - 1 || j == h - 1 ? "W" : "F";
                //偶数番目のマスを壁生成開始地点候補に追加する
                if (i % 2 == 0 && j % 2 == 0) { _startPoint.Add((i, j)); }
            }
        ExtendWall(maze, _startPoint);
        SetSpot(maze, "S");
        SetSpot(maze, "G");
        return ArrayToString(maze);
    }

    /// <summary>壁をランダムな方向に伸ばす</summary>
    /// <param name="map">マップの大きさ</param>
    /// <param name="startPoints">壁生成開始地点候補のリスト</param>
    void ExtendWall(string[,] map, List<(int, int)> startPoints)
    {
        //壁生成開始地点候補から、ランダムに選択する
        int startIndex = Random.Range(0, startPoints.Count);
        //壁生成開始地点をセットする
        int x = startPoints[startIndex].Item1;
        int y = startPoints[startIndex].Item2;
        //壁生成開始地点のリストから、候補だった座標を削除する
        startPoints.RemoveAt(startIndex);

        while(true)
        {
            //壁を伸ばす方向の候補をリストに格納する
            List<string> direction = new List<string>();
            //壁を伸ばせるかを確認する
            if (map[x, y - 1] == "F" && map[x, y - 2] == "F") direction.Add("UP");
            if (map[x, y + 1] == "F" && map[x, y + 2] == "F") direction.Add("DOWN");
            if (map[x - 1, y] == "F" && map[x - 2, y] == "F") direction.Add("LEFT");
            if (map[x + 1, y] == "F" && map[x + 2, y] == "F") direction.Add("RIGHT");
            //壁を伸ばす方向をランダムで決める
            int dirIndex = Random.Range(0, direction.Count);
            switch (direction[dirIndex])
            {
                case "UP":
                    WallInstallation(map, x, y--);
                    WallInstallation(map, x, y--);
                    break;
                case "DOWN":
                    WallInstallation(map, x, y++);
                    WallInstallation(map, x, y++);
                    break;
                case "LEFT":
                    WallInstallation(map, x--, y);
                    WallInstallation(map, x--, y);
                    break;
                case "RIGHT":
                    WallInstallation(map, x++, y);
                    WallInstallation(map, x++, y);
                    break;
            }
            //壁を延長する
            map[x, y] = "W";
            //壁を延長できる方向がなくなったら、ループを抜ける
            if (direction.Count == 0) break;
        }
    }

    /// <summary>壁を設置する</summary>
    void WallInstallation(string[,] map, int x, int y)
    {
        map[x, y] = "W";
        // x. y が共に偶数だったら、壁生成開始地点候補から削除する
        if (x % 2 == 0 && y % 2 == 0) _startPoint.Remove((x, y));
    }

    /// <summary>ランダムな行き止まりに、特定の文字を設置する</summary>
    /// <param name="maze">マップの大きさ</param>
    /// <param name="str">特定の文字</param>
    void SetSpot(string[,] maze, string str)
    {
        foreach (var point in _goalPosition.
            OrderBy(_ => _goalPosition.Count).Where(p => maze[p.Item1, p.Item2] == "F"))
        {
            //3方向が壁になっている場所を探す
            int count = 0;
            if (maze[point.Item1, point.Item2 - 1] == "W") count++;
            if (maze[point.Item1, point.Item2 + 1] == "W") count++;
            if (maze[point.Item1 - 1, point.Item2] == "W") count++;
            if (maze[point.Item1 + 1, point.Item2] == "W") count++;

            if(count == 3)
            {
                maze[point.Item1, point.Item2] = str;
                Debug.Log($"{str} = Complete");
                break;
            }
        }
    }

    /// <summary>迷路を文字列にして表示する</summary>
    /// <param name="maze">迷路の全長</param>
    /// <returns>文字列化した迷路</returns>
    string ArrayToString(string[,] maze)
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
