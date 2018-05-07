using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MackySoft.Pooling {

	[CustomPropertyDrawer(typeof(Pool))]
	public class D_Pool : PropertyDrawer {

		public override float GetPropertyHeight (SerializedProperty property,GUIContent label) {
			return EditorGUIUtility.singleLineHeight * 5 + 4;
		}

		public override void OnGUI (Rect position,SerializedProperty property,GUIContent label) {
			EditorGUI.BeginProperty(position,label,property);
			var prefab = property.FindPropertyRelative("_Prefab");
			var maxCount = property.FindPropertyRelative("maxCount");
			var prepareCount = property.FindPropertyRelative("prepareCount");
			var interval = property.FindPropertyRelative("_Interval");
			var pooled = property.FindPropertyRelative("pooled");
			
			position.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.LabelField(position,label.text + ": " + pooled.arraySize,EditorStyles.boldLabel);
			EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
			position.y += EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(position,prefab);
			EditorGUI.EndDisabledGroup();
			position.y += EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(position,maxCount);
			position.y += EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(position,prepareCount);
			position.y += EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(position,interval);
			EditorGUI.EndProperty();
		}
	}
}