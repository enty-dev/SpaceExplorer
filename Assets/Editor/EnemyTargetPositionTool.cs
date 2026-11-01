using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyScript))]
public class EnemyTargetPositionTool : Editor
{
    private bool isSettingPosition = false;
    private EnemyScript enemyScript;

    private void OnEnable()
    {
        enemyScript = (EnemyScript)target;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        isSettingPosition = false;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Target Position Tool", EditorStyles.boldLabel);

        // Button to enable click-to-place mode
        if (GUILayout.Button(isSettingPosition ? "Setting Position... (Click in Scene)" : "Set Target Position (Click Mode)", GUILayout.Height(30)))
        {
            isSettingPosition = !isSettingPosition;
            if (isSettingPosition)
            {
                SceneView.RepaintAll();
            }
        }

        // Display current target position
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Current Target:", enemyScript.targetPosition.ToString());

        // Button to reset target position
        if (GUILayout.Button("Reset to Spawn Position"))
        {
            enemyScript.targetPosition = enemyScript.transform.position;
            EditorUtility.SetDirty(enemyScript);
        }

        if (isSettingPosition)
        {
            EditorGUILayout.HelpBox("Click in the Scene View to set the target position. Press Escape to cancel.", MessageType.Info);
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!isSettingPosition || enemyScript == null)
            return;

        // Handle escape key to cancel
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            isSettingPosition = false;
            Event.current.Use();
            return;
        }

        // Handle mouse click
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Plane groundPlane = new Plane(Vector3.forward, enemyScript.transform.position.z);

            if (groundPlane.Raycast(ray, out float hit))
            {
                Vector3 clickPosition = ray.origin + ray.direction * hit;
                enemyScript.targetPosition = clickPosition;
                EditorUtility.SetDirty(enemyScript);
                isSettingPosition = false;
                Event.current.Use();
            }
        }

        // Draw the target position in the scene
        Handles.color = Color.green;
        Handles.DrawWireDisc(enemyScript.targetPosition, Vector3.forward, 0.3f);
        Handles.Label(enemyScript.targetPosition + Vector3.up * 0.5f, "Target");

        // Draw line from spawn to target
        Handles.color = Color.yellow;
        Handles.DrawLine(enemyScript.transform.position, enemyScript.targetPosition);
    }
}
