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
    private Color _color;
    
    private Vector2 _touchPoint;
    private float _minMagnitude = 20f;

    [SerializeField] private Point _point = new Point();

    public void SetInfo(int x, int y)
    {
        _point.SetInfo(x, y);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _touchPoint = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
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
            Swap(next);
        }
    }
    
    public void Test()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_background.DOColor(new Color(0, 0, 255), 1f));
        sequence.Append(_background.DOColor(new Color(255, 0, 0), 1f));
        sequence.Play();
    }

    public Vector2 GetPosition()
    {
        return GetComponent<RectTransform>().localPosition;
    }

    public void Swap(GameBoardElement element)
    {
        int x1 = element._point.x;
        int y1 = element._point.y;
        element.SetInfo(_point.x, _point.y);
        SetInfo(x1, y1);
        
        Move(element.GetPosition());
        element.Move(GetPosition());
    }
    
    public void Move(Vector2 pos)
    {
        GetComponent<RectTransform>().DOAnchorPos(pos, 0.5f);
    }

    public void SetColor(Color color)
    {
        _background.color = color;
    }
}
