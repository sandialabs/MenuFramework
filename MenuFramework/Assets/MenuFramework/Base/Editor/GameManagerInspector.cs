using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor
{
    //Declare class instance
    private GameManager gameManager;
    private SerializedObject soGameManager;
    private Texture image;

    //Declare ReorderedList variables
    private ReorderableList sceneList;
    private ReorderableList helpMenus;

    private SerializedProperty frameworkGreeting;
    private SerializedProperty loginGreeting;
    private SerializedProperty helpMenuFoldout, sceneListFoldout;

    private void OnEnable()
    {
        //Instantiate class object
        gameManager = (GameManager)target;
        soGameManager = new SerializedObject(gameManager);
        image = (Texture)Resources.Load("SandiaLabsMenuFramework");

        //Instantiate the local variables with the class variables
        frameworkGreeting = soGameManager.FindProperty("frameworkGreeting");
        loginGreeting = soGameManager.FindProperty("loginGreetingBegin");
        helpMenuFoldout = soGameManager.FindProperty("helpMenuFoldout");
        sceneListFoldout = soGameManager.FindProperty("sceneListFoldout");


        helpMenus = new ReorderableList(serializedObject, serializedObject.FindProperty("helpMenus"), true, true, true, true);
        helpMenus.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Help Menus");
                };
        helpMenus.onSelectCallback = (ReorderableList l) =>
                {
                    var gameObject = l.serializedProperty.GetArrayElementAtIndex(l.index).objectReferenceValue as GameObject;
                    if (gameObject)
                        EditorGUIUtility.PingObject(gameObject);
                };
        helpMenus.onCanRemoveCallback = (ReorderableList l) =>
                {
                    return l.count > 0;
                };
        helpMenus.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = helpMenus.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                     new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                 element, GUIContent.none);
            };


        sceneList = new ReorderableList(serializedObject, serializedObject.FindProperty("sceneList"), true, true, true, true);
        sceneList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Scene List");
        };

        sceneList.onSelectCallback = (ReorderableList l) =>
        {
            var gameObject = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("ToggleObject").objectReferenceValue as GameObject;
            if (gameObject)
                EditorGUIUtility.PingObject(gameObject);
        };

        sceneList.onCanRemoveCallback = (ReorderableList l) =>
        {
            return l.count > 0;
        };

        sceneList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = sceneList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                     new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight),
                 element.FindPropertyRelative("toggleObject"), GUIContent.none);
                EditorGUI.PropertyField(
                    new Rect(rect.x + 80, rect.y, rect.width - 60 - 20, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("sceneName"), GUIContent.none);
            };
        sceneList.onAddCallback = (List) =>
        {
            SerializedProperty addElement;
            List.serializedProperty.arraySize++;
            addElement = List.serializedProperty.GetArrayElementAtIndex(List.serializedProperty.arraySize - 1);
            var sceneName = addElement.FindPropertyRelative("sceneName");
            var toggleObject = addElement.FindPropertyRelative("toggleObject");
            sceneName.stringValue = "";
            toggleObject.objectReferenceValue = null;
        };
    }
    public override void OnInspectorGUI()
    {
        GUILayout.Label(image);
        EditorGUILayout.Space();
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        //base.OnInspectorGUI();

        EditorGUILayout.PropertyField(frameworkGreeting, new GUIContent("Application Start Greeting: "));
        EditorGUILayout.PropertyField(loginGreeting, new GUIContent("Login Greeting: "));

        //sceneListFoldout.boolValue = EditorGUILayout.Foldout(sceneListFoldout.boolValue, new GUIContent("Scenes"), true);
        //if (sceneListFoldout.boolValue)
            //sceneList.DoLayoutList();

        helpMenuFoldout.boolValue = EditorGUILayout.Foldout(helpMenuFoldout.boolValue, new GUIContent("Help Menus"), true);
        if (helpMenuFoldout.boolValue)
            helpMenus.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    public void InsertNewTag(string tagName)
    {
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        SerializedObject soAsset = new SerializedObject(asset[0]);
        SerializedProperty soTags = soAsset.FindProperty("tags");
        for (int i = 0; i < soTags.arraySize; i++)
            if (soTags.GetArrayElementAtIndex(i).stringValue == tagName)
                return;

        soTags.InsertArrayElementAtIndex(0);
        soTags.GetArrayElementAtIndex(0).stringValue = tagName;
    }
}