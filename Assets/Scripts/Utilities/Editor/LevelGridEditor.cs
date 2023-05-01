using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGrid))]
public class LevelGridEditor : Editor
{
    SerializedProperty prefabReferences;
    SerializedProperty gameGrid;
    SerializedProperty enemySpawnPoints;
    SerializedProperty playerSpawnPoint;

    public void OnEnable()
    {
        prefabReferences = serializedObject.FindProperty(nameof(LevelGrid.prefabsReferences));
        gameGrid = serializedObject.FindProperty(nameof(LevelGrid.gridList));
        enemySpawnPoints = serializedObject.FindProperty(nameof(LevelGrid.enemySpawnPoints));
        playerSpawnPoint = serializedObject.FindProperty(nameof(LevelGrid.playerSpawnPoint));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        LevelGrid levelGrid = (LevelGrid)target;

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create New Grid"))
        {
            levelGrid.CreateGrid();
        }

        if (GUILayout.Button("Save & Validate"))
        {
            levelGrid.SaveAndValidateSetUp();

            for (int i = 0; i < levelGrid.gridList.Count; i++)
            {
                EditorUtility.SetDirty(levelGrid.gridList[i]);
            }
        }

        //if (GUILayout.Button("Reset Pos Info"))
        //{
        //    levelGrid.ResetPosInfo();
        //}

        if (GUILayout.Button("Clear"))
        {
            levelGrid.ClearSettings();
            levelGrid.ClearGrid();
        }

        EditorGUILayout.PropertyField(prefabReferences);
        GUI.enabled = false;
        EditorGUILayout.PropertyField(gameGrid);
        GUI.enabled = true;

        if (EditorGUI.EndChangeCheck())
        {
            Debug.Log("Refreshing Grid");
            EditorUtility.SetDirty(target);
        }

        //DrawDefaultInspector();
        DrawPropertiesExcluding(serializedObject, nameof(LevelGrid.prefabsReferences), nameof(LevelGrid.gridList), nameof(LevelGrid.playerSpawnPoint), nameof(LevelGrid.enemySpawnPoints));
        GUI.enabled = false;
        //EditorGUILayout.PropertyField(gameGrid);
        EditorGUILayout.PropertyField(playerSpawnPoint);
        EditorGUILayout.PropertyField(enemySpawnPoints);
        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }

}
