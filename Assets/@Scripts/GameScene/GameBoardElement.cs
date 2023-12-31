using System;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class GameBoardElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region ## Component ##

    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private Image _background;
    [SerializeField] private Image _highlight;
    [SerializeField] private TextMeshProUGUI _idText;
    private RectTransform _rectTransform;

    #endregion

    #region ## Property ##

    private int _id = -1;
    public int ID
    {
        get => _id;
        set => _id = value;
    }

    private CommonDefinition.Point _point = new CommonDefinition.Point();
    public CommonDefinition.Point Point
    {
        get
        {
            if (_point == null)
            {
                _point = new CommonDefinition.Point();
            }
            return _point;
        }
        set => _point = value;
    }
    
    #endregion
    
    private Color _color;
    private float _width;
    private float _height;
    
    private Vector2 _touchPoint;
    private bool _isClicked = false;
    public bool IsMoving = false;
    
    private float _minMagnitude = 20f;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (_gameBoard == null)
        {
            _gameBoard = FindObjectOfType<GameBoard>();
        }
        
        if (_background == null)
        {
            _background = Util.Find<Image>(transform, "Background");
        }

        if (_highlight == null)
        {
            _highlight = Util.Find<Image>(transform, "Highlight");
        }

        if (_idText == null)
        {
            _idText = Util.Find<TextMeshProUGUI>(transform, "IDText");
        }
        
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        _width = _rectTransform.sizeDelta.x;
        _height = _rectTransform.sizeDelta.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanPressDown()) return;
        
        _isClicked = true;
        _touchPoint = eventData.position;
        _highlight.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CanPressUp()) return;

        var touchPoint = eventData.position;

        var dir = (touchPoint - _touchPoint).normalized;
        var mag = (touchPoint - _touchPoint).magnitude;

        var x = 0;
        var y = 0;
        if (mag >= _minMagnitude)
        {
            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            {
                if (dir.x > 0) x = 1;
                else x = -1;
            } 
            else
            {
                if (dir.y > 0) y = -1;
                else y = 1;
            }
            
            var board = FindObjectOfType<GameBoard>();
            
            var next = board.GetElement(Point.x + x, Point.y + y);
            if (next.ID == -1)
            {
                _highlight.gameObject.SetActive(false);
                _isClicked = false;
                return;
            }
            
            if (!next.IsMoving)
            {
                board.Swap(this, next, () =>
                {
                    board.IsMatch(Point.x, Point.y);
                    board.IsMatch(Point.x - x, Point.y - y);
                });
            }
        }

        _highlight.gameObject.SetActive(false);
        _isClicked = false;
    }
    
    public void SetInfo(int x, int y, int id = -1)
    {
        Point.x = x;
        Point.y = y;

        if (id != -1)
        {
            ID = id;
            _idText.text = id.ToString();
        }
    }
    
    public void SetColor(Color color)
    {
        _background.color = color;
    }

    public void Move(Action onCallback = null)
    {
        var targetPos = new Vector2(_width * Point.x, _height * -Point.y);
        IsMoving = true;
        _rectTransform.DOAnchorPos(targetPos, 0.5f).OnComplete(() =>
        {
            IsMoving = false;
            if (onCallback != null)
            {
                onCallback();
            }
        });
    }

    [Tooltip("test")]
    public void Test()
    {
        MoveY(-1);
    }
    
    public void MoveY(int num, Action onCallback = null)
    {
        IsMoving = true;

        var targetPos = GetNewPosition(0, num);
        _rectTransform.DOAnchorPosY(targetPos.y, 0.5f).OnComplete(() =>
        {
            IsMoving = false;
        });
    }
    
    public Vector2 GetPosition()
    {
        return GetComponent<RectTransform>().localPosition;
    }

    public Vector2 GetNewPosition(int x, int y)
    {
        Point += new CommonDefinition.Point(x, y);
        var targetPos = new Vector2(_width * (Point.x), _height * -Point.y);

        return targetPos;
    }
    
    public bool CanPressDown()
    {
        if (_isClicked || _id == -1 || IsMoving) return false;
        
        return true;
    }

    public bool CanPressUp()
    {
        if (!_isClicked || IsMoving) return false;

        return true;
    }
}
