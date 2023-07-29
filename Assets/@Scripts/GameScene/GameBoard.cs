using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{
    [Header("-- Options --")] 
    [SerializeField] private int XAxisLength;
    [SerializeField] private int YAxisLength;
    
    [Header("-- Game Board --")]
    [SerializeField] private List<GameBoardElement> _gameBoardElementList;

    public List<int> _gameTable = new List<int>();
    private List<int> _idList = new List<int>() { 1, 2, 3, 4, 5 };
    
    [SerializeField] private List<Color32> _colorList = new List<Color32>()
    {
        new Color32(200, 50, 50, 255),
        new Color32(50, 200, 50, 255),
        new Color32(50, 50, 200, 255),
        new Color32(200, 200, 50, 255),
        new Color32(50, 200, 200, 255)
    };
    
    private Dictionary<CommonDefinition.Point, GameBoardElement> _gameBoardElementDic = new Dictionary<CommonDefinition.Point, GameBoardElement>();


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int y = 0; y < YAxisLength; y++)
        {
            for (int x = 0; x < XAxisLength; x++)
            {
                var index = GetIndex(x, y);
                var id = Random.Range(0, 5);
                _gameBoardElementList[index].SetInfo(x, y, _idList[id]);
                _gameBoardElementList[index].SetColor(_colorList[id]);
                _gameTable.Add(id);
            }
        }
        
        for (int y = 0; y < YAxisLength; y++)
        {
            for (int x = 0; x < XAxisLength; x++)
            {
                int index = GetIndex(x, y);
                if (_gameTable[index] == -1) continue;
                IsMatch(x, y);
            }
        }
    }

    public void Swap(GameBoardElement a, GameBoardElement b, Action onCallback = null)
    {
        var pointA = a.GetPoint();
        var pointB = b.GetPoint();

        int aX = pointA.x;
        int aY = pointA.y;
        
        int bX = pointB.x;
        int bY = pointB.y;
        
        GameBoardElement tempElement = _gameBoardElementList[GetIndex(aX, aY)];
        _gameBoardElementList[GetIndex(aX, aY)] = _gameBoardElementList[GetIndex(bX, bY)];
        _gameBoardElementList[GetIndex(bX, bY)] = tempElement;
        
        b.SetInfo(aX, aY);
        a.SetInfo(bX, bY);
        
        a.Move(b.GetPosition(), onCallback);
        b.Move(a.GetPosition());

        int index1 = GetIndex(aX, aY);
        int index2 = GetIndex(bX, bY);
        (_gameTable[index1], _gameTable[index2]) = (_gameTable[index2], _gameTable[index1]);
    }
    
    public GameBoardElement GetElement(int x, int y)
    {
        var index = GetIndex(x, y);
        return _gameBoardElementList[index];
    }

    private int GetIndex(int x, int y)
    {
        return (XAxisLength * y) + x;
    }

    public bool Check(GameBoardElement element1, GameBoardElement element2, CommonEnum.Direction direction)
    {
        int id = element1.GetID();
        
        int x1 = element1.GetPoint().x;
        int y1 = element1.GetPoint().y;
            
        int x2 = element2.GetPoint().x;
        int y2 = element2.GetPoint().y;
        
        if (direction == CommonEnum.Direction.HORIZONTAL)
        {
            for (int i = x1 - 2; i <= x2 + 2; i++)
            {
                if (i == x1 || i == x2) continue;
                if (!isValid(i, y1)) continue;
                if (!isSameLine(x1, y1, i, y1)) continue;

                int nextID = GetElement(i, y1).GetID();
                if (id == nextID)
                {
                    return true;
                }
            }
        } 
        else if (direction == CommonEnum.Direction.VERTICAL)
        {
            for (int i = y1 - 2; i <= y2 + 2; i++)
            {
                if (i == y1 || i == y2) continue;
                if (!isValid(x1, i)) continue;
                if (!isSameLine(x1, y1, x1, i)) continue;
                
                int nextID = GetElement(x1, i).GetID();
                if (id == nextID)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsMatch(int x, int y)
    {
        int index = GetIndex(x, y);
        int id = _gameTable[index];
        
        bool left = true;
        bool right = true;
        bool up = true;
        bool down = true;

        List<int> leftRemoveList = new List<int>();
        List<int> rightRemoveList = new List<int>();
        List<int> upRemoveList = new List<int>();
        List<int> downRemoveList = new List<int>();

        int count = 1;
        while (left || right || up || down)
        {
            if (left)
            {
                int leftIndex = isSameLine(x, y, x - count, y) ? GetIndex(x - count, y) : -1;
                if (leftIndex == -1 || !isValid(x - count, y) || _gameTable[leftIndex] != id)
                {
                    left = false;
                }
                else
                {
                    Debug.Log(_gameTable[leftIndex] + "," + id);
                    if (_gameTable[leftIndex] == id)
                    {
                        leftRemoveList.Add(leftIndex);
                    }
                }
            }

            if (right)
            {
                int rightIndex = isSameLine(x, y, x + count, y) ? GetIndex(x + count, y) : -1;
                if (rightIndex == -1 || !isValid(x + count, y)|| _gameTable[rightIndex] != id)
                {
                    right = false;
                }
                else
                {
                    if (_gameTable[rightIndex] == id)
                    {
                        rightRemoveList.Add(rightIndex);
                    }
                }
            }

            if (up)
            {
                int upIndex = isSameLine(x, y, x, y - count) ? GetIndex(x, y - count) : -1;
                if (upIndex == -1 || !isValid(x, y - count)|| _gameTable[upIndex] != id)
                {
                    up = false;
                }
                else
                {
                    if (_gameTable[upIndex] == id)
                    {
                        upRemoveList.Add(upIndex);
                    }
                }
            }

            if (down)
            {
                int downIndex = isSameLine(x, y, x, y + count) ? GetIndex(x, y + count) : -1;
                if (downIndex == -1 || !isValid(x, y + count)|| _gameTable[downIndex] != id)
                {
                    down = false;
                }
                else
                {
                    if (_gameTable[downIndex] == id)
                    {
                        downRemoveList.Add(downIndex);
                    }
                }
            }

            count++;
        }

        List<int> removeList = new List<int>();
        if (leftRemoveList.Count + rightRemoveList.Count >= 2)
        {
            removeList.AddRange(leftRemoveList);
            removeList.AddRange(rightRemoveList);
        }
        if (upRemoveList.Count + downRemoveList.Count >= 2)
        {
            removeList.AddRange(upRemoveList);
            removeList.AddRange(downRemoveList);
        }
        
        if (removeList.Count > 0)
        {
            removeList.Add(index);
        }
        Debug.Log(removeList.Count);
        for (int i = 0; i < removeList.Count; i++)
        {
            _gameBoardElementList[removeList[i]].SetColor(new Color32(0, 0, 0, 255));
            _gameTable[removeList[i]] = -1;
        }
        return false;
    }

    private bool isSameLine(int x1, int y1, int x2, int y2)
    {
        if (x1 % XAxisLength == x2 % XAxisLength || y1 % YAxisLength == y2 % YAxisLength)
        {
            return true;
        }

        return false;
    }

    private bool isValid(int x, int y)
    {
        int index = GetIndex(x, y);
        if (index < 0 || index >= _gameBoardElementList.Count) return false;
        
        return true;
    }
}
