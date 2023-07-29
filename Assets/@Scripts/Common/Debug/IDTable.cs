using System;
using System.Collections;
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
        var table = _gameBoard._gameTable;
        for (int i = 0; i < table.Count; i++)
        {
            _textList[i].text = table[i].ToString();
            if (table[i] == -1)
            {
                _textList[i].color = new Color32(255, 0, 0, 255);
            }
        }
    }
}
