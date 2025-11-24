using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    public static event Action<Platform> OnPlatformClicked;
    [SerializeField] private LayerMask platformLayerMask;
    public static bool towerPanelOpen { get; set; } = false;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (towerPanelOpen || Time.timeScale == 0f || EventSystem.current.IsPointerOverGameObject())
            return;

        bool mousePressed = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool touchPressed = false;

        Vector2 screenPos = Vector2.zero;

        if (mousePressed)
        {
            screenPos = Mouse.current.position.ReadValue();
        }
        else if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.wasPressedThisFrame)
                {
                    touchPressed = true;
                    screenPos = touch.position.ReadValue();
                    break;
                }
            }
        }

        if (mousePressed || touchPressed)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPos);
            RaycastHit2D raycastHit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, platformLayerMask);

            if (raycastHit.collider != null)
            {
                Platform platform = raycastHit.collider.GetComponent<Platform>();
                if (platform != null)
                {
                    OnPlatformClicked?.Invoke(platform);
                }
            }
        }
    }

    public void PlaceTower(TowerData data)
    {
        Instantiate(data.prefab, transform.position, Quaternion.identity, transform);

        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = false;
        }
    }
}
