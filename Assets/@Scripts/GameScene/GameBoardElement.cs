using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameBoardElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    public class Point
    {
        public int x;
        public int y;

        public void SetInfo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    [SerializeField] private Image _background;
    [SerializeField] private Image _highlight;
    private Color _color;
    
    private Vector2 _touchPoint;
    private float _minMagnitude = 20f;

    private bool _isClicked = false;
    public bool IsMoving = false;

    [SerializeField] private Point _point = new Point();

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
    
    public void SetInfo(int x, int y)
    {
        _point.SetInfo(x, y);
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

    public Point GetPoint()
    {
        return _point;
    }
}
