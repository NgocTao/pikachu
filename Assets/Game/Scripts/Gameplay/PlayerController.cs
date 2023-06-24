using System.Collections;
using System.Collections.Generic;
using StormStudio.Common.UI;
using UnityEngine;

public interface IDragListener
{

}

public class PlayerController : MonoBehaviour
{
    IGameController _gameController;

    public void Setup(IGameController gameController)
    {
        _gameController = gameController;
    }
    public void Reset()
    {
    }

    void Update()
    {
        switch (InputHelper.Instance.GetTouchPhase())
        {
            case TouchPhase.Began:
                MoveTouch(InputHelper.Instance.GetTouchPosition());
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Ended:
                break;
        }
    }

    public void MoveTouch(Vector2 pos)
    {
        if (_gameController.IsOver || _gameController.IsPause) return;

        if (!UIManager.Instance.IsUIInteractable) return;

        var objTouched = getGObjectAtPosition(pos);
        if (objTouched != null)
        {
            var cell = objTouched.GetComponent<Cell>();
            if (cell != null)
            {
                _gameController.OnClickCell(cell);
            }

        }
    }

    #region Utils
    GameObject getGObjectAtPosition(Vector2 pos)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(pos);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Cell"));
        if (hit.collider != null)
        {
            Debug.Log("collider: " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }
        return null;
    }
    #endregion
}
