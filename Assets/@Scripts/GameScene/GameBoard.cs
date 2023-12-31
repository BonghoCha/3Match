using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{
    [Header("-- Options --")] 
    [SerializeField] private int XAxisLength;
    [SerializeField] private int YAxisLength;
    
    [Header("-- Game Board --")]
    [SerializeField] private List<GameBoardElement> _gameBoardElementList;
    [SerializeField] public GameBoardElement[][] _gameBoardElementTable;
    private List<CommonDefinition.Point> _removeList = new List<CommonDefinition.Point>();

    private List<int> _idList = new List<int>() { 1, 2, 3, 4, 5 };
    private List<Color32> _colorList = new List<Color32>()
    {
        new Color32(200, 50, 50, 255),
        new Color32(50, 200, 50, 255),
        new Color32(50, 50, 200, 255),
        new Color32(200, 200, 50, 255),
        new Color32(50, 200, 200, 255)
    };
    
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _gameBoardElementTable = new GameBoardElement[YAxisLength][];
        for (int y = 0; y < YAxisLength; y++)
        {
            _gameBoardElementTable[y] = new GameBoardElement[XAxisLength];
            for (int x = 0; x < XAxisLength; x++)
            {
                Generate(x, y);
            }
        }
        
        for (int y = 0; y < YAxisLength; y++)
        {
            for (int x = 0; x < XAxisLength; x++)
            {
                if (_gameBoardElementTable[y][x].ID == -1) continue;
                IsMatch(x, y);
            }
        }
    }

    private void Generate(int x, int y)
    {
        var index = GetIndex(x, y);
        var id = _idList[Random.Range(0, 5)];
        _gameBoardElementTable[y][x] = _gameBoardElementList[index];
        _gameBoardElementTable[y][x].SetInfo(x, y, id);
        _gameBoardElementTable[y][x].SetColor(GetColor(id - 1));
        _gameBoardElementTable[y][x].gameObject.SetActive(true);
    }

    public void Swap(GameBoardElement a, GameBoardElement b, Action onCallback = null)
    {
        SwapInfo(a, b);

        a.Move(onCallback);
        b.Move();

    }

    public void SwapInfo(GameBoardElement a, GameBoardElement b)
    {
        var pointA = a.Point;
        var pointB = b.Point;

        var aX = pointA.x;
        var aY = pointA.y;
        
        var bX = pointB.x;
        var bY = pointB.y;
        a.SetInfo(bX,bY);
        b.SetInfo(aX, aY);

        (_gameBoardElementTable[pointA.y][pointA.x], _gameBoardElementTable[pointB.y][pointB.x]) = (_gameBoardElementTable[pointB.y][pointB.x], _gameBoardElementTable[pointA.y][pointA.x]);
    }
    
    public GameBoardElement GetElement(int x, int y)
    {
        return _gameBoardElementTable[y][x];
    }
    
    public GameBoardElement GetElement(CommonDefinition.Point point)
    {
        return _gameBoardElementTable[point.y][point.x];
    }

    public int GetIndex(int x, int y)
    {
        return (XAxisLength * y) + x;
    }

    public bool IsMatch(int x, int y)
    {
        int id = _gameBoardElementTable[y][x].ID;
        
        if (id == -1) return false;
        
        bool left = true;
        bool right = true;
        bool up = true;
        bool down = true;

        List<CommonDefinition.Point> leftRemoveList = new List<CommonDefinition.Point>();
        List<CommonDefinition.Point> rightRemoveList = new List<CommonDefinition.Point>();
        List<CommonDefinition.Point> upRemoveList = new List<CommonDefinition.Point>();
        List<CommonDefinition.Point> downRemoveList = new List<CommonDefinition.Point>();

        int count = 1;
        while (left || right || up || down)
        {
            if (left)
            {
                if (IsSameLine(x, y, x - count, y))
                {
                    GameBoardElement target = GetElement(x - count, y);
                    if (target == null || !isValid(x - count, y) || target.ID != id || target.ID == -1)
                    {
                        left = false;
                    }
                    else
                    {
                        if (target.ID == id)
                        {
                            leftRemoveList.Add(new CommonDefinition.Point(x - count, y));
                        }
                    }
                }
            }

            if (right)
            {
                if (IsSameLine(x, y, x + count, y))
                {
                    GameBoardElement target = GetElement(x + count, y);
                    if (target == null || !isValid(x + count, y) || target.ID != id || target.ID == -1)
                    {
                        right = false;
                    }
                    else
                    {
                        if (target.ID == id)
                        {
                            rightRemoveList.Add(new CommonDefinition.Point(x + count, y));
                        }
                    }
                }
            }

            if (up)
            {
                if (IsSameLine(x, y, x, y - count))
                {
                    GameBoardElement target = GetElement(x, y - count);
                    if (target == null || !isValid(x, y - count) || target.ID != id || target.ID == -1)
                    {
                        up = false;
                    }
                    else
                    {
                        if (target.ID == id)
                        {
                            upRemoveList.Add(new CommonDefinition.Point(x, y - count));
                        }
                    }
                }
            }

            if (down)
            {
                if (IsSameLine(x, y, x, y + count))
                {
                    GameBoardElement target = GetElement(x, y + count);
                    if (target == null || !isValid(x, y + count) || target.ID != id || target.ID == -1)
                    {
                        down = false;
                    }
                    else
                    {
                        if (target.ID == id)
                        {
                            downRemoveList.Add(new CommonDefinition.Point(x, y + count));
                        }
                    }
                }
            }

            count++;

            if (count >= XAxisLength && count >= YAxisLength) break;
        }

        if (leftRemoveList.Count + rightRemoveList.Count >= 2)
        {
            _removeList.AddRange(leftRemoveList);
            _removeList.AddRange(rightRemoveList);
        }
        if (upRemoveList.Count + downRemoveList.Count >= 2)
        {
            _removeList.AddRange(upRemoveList);
            _removeList.AddRange(downRemoveList);
        }
        
        if (_removeList.Count > 0)
        {
            _removeList.Add(_gameBoardElementTable[y][x].Point);
        }
        
        for (int i = 0; i < _removeList.Count; i++)
        {
            var removeTarget = GetElement(_removeList[i]);
            removeTarget.SetColor(new Color32(0, 0, 0, 255));
            removeTarget.ID = -1;
            removeTarget.gameObject.SetActive(false);

            FindObjectOfType<RemoveTable>().AddNumber(GetIndex(_removeList[i].x, _removeList[i].y));
        }
        _removeList.Clear();

        SortBlocks();
        return false;
    }

    public void SortBlocks()
    {
        for (int x = 0; x < XAxisLength; x++)
        {
            int num = 0;
            for (int y = YAxisLength - 1; y >= 0; y--)
            {
                if (_gameBoardElementTable[y][x].ID == -1)
                {
                    num++;
                    continue;
                }
                _gameBoardElementTable[y][x].MoveY(num);
                _gameBoardElementTable[y][x].SetInfo(x, y + num);
                _gameBoardElementTable[y + num][x].SetInfo(x, y);
                (_gameBoardElementTable[y][x], _gameBoardElementTable[y + num][x]) = (_gameBoardElementTable[y + num][x], _gameBoardElementTable[y][x]);  
            }
        }
    }

    public void Regenerate()
    {
        for (int x = 0; x < XAxisLength; x++)
        {
            int num = 0;
            for (int y = YAxisLength - 1; y >= 0; y--)
            {
                if (_gameBoardElementTable[y][x].ID == -1)
                {
                    Generate(x, y);
                }
            }
        }
    }

    private bool IsSameLine(int x1, int y1, int x2, int y2)
    {
        if (x1 < 0 || x1 >= XAxisLength || x2 < 0 || x2 >= XAxisLength) return false;
        if (y1 < 0 || y1 >= YAxisLength || y2 < 0 || y2 >= YAxisLength) return false;
        
        if (x1 % XAxisLength == x2 % XAxisLength || y1 % YAxisLength == y2 % YAxisLength)
        {
            return true;
        }

        return false;
    }

    private bool isValid(int x, int y)
    {
        if (x < 0 || x >= XAxisLength) return false;
        if (y < 0 || y >= YAxisLength) return false;
        
        return true;
    }

    public Color32 GetColor(int id)
    {
        Debug.Log(id);
        return _colorList[id];
    }
}
