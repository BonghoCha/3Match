using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemoveTable : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _textList = new List<TextMeshProUGUI>();
    private int[] _numberList = new int[10];

    public void AddNumber(int index)
    {
//        _numberList[index]++;
//        _textList[index].text = _numberList[index].ToString();
    }
    
    public void RemoveNumber(int index)
    {
        _numberList[index]--;
        _textList[index].text = _numberList[index].ToString();
    }
}
