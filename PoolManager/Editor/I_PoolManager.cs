using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace MackySoft {

	[CustomEditor(typeof(PoolManager))]
	public class I_PoolManager : Editor {
		
		private SerializedProperty poolList;
		private ReorderableList l_poolList;

		private void OnEnable () {
			poolList = serializedObject.FindProperty("poolList");
			l_poolList = new ReorderableList(serializedObject,poolList,false,true,true,true);
			l_poolList.displayAdd = !EditorApplication.isPlaying;
			l_poolList.elementHeight = EditorGUIUtility.singleLineHeight * 5 + 4;
			l_poolList.drawHeaderCallback = rect => EditorGUI.LabelField(rect,poolList.displayName);
			l_poolList.drawElementCallback = (rect,index,isActive,isFocused) => {
				var pool = poolList.GetArrayElementAtIndex(index);
				rect.height -= 4;
				rect.y += 2;
				EditorGUI.PropertyField(rect,pool);
			};
			l_poolList.onAddCallback = list => {
				poolList.arraySize++;
				var pool = poolList.GetArrayElementAtIndex(poolList.arraySize - 1);
				pool.FindPropertyRelative("_Prefab").objectReferenceValue = null;
				pool.FindPropertyRelative("maxCount").intValue = 0;
				pool.FindPropertyRelative("prepareCount").intValue = 0;
				pool.FindPropertyRelative("_Interval").floatValue = 1;
			};
			l_poolList.onRemoveCallback = list => {
				if (EditorApplication.isPlaying)
					PoolManager.RemovePool(PoolManager.Instance[list.index].Prefab);
				poolList.DeleteArrayElementAtIndex(list.index);
			};
		}

		public override void OnInspectorGUI () {
			serializedObject.Update();
			l_poolList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}
	}
}