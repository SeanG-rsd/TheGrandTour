using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera), typeof(PixelPerfectCamera))]
public class CameraMovement : MonoBehaviour
{
    [Header("Zoom (Pixel Perfect Assets PPU - higher is more zoomed in)")]
    [SerializeField] private int minPPU = 20;
    [SerializeField] private int maxPPU = 400;
    [SerializeField, Range(0.01f, 0.5f)] private float zoomStep = 0.1f;

    private Camera cam;
    private PixelPerfectCamera pixelPerfect;
    private bool isDragging;
    private Vector3 dragOrigin;

    void Awake()
    {
        cam = GetComponent<Camera>();
        pixelPerfect = GetComponent<PixelPerfectCamera>();
    }

    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        HandlePan(mouse);
        HandleZoom(mouse);
    }

    void HandlePan(Mouse mouse)
    {
        Vector2 mousePosition = mouse.position.ReadValue();

        if (mouse.rightButton.wasPressedThisFrame)
        {
            isDragging = true;
            dragOrigin = cam.ScreenToWorldPoint(mousePosition);
        }

        if (mouse.rightButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging)
        {
            // Keep the world point that was grabbed under the cursor
            Vector3 current = cam.ScreenToWorldPoint(mousePosition);
            Vector3 offset = dragOrigin - current;
            offset.z = 0f;
            transform.position += offset;
        }
    }

    void HandleZoom(Mouse mouse)
    {
        float scroll = mouse.scroll.ReadValue().y;
        if (Mathf.Approximately(scroll, 0f)) return;

        // The Pixel Perfect Camera drives orthographicSize, so zoom by changing its PPU instead.
        // Use the sign only, since scroll magnitude differs between platforms/devices.
        int oldPPU = pixelPerfect.assetsPPU;
        int delta = Mathf.Max(1, Mathf.RoundToInt(oldPPU * zoomStep));
        int newPPU = Mathf.Clamp(oldPPU + (scroll > 0f ? delta : -delta), minPPU, maxPPU);
        if (newPPU == oldPPU) return;

        // The Pixel Perfect Camera only applies the new size when rendering, so work out the
        // view scale change directly and shift the camera to keep the cursor's world point fixed.
        Vector3 cursorWorld = cam.ScreenToWorldPoint(mouse.position.ReadValue());
        float ratio = (float)oldPPU / newPPU;
        Vector3 offset = (cursorWorld - transform.position) * (1f - ratio);
        offset.z = 0f;

        pixelPerfect.assetsPPU = newPPU;
        transform.position += offset;
    }
}
