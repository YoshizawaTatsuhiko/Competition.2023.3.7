using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>生成する度に変化するマップを生み出す</summary>
public class RandomMapController : MonoBehaviour
{
    /// <summary>壁生成開始地点候補</summary>
    List<(int, int)> _startPoint = new List<(int, int)>();
    List<(int, int)> _goalPosition = new List<(int, int)>();

    /// <summary>マップを自動生成する</summary>
    /// <param name="width">横幅</param>
    /// <param name="height">縦幅</param>
    public void GenerateMap(int width, int height)
    {
        //渡された値が 5未満だったら、エラーを返す
        if (width <= 5 || height <= 5) throw new System.ArgumentOutOfRangeException();
        //渡された値が奇数なら、偶数にして返す
        int w = width % 2 == 0 ? width : width - 1;
        int h = height % 2 == 0 ? height : height - 1;

        //マップの外周を壁にし、その内側を通路にする。
        string[,] maze = new string[w, h];
        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
            {                
                maze[i, j] = i * j == 0 || i == w - 1 || j == h - 1 ? "W" : "F";  //"W"を壁に、"F"を床にする
                if (i % 2 == 0 && j % 2 == 0) { _startPoint.Add((i, j)); }  //偶数番目のマスを壁生成開始地点候補に追加する
            }
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
        //壁生成開始地点のリストから、削除する
        startPoints.RemoveAt(startIndex);

        while(true)
        {
            //壁を伸ばす方向の候補をリストに格納する
            List<string> direction = new List<string>();
            if (map[x, y - 1] == "F" && map[x, y - 2] == "F") direction.Add("UP");
            if (map[x, y + 1] == "F" && map[x, y + 2] == "F") direction.Add("DOWN");
            if (map[x - 1, y] == "F" && map[x - 2, y] == "F") direction.Add("LEFT");
            if (map[x + 1, y] == "F" && map[x + 2, y] == "F") direction.Add("RIGHT");
            //壁を延長できる方向がなくなったら、ループを抜ける
            if (direction.Count == 0) break;
            //壁を延長する
            map[x, y] = "W";
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
        }
    }

    /// <summary>壁を設置する</summary>
    void WallInstallation(string[,] map, int x, int y)
    {
        map[x, y] = "W";
        if (x * y % 2 == 0) _startPoint.Remove((x, y));  // x. y が共に偶数だったら、壁生成開始地点候補から削除する
    }
}
