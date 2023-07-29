using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class BallController : MonoBehaviour
    {
        public Vector2 MinMaxXPosition;
        public bool    IsGameStarted = false;

        private Camera      _camera;
        private Rigidbody2D _rb2D;
        
        private void Start()
        {
            _camera = Camera.main;
            _rb2D   = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!IsGameStarted) return;
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject(0))
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                float   xOffSet  = Mathf.Clamp(mousePos.x, MinMaxXPosition.x, MinMaxXPosition.y);
                _rb2D.position = new Vector2(xOffSet, _rb2D.position.y);
            }
        }
    }
}