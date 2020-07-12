using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputController : MonoBehaviour
{
    const string CLICK_COLLIDER_LAYER = "Click";
    const float MAX_DISTANCE = 2000f;


    public static event Action onGameWorldClicked;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var hitUI = EventSystem.current.IsPointerOverGameObject();

            if (!hitUI)
            {
                Piece hitPiece = null;

                var mask = LayerMask.GetMask(CLICK_COLLIDER_LAYER);
                var hit = Physics2D.Raycast(
                    MainCamera.c.ScreenToWorldPoint(Input.mousePosition),
                    Vector2.zero, MAX_DISTANCE, mask
                );

                hitPiece = hit.transform?.GetComponentInParent<Piece>();

                if (hitPiece != null) hitPiece.Clicked();
                else onGameWorldClicked?.Invoke();
            }
        }
    }
}