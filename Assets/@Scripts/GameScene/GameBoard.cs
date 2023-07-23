using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
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
                var index = (XAxisLength * y) + x;
                _gameBoardElementList[index].SetInfo(x, y);
            }
        }
    }
    
    public GameBoardElement GetElement(int x, int y)
    {
        var index = (XAxisLength * y) + x;
        return _gameBoardElementList[index];
    }

    public void Test(int x, int y)
    {
        var element = GetElement(x, y);
        element.Test();
    }
}
