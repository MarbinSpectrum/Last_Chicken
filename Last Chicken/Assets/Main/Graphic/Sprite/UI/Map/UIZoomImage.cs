using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIZoomImage : MonoBehaviour,IScrollHandler
{
    //Make sure these values are evenly divisible by scaleIncrement
    [SerializeField] float _minimumScale = 0.5f;
    [SerializeField] float _initialScale = 1f;
    [SerializeField] float _maximumScale = 3f;
    /////////////////////////////////////////////
    [SerializeField] float _scaleIncrement = .5f;
    /////////////////////////////////////////////

    [HideInInspector] Vector3 _scale;

    RectTransform _thisTransform;

    [SerializeField] Image zoomPoint;
    [SerializeField] float _zoomSpeed = .5f;
    private void Awake()
    {

        _thisTransform = transform as RectTransform;

        _scale.Set(_initialScale, _initialScale, 1f);
        _thisTransform.localScale = _scale;

    }

    public void Update()
    {
        if (KeyManager.nowController == GameController.KeyBoard)
        {
            zoomPoint.transform.position = Input.mousePosition;
            zoomPoint.enabled = false;
        }
        else
        {
            if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.SystemDown]))
                zoomPoint.transform.position -= new Vector3(0, _zoomSpeed, 0);
            if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.SystemUp]))
                zoomPoint.transform.position += new Vector3(0, _zoomSpeed, 0);
            if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.SystemLeft]))
                zoomPoint.transform.position -= new Vector3(_zoomSpeed, 0, 0);
            if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.SystemRight]))
                zoomPoint.transform.position += new Vector3(_zoomSpeed, 0, 0);
            if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.Select]))
                ChangeSize(1);
            else if (KeyManager.GetKey(KeyManager.instance.gamePad[GameKeyType.Cancle]))
                ChangeSize(-1);

            zoomPoint.enabled = true;
        }
    }

    private void OnEnable()
    {
        zoomPoint.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    public void OnScroll(PointerEventData eventData)
    {
        float delta = eventData.scrollDelta.y;

        ChangeSize(delta);
    }

    private void ChangeSize(float delta)
    {
        Vector2 relativeMousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, zoomPoint.transform.position, null, out relativeMousePosition);

        if (delta > 0 && _scale.x < _maximumScale)
        {   //zoom in

            _scale.Set(_scale.x + _scaleIncrement, _scale.y + _scaleIncrement, 1f);
            _thisTransform.localScale = _scale;
            _thisTransform.anchoredPosition -= (relativeMousePosition * _scaleIncrement);
        }

        else if (delta < 0 && _scale.x > _minimumScale)
        {   //zoom out

            _scale.Set(_scale.x - _scaleIncrement, _scale.y - _scaleIncrement, 1f);
            _thisTransform.localScale = _scale;
            _thisTransform.anchoredPosition += (relativeMousePosition * _scaleIncrement);
        }
    }
}
