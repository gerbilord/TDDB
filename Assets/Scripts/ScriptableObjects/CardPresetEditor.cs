using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;

[CustomEditor(typeof(CardPreset))]
[CanEditMultipleObjects]
public class CardPresetEditor : Editor
{
    private ReorderableList playEffectsList;

    private SerializedProperty cardPrefabProp;
    private SerializedProperty playEffectsProp;

    private struct PlayEffectCreationParams
    {
        public string Path;
    }

    public void OnEnable()
    {
        cardPrefabProp = serializedObject.FindProperty("cardPrefab");
        playEffectsProp = serializedObject.FindProperty("playEffectsList");

        playEffectsList = new ReorderableList(
            serializedObject,
            playEffectsProp,
            draggable: true,
            displayHeader: true,
            displayAddButton: true,
            displayRemoveButton: true
        );

        playEffectsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Play Effects");
        };

        playEffectsList.onRemoveCallback = (ReorderableList l) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(l.index);
            var obj = element.objectReferenceValue;

            AssetDatabase.RemoveObjectFromAsset(obj);

            DestroyImmediate(obj, true);
            l.serializedProperty.DeleteArrayElementAtIndex(l.index);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        };

        playEffectsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = playEffectsProp.GetArrayElementAtIndex(index);

            rect.y += 2;
            rect.width -= 10;
            rect.height = EditorGUIUtility.singleLineHeight;

            if (element.objectReferenceValue == null)
            {
                return;
            }
            string label = element.objectReferenceValue.name;
            EditorGUI.LabelField(rect, label, EditorStyles.boldLabel);

            // Convert this element's data to a SerializedObject so we can iterate
            // through each SerializedProperty and render a PropertyField.
            SerializedObject nestedObject = new SerializedObject(element.objectReferenceValue);

            // Loop over all properties and render them
            SerializedProperty prop = nestedObject.GetIterator();
            float y = rect.y;
            while (prop.NextVisible(true))
            {
                if (prop.name == "m_Script")
                {
                    continue;
                }

                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, prop);
            }

            nestedObject.ApplyModifiedProperties();

            // Mark edits for saving
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        };

        playEffectsList.elementHeightCallback = (int index) =>
        {
            float baseProp = EditorGUI.GetPropertyHeight(
                playEffectsList.serializedProperty.GetArrayElementAtIndex(index), true);

            float additionalProps = 0;
            SerializedProperty element = playEffectsProp.GetArrayElementAtIndex(index);
            if (element.objectReferenceValue != null)
            {
                SerializedObject playEffect = new SerializedObject(element.objectReferenceValue);
                SerializedProperty prop = playEffect.GetIterator();
                while (prop.NextVisible(true))
                {
                    // XXX: This logic stays in sync with the loop in drawElementCallback.
                    if (prop.name == "m_Script")
                    {
                        continue;
                    }

                    additionalProps += EditorGUIUtility.singleLineHeight;
                }
            }

            float spacingBetweenElements = EditorGUIUtility.singleLineHeight / 2;

            return baseProp + spacingBetweenElements + additionalProps;
        };
            
        playEffectsList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
        {
            var menu = new GenericMenu();
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/ScriptableObjects/Cards/CardEffects" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var type = AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject));
                if (!(type is IPlayEffects))
                {
                    continue;
                }

                menu.AddItem(
                    new GUIContent(Path.GetFileNameWithoutExtension(path)),
                    false,
                    addClickHandler,
                    new PlayEffectCreationParams() { Path = path });
            }
            menu.ShowAsContext();
        };
    }

    private void addClickHandler(object dataObj)
    {
        // Make room in list
        var data = (PlayEffectCreationParams)dataObj;
        var index = playEffectsList.serializedProperty.arraySize;
        playEffectsList.serializedProperty.arraySize++;
        playEffectsList.index = index;
        var element = playEffectsList.serializedProperty.GetArrayElementAtIndex(index);

        // Create the new PlayEffect
        var type = AssetDatabase.LoadAssetAtPath(data.Path, typeof(ScriptableObject));
        var newPlayEffect = ScriptableObject.CreateInstance(type.name);
        newPlayEffect.name = type.name;

        // Add it to CardPreset
        var cardPreset = (CardPreset)target;
        AssetDatabase.AddObjectToAsset(newPlayEffect, cardPreset);
        AssetDatabase.SaveAssets();
        element.objectReferenceValue = newPlayEffect;
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(cardPrefabProp);
        playEffectsList.DoLayoutList();

        /*if (GUILayout.Button("Delete All Play Effects"))
        {
            var cardPreset = (CardPreset)target;
            Undo.RecordObject(cardPreset, "Delete All Play Effects");

            cardPreset.playEffectsList.Clear();

            EditorUtility.SetDirty(cardPreset);
            AssetDatabase.SaveAssets();
        }*/

        serializedObject.ApplyModifiedProperties();
    }
}