using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IDTable : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _textList = new List<TextMeshProUGUI>();
    private GameBoard _gameBoard;

    private void Start()
    {
        _gameBoard = FindObjectOfType<GameBoard>();
    }

    void Update()
    {
        var table = _gameBoard._gameBoardElementTable;
        for (int y = 0; y < table.Length; y++)
        {
            for (int x = 0; x < table[y].Length; x++)
            {
                int index = _gameBoard.GetIndex(x, y);
                _textList[index].text = table[y][x].ID.ToString();
                if (table[y][x].ID == -1)
                {
                    _textList[index].color = new Color32(255, 0, 0, 255);
                }
                else
                {
                    _textList[index].color = new Color32(0, 0, 0, 255);
                }
            }
        }
    }
}
