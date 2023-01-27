using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MazeCreaterExtend))]

class MazeGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("MAX SIZE = 32")]
    private int _size = 5;
    public int Width { get => _size < 5 ? 5 : _size; }
    public int Height { get => _size < 5 ? 5 : _size; }

    [SerializeField, Tooltip("迷路を形成するobject")]
    private GameObject[] _go = null;
    /// <summary>作成した迷路</summary>
    private MazeCreaterExtend _maze = null;
    /// <summary>迷路の設計図</summary>
    private string[,] _bluePrint;
    /// <summary>任意のイベントを起こす座標を入れるリスト</summary>
    List<(int, int)> _coordinateList = new List<(int, int)>();

    private void Awake()
    {
        _maze = GetComponent<MazeCreaterExtend>();
        string[] mazeInfo = _maze.GenerateMaze(Width, Height).Split("\n");
        _bluePrint = new string[mazeInfo[0].Length, mazeInfo.Length - 1];
        To2DArray(mazeInfo, _bluePrint);

        _coordinateList = FindMazePoint(_bluePrint, "W", 3);

        (int, int) point = SetSpotRandom(_bluePrint, _coordinateList, "S");
        FindFurthestPoint(_bluePrint, point, _coordinateList, "G");

        SetSpotRandom(_bluePrint, _coordinateList, "C");
        SetSpotRandom(_bluePrint, _coordinateList, "C");

        GameObject wallParent = new GameObject("Wall Parent");
        GameObject floorParent = new GameObject("Floor Parent");
        GameObject otherParent = new GameObject("Other Parent");

        for (int i = 0; i < _bluePrint.GetLength(0); i++)
        {
            for (int j = 0; j < _bluePrint.GetLength(1); j++)
            {
                if (_bluePrint[i, j] == "W") Instantiate(_go[0], 
                    new Vector3(i - Width / 2, 0f, j - Height / 2), Quaternion.identity, wallParent.transform);
                if (_bluePrint[i, j] == "F") Instantiate(_go[1], 
                    new Vector3(i - Width / 2, 0f, j - Height / 2), Quaternion.identity, floorParent.transform);
                if (_bluePrint[i, j] == "S") Instantiate(_go[2],
                    new Vector3(i - Width / 2, 0f, j - Height / 2), Quaternion.identity, otherParent.transform);
                if (_bluePrint[i, j] == "G") Instantiate(_go[3],
                    new Vector3(i - Width / 2, 0f, j - Height / 2), Quaternion.identity, otherParent.transform);
                if (_bluePrint[i, j] == "C") Instantiate(_go[4],
                    new Vector3(i - Width / 2, 0f, j - Height / 2), Quaternion.identity, otherParent.transform);
            }
        }
    }

    /// <summary>一次元配列を二次元配列に変換する(string型限定)</summary>
    /// <param name="array">string型の一次元配列</param>
    /// <param name="twoDimensionalArray">string型の二次元配列</param>
    private string[,] To2DArray(string[] array, string[,] twoDimensionalArray)
    {
        for (int i = 0; i < twoDimensionalArray.GetLength(0); i++)
        {
            for (int j = 0; j < twoDimensionalArray.GetLength(1); j++)
            {
                twoDimensionalArray[i, j] = array[i][j].ToString();
            }
        }
        return twoDimensionalArray;
    }

    #region Event Method

    /// <summary>隣接した文字を検索して、条件に合致した座標をリストアップする</summary>
    /// <param name="conditionChar">知りたい座標に隣接する文字</param>
    /// <param name="conditionCount">隣接する、条件となる文字の個数</param>
    /// <returns>条件に合致した座標を格納したリスト「(int, int)型」</returns>
    private List<(int, int)> FindMazePoint(string[,] bluePrint, string conditionChar, int conditionCount)
    {
        // 条件に合致した座標を格納するリスト
        List<(int, int)> coordinateList = new List<(int, int)>();
        if (conditionCount < 0) conditionCount = 0;
        if (conditionCount > 4) conditionCount = 4;

        // アルゴリズムの都合上、i * j == 奇数の場所しか条件に合う座標は存在しないので奇数番目の座標のみ検索する。
        for (int i = 1; i < bluePrint.GetLength(1) - 1; i += 2)
        {
            for (int j = 1; j < bluePrint.GetLength(0) - 1; j += 2)
            {
                // 隣接する4方向のどれかが壁だったら、カウントする。
                int count = 0;

                if (bluePrint[i, j - 1] == conditionChar) count++;
                if (bluePrint[i, j + 1] == conditionChar) count++;
                if (bluePrint[i - 1, j] == conditionChar) count++;
                if (bluePrint[i + 1, j] == conditionChar) count++;

                if (count == conditionCount)
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
    private void FindFurthestPoint(string[,] bluePrint, (int, int) point, List<(int, int)> coordinateList, string chara)
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
            if (max < distance)
            {
                max = distance;
                tapple = n;
            }
        }
        // 特定の座標に文字を配置したら、その座標をリストから削除する。
        // これにより、同じリスト使っている限り、上書きされることは無くなる。
        bluePrint[tapple.Item1, tapple.Item2] = chara;
        coordinateList.Remove(tapple);
    }

    /// <summary>任意の文字をランダムな座標に配置する</summary>
    /// <param name="coordinateList">文字を配置する候補座標のリスト</param>
    /// <param name="chara">配置する文字</param>
    private (int, int) SetSpotRandom(string[,] bluePrint, List<(int, int)> coordinateList, string chara)
    {
        (int, int) tapple = (0, 0);

        // 候補座標のリストの要素にGUIDを一時的に割り当てて、ソートする。
        // GUIDの値はランダムなので、要素の順番がバラバラになる。
        foreach ((int, int) p in coordinateList.OrderBy(_ => System.Guid.NewGuid()))
        {
            bluePrint[p.Item1, p.Item2] = chara;
            tapple = p;
            coordinateList.Remove((p.Item1, p.Item2));
            break;
        }
        return tapple;
    }

    #endregion
}
