using UnityEngine;

[ExecuteInEditMode] // Allows you to see the changes in the Scene view without pressing Play
public class CameraUIAligner : MonoBehaviour
{
    [SerializeField] private RectTransform gameplayAreaUI;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateCameraViewport();
    }

    void LateUpdate()
    {
        // Keeps it updated if the screen resizes or in Editor mode
        UpdateCameraViewport();
    }

    void UpdateCameraViewport()
    {
        if (gameplayAreaUI == null || cam == null) return;

        // Get the screen corners of the UI element
        Vector3[] corners = new Vector3[4];
        gameplayAreaUI.GetWorldCorners(corners);

        // Convert world corners to normalized screen coordinates (0 to 1)
        float xMin = corners[0].x / Screen.width;
        float yMin = corners[0].y / Screen.height;
        float xMax = corners[2].x / Screen.width;
        float yMax = corners[2].y / Screen.height;

        // Set the camera's rect to match the UI perfectly
        cam.rect = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }
}