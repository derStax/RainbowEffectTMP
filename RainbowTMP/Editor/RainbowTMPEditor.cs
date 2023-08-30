using UnityEditor;
using UnityEngine;

namespace FourteenDynamics.TMPExtentions.Rainbow {
    [CustomEditor(typeof(RainbowTMP))]
    [CanEditMultipleObjects]
    public class RainbowTMPEditor : Editor {
        private SerializedProperty _useCustomGradient;
        private SerializedProperty _detailedGradient;
        private SerializedProperty _thisGradient;
        private SerializedProperty _individualCharacters;
        private SerializedProperty _startValue;

        private SerializedProperty _bottomLeft;
        private SerializedProperty _topRight;
        private SerializedProperty _bottomRight;

        private SerializedProperty _duration;

        private SerializedProperty _differenceCharacters;
        private SerializedProperty _updatesPerSecond;
        private SerializedProperty _ignoreSpaces;

        private void OnEnable() {
            _useCustomGradient = serializedObject.FindProperty("_useCustomGradient");
            _detailedGradient = serializedObject.FindProperty("_detailedGradient");
            _thisGradient = serializedObject.FindProperty("_thisGradient");
            _individualCharacters = serializedObject.FindProperty("_individualCharacters");
            _startValue = serializedObject.FindProperty("_startValue");




            _bottomLeft = serializedObject.FindProperty("_bottomLeft");
            _topRight = serializedObject.FindProperty("_topRight");
            _bottomRight = serializedObject.FindProperty("_bottomRight");




            _duration = serializedObject.FindProperty("_duration");

            _differenceCharacters = serializedObject.FindProperty("_differenceCharacters");
            _updatesPerSecond = serializedObject.FindProperty("_updatesPerSecond");
            _ignoreSpaces = serializedObject.FindProperty("_ignoreSpaces");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_useCustomGradient);
            if(_useCustomGradient.boolValue) {
                EditorGUILayout.PropertyField(_detailedGradient);
                if(_detailedGradient.boolValue) {
                    GUIContent c = new("Top Left");
                    EditorGUILayout.PropertyField(_thisGradient, c);
                    EditorGUILayout.PropertyField(_bottomLeft);
                    EditorGUILayout.PropertyField(_topRight);
                    EditorGUILayout.PropertyField(_bottomRight);
                    if(GUILayout.Button("Copy \"Top Left\" to other")) {
                        RainbowTMP r = (RainbowTMP) target;
                        r.SetGradientsEqual();
                    }
                } else {
                    EditorGUILayout.PropertyField(_thisGradient);
                }
            }
            

            EditorGUILayout.PropertyField(_individualCharacters);
            EditorGUILayout.PropertyField(_startValue);

            if(_individualCharacters.boolValue == false) {
                EditorGUILayout.PropertyField(_duration);
            } else {
                EditorGUILayout.PropertyField(_differenceCharacters);
                EditorGUILayout.PropertyField(_updatesPerSecond);
                EditorGUILayout.PropertyField(_ignoreSpaces);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

}
