using GameField.Items;
using UnityEngine;
using System;
using GameInterface;
using GameLogic;
using GameField;

public class InputHandler : MonoBehaviour
{
    private Vector3 _firstMousePosition;
    private bool _isClick;
    private bool _isDrag;
    private const float OffsetScroll = 0.2f;
    private bool _blockClick;
    
    public static Action<Item> OnClickItem;

    private void OnEnable()
    {
        PauseController.OnBlockClick += DoOnBlockClick;
        PauseController.OffBlockClick += DoOffBlockClick;
        GameStateController.OnBlockClick += DoOnBlockClick;
    }

    private void OnDisable()
    {
        PauseController.OnBlockClick -= DoOnBlockClick;
        PauseController.OffBlockClick -= DoOffBlockClick;
        GameStateController.OnBlockClick -= DoOnBlockClick;
    }

    private void DoOnBlockClick()
    {
        _blockClick = true;
    }

    private void DoOffBlockClick()
    {   
        _blockClick = false;
    }

    private void Update()
    {
        if (_blockClick) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _firstMousePosition = mousePos;
            _isClick = true;
            _isDrag = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isClick = false;

            var hit = Physics2D.Raycast(_firstMousePosition, Vector2.zero);

            if (!_isDrag && 
                hit.collider != null && 
                hit.collider.TryGetComponent<Item>(out var item))
            {
                OnClickItem?.Invoke(item);
            }
        }

        if (_isClick)
        {
            CheckMoveMouse();
        }
    }

    private void CheckMoveMouse()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CheckScroll(Math.Abs(mousePos.y - _firstMousePosition.y) >= OffsetScroll);
        CheckScroll(Math.Abs(mousePos.x - _firstMousePosition.x) >= OffsetScroll);
    }

    private void CheckScroll(bool isScroll)
    {
        if (isScroll)
        {
            _isClick = false;
            _isDrag = true;
        }
    }
}
