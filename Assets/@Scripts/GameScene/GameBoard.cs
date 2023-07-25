using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{
    [Header("-- Options --")] 
    [SerializeField] private int XAxisLength;
    [SerializeField] private int YAxisLength;
    [SerializeField] private List<GameBoardElement> _gameBoardElementList;

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
                _gameBoardElementList[index].SetInfo(x, y);
            }
        }
    }
    
    public GameBoardElement GetElement(int x, int y)
    {
        var index = (XAxisLength * y) + x;
        return _gameBoardElementList[index];
    }

    public void Swap(GameBoardElement a, GameBoardElement b)
    {
        var pointA = a.GetPoint();
        var pointB = b.GetPoint();

        int aX = pointA.x;
        int aY = pointA.y;
        
        int bX = pointB.x;
        int bY = pointB.y;
        
        var temp = _gameBoardElementList[GetIndex(aX, aY)];
        _gameBoardElementList[GetIndex(aX, aY)] = _gameBoardElementList[GetIndex(bX, bY)];
        _gameBoardElementList[GetIndex(bX, bY)] = temp;
        
        b.SetInfo(aX, aY);
        a.SetInfo(bX, bY);
        
        a.Move(b.GetPosition());
        b.Move(a.GetPosition());
    }

    private int GetIndex(int x, int y)
    {
        return (XAxisLength * y) + x;
    }
}
