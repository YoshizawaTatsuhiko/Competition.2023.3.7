using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MazeCreaterExtend))]

class MazeGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("SIZE が 80を超えると重くなる")]
    private int _size = 5;
    public int Width { get => _size < 5 ? 5 : _size; }
    public int Height { get => _size < 5 ? 5 : _size; }

    [SerializeField]
    private GameObject _wall = null;
    [SerializeField]
    private GameObject _path = null;
    /// <summary>生成するときの通路の座標</summary>
    private float _pathHeight = 0f;
    [SerializeField]
    private GameObject _start = null;
    [SerializeField]
    private GameObject _goal = null;
    [SerializeField]
    private GameObject _gimic = null;
    [SerializeField]
    private GameObject _ceiling = null;
    /// <summary>屋根を生成する座標(迷路を生成してから屋根をかぶせるため)</summary>
    private Vector3 _ceilingPos = Vector3.zero;
    [SerializeField]
    private GameObject _enemySpawner = null;
    private Dictionary<string, GameObject> _objDic = new Dictionary<string, GameObject>();
    /// <summary>作成した迷路</summary>
    private MazeCreaterExtend _maze = null;
    /// <summary>迷路の設計図</summary>
    private string[,] _blueprint;
    /// <summary>任意のイベントを起こす座標を入れるリスト</summary>
    List<(int, int)> _coordinateList = new List<(int, int)>();

    private void Awake()
    {
        if (!FindObjectOfType<GameManager>()) GenerateMazeFromBlueprint();
    }

    public void GenerateMazeFromBlueprint()
    {
        _objDic.Add("W", _wall);
        _objDic.Add("F", _path);
        _objDic.Add("S", _start);
        _objDic.Add("G", _goal);
        _objDic.Add("C", _gimic);
        _objDic.Add("E", _enemySpawner);

        _maze = GetComponent<MazeCreaterExtend>();
        string[] mazeInfo = _maze.GenerateMaze(Width, Height).Split("\n");
        _blueprint = new string[mazeInfo[0].Length, mazeInfo.Length - 1];
        To2DArray(mazeInfo, _blueprint);

        _coordinateList = FindMazePoint(_blueprint, "W", 3);

        (int, int) point = SetSpotRandom(_blueprint, _coordinateList, "S");
        FindFurthestPoint(_blueprint, point, _coordinateList, "G");

        SetSpotRandom(_blueprint, _coordinateList, "C");
        SetSpotRandom(_blueprint, _coordinateList, "C");
        SetSpotRandom(_blueprint, _coordinateList, "E");
        SetSpotRandom(_blueprint, _coordinateList, "E");
        SetSpotRandom(_blueprint, _coordinateList, "E");
        SetSpotRandom(_blueprint, _coordinateList, "E");
        FindFurthestPoint(_blueprint, point, _coordinateList, "E");
        FindFurthestPoint(_blueprint, point, _coordinateList, "E");

        GameObject wallParent = new GameObject("Wall Parent");
        GameObject floorParent = new GameObject("Floor Parent");
        GameObject otherParent = new GameObject("Other Parent");

        _pathHeight = Vector3.zero.y + -_wall.transform.localScale.y / 2f;

        for (int i = 0; i < _blueprint.GetLength(1); i++)
        {
            for (int j = 0; j < _blueprint.GetLength(0); j++)
            {
                string chara = _blueprint[j, i];

                if (_objDic.ContainsKey(chara))
                {
                    if (chara == "W")
                        Instantiate(_objDic[chara],
                            new Vector3(j - Width / 2, 0f, i - Height / 2), Quaternion.identity, wallParent.transform);
                    else if (chara == "F")
                        Instantiate(_objDic[chara] == null ? _path : _objDic[chara],
                                new Vector3(j - Width / 2, _pathHeight, i - Height / 2), Quaternion.identity, floorParent.transform);
                    else
                        Instantiate(_objDic[chara] == null ? _path : _objDic[chara],
                            new Vector3(j - Width / 2, _pathHeight, i - Height / 2), Quaternion.identity, otherParent.transform);
                }
            }
        }
        _ceilingPos.y = Vector3.zero.y + _wall.transform.localScale.y / 2f;
        GameObject ceiling = Instantiate(_ceiling == null ? new GameObject() : _ceiling, _ceilingPos, Quaternion.identity, otherParent.transform);
        ceiling.transform.localScale = new Vector3(Width, 0.1f, Height);
    }

    /// <summary>一次元配列を二次元配列に変換する(string型限定)</summary>
    /// <param name="array">string型の一次元配列</param>
    /// <param name="twoDimensionalArray">string型の二次元配列</param>
    private void To2DArray(string[] array, string[,] twoDimensionalArray)
    {
        for (int i = 0; i < twoDimensionalArray.GetLength(0); i++)
        {
            for (int j = 0; j < twoDimensionalArray.GetLength(1); j++)
            {
                twoDimensionalArray[i, j] = array[i][j].ToString();
            }
        }
    }

    #region Event Method

    /// <summary>隣接した文字を検索して、条件に合致した座標をリストアップする</summary>
    /// <param name="conditionChar">知りたい座標に隣接する文字</param>
    /// <param name="conditionCount">隣接する、条件となる文字の個数</param>
    /// <returns>条件に合致した座標を格納したリスト「(int, int)型」</returns>
    private List<(int, int)> FindMazePoint(string[,] blueprint, string conditionChar, int conditionCount)
    {
        // 条件に合致した座標を格納するリスト
        List<(int, int)> coordinateList = new List<(int, int)>();
        if (conditionCount < 0) conditionCount = 0;
        if (conditionCount > 4) conditionCount = 4;

        // i * j == 奇数の場所の中から、条件に合致する場所を検索する。
        for (int i = 1; i < blueprint.GetLength(1) - 1; i += 2)
        {
            for (int j = 1; j < blueprint.GetLength(0) - 1; j += 2)
            {
                // 隣接する4方向のどれかが[conditionChar]だったら、カウントする。
                int count = 0;

                if (blueprint[i, j - 1] == conditionChar) count++;
                if (blueprint[i, j + 1] == conditionChar) count++;
                if (blueprint[i - 1, j] == conditionChar) count++;
                if (blueprint[i + 1, j] == conditionChar) count++;

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
    private void FindFurthestPoint(string[,] blueprint, (int, int) point, List<(int, int)> coordinateList, string chara)
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
        blueprint[tapple.Item1, tapple.Item2] = chara;
        coordinateList.Remove(tapple);
    }

    /// <summary>任意の文字をランダムな座標に配置する</summary>
    /// <param name="coordinateList">文字を配置する候補座標のリスト</param>
    /// <param name="chara">配置する文字</param>
    private (int, int) SetSpotRandom(string[,] blueprint, List<(int, int)> coordinateList, string chara)
    {
        (int, int) tapple = (0, 0);

        // 候補座標のリストの要素にGUIDを一時的に割り当てて、ソートする。
        // GUIDの値はランダムなので、要素の順番がバラバラになる。
        foreach ((int, int) p in coordinateList.OrderBy(_ => System.Guid.NewGuid()))
        {
            blueprint[p.Item1, p.Item2] = chara;
            tapple = p;
            coordinateList.Remove((p.Item1, p.Item2));
            break;
        }
        return tapple;
    }

    #endregion
}
