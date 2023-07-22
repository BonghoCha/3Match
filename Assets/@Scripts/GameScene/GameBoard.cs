using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private List<GameBoardElement> _gameBoardElementList;

    public GameBoardElement GetElement(int x, int y)
    {
        return _gameBoardElementList[y * x];
    }
}
