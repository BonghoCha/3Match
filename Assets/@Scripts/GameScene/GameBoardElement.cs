using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameBoardElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _highlight;
    [SerializeField] private TextMeshProUGUI _idText;
    private Color _color;
    
    private Vector2 _touchPoint;
    private float _minMagnitude = 20f;

    private bool _isClicked = false;
    public bool IsMoving = false;

    private int _identity = -1;

    [SerializeField] private CommonDefinition.Point _point = new CommonDefinition.Point();

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isClicked) return;
        if (IsMoving) return;
        
        _isClicked = true;
        
        _touchPoint = eventData.position;
        
        _highlight.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isClicked) return;
        if (IsMoving) return;
        
        var touchPoint = eventData.position;

        var dir = (touchPoint - _touchPoint).normalized;
        var mag = (touchPoint - _touchPoint).magnitude;

        var nextPos = transform.localPosition;
        var x = 0;
        var y = 0;
        if (mag >= _minMagnitude)
        {
            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    x = 1;
                    nextPos += new Vector3(50, 0);
                }
                else
                {
                    x = -1;
                    nextPos += new Vector3(-50, 0);
                }
            } 
            else
            {
                if (dir.y > 0)
                {
                    y = -1;
                    nextPos += new Vector3(0, 50);
                }
                else
                {
                    y = 1;
                    nextPos += new Vector3(0, -50);
                }
            }
            
            //GetComponent<RectTransform>().DOAnchorPos(nextPos, 0.5f);
            
            var board = FindObjectOfType<GameBoard>();
            
            var next = board.GetElement(_point.x + x, _point.y + y);
            if (!next.IsMoving)
            {
                board.Swap(this, next);    
            }
        }

        _highlight.gameObject.SetActive(false);
        _isClicked = false;
    }
    
    public void SetInfo(int x, int y, int id = -1)
    {
        _point.SetInfo(x, y);

        if (id != -1)
        {
            _identity = id;
            _idText.text = id.ToString();
        }
    }
    
    public void SetColor(Color color)
    {
        _background.color = color;
    }

    public void Move(Vector2 pos)
    {
        IsMoving = true;
        GetComponent<RectTransform>().DOAnchorPos(pos, 0.5f).OnComplete(() =>
        {
            IsMoving = false;
        });
    }
    
    public Vector2 GetPosition()
    {
        return GetComponent<RectTransform>().localPosition;
    }

    public CommonDefinition.Point GetPoint()
    {
        return _point;
    }

    public int GetID()
    {
        return _identity;
    }
}