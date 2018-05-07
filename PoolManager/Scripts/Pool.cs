using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MackySoft.Pooling {
	
	[Serializable]
	public class Pool {

		#region Variables
		
		[SerializeField]
		private GameObject _Prefab = null;
		
		/// <summary>
		/// <para> Instances exceeding this number are not instantiated. </para>
		/// <para> If it is 0 or less, it is instantiated without limit. </para>
		/// </summary>
		[Tooltip(
			"Instances exceeding this number are not instantiated.\n" +
			"If it is 0 or less, it is instantiated without limit."
		)]
		public int maxCount = 0;
		
		/// <summary>
		/// <para> Instances exceeding this number are destroyed. </para>
		/// <para> See also: <see cref="RemoveCoroutine"/> </para>
		/// </summary>
		[Tooltip("Instances exceeding this number are destroyed.")]
		public int prepareCount = 0;

		[Tooltip("Interval calling #RemoveObject(int) in #RemoveCoroutine().")]
		[SerializeField]
		private float _Interval = 1;

		[SerializeField]
		private List<GameObject> pooled = new List<GameObject>();

		#endregion

		#region Properties

		/// <summary>
		/// Referenced prefab. (Read only)
		/// </summary>
		public GameObject Prefab {
			get { return _Prefab; }
		}

		/// <summary>
		/// Interval calling <see cref="RemoveObject(int)"/> in <see cref="RemoveCoroutine"/>.
		/// </summary>
		public float Interval {
			get { return _Interval; }
			set { _Interval = Mathf.Max(0.1f,value); }
		}

		/// <summary>
		/// Count of pooled instance.
		/// </summary>
		public int Count {
			get { return pooled.Count; }
		}

		#endregion

		/// <summary>
		/// <see cref="Prefab"/> can be set only in this constructor or inspector.
		/// </summary>
		/// <param name="prefab"> <see cref="Prefab"/> for the new pool. </param>
		/// <param name="maxCount"> <see cref="maxCount"/> for the new pool. </param>
		/// <param name="prepareCount"> <see cref="prepareCount"/> for the new pool. </param>
		/// <param name="interval"> <see cref="Interval"/> for the new pool. </param>
		public Pool (GameObject prefab,int maxCount = 0,int prepareCount = 0,float interval = 1) {
			if (!prefab)
				throw new ArgumentNullException("prefab");
			_Prefab = prefab;
			this.maxCount = maxCount;
			this.prepareCount = prepareCount;
			Interval = interval;
		}

		/// <summary>
		/// Get the instance.
		/// </summary>
		/// <param name="position"> Position for the instance. </param>
		/// <param name="rotation"> Orientation of the instance. </param>
		/// <param name="parent"> Parent that will be assigned to instance. </param>
		public GameObject Get (Vector3 position,Quaternion rotation,Transform parent = null) {
			pooled.RemoveAll(o => !o);

			for (int i = 0;Count > i;i++) {
				if (!pooled[i].activeSelf) {
					pooled[i].transform.SetPositionAndRotation(position,rotation);
					pooled[i].transform.SetParent(parent);
					pooled[i].SetActive(true);
					return pooled[i];
				}
			}
		
			if (maxCount <= 0 || Count < maxCount) {
				GameObject obj = GameObject.Instantiate(Prefab,position,rotation,parent);
				obj.SetActive(true);
				pooled.Add(obj);
				return obj;
			}
			return null;
		}
		
		/// <summary>
		/// Get the instance.
		/// </summary>
		/// <param name="parent"> Parent that will be assigned to instance. </param>
		public GameObject Get (Transform parent = null) {
			pooled.RemoveAll(o => !o);

			for (int i = 0;Count > i;i++) {
				if (!pooled[i].activeSelf) {
					pooled[i].transform.SetParent(parent);
					pooled[i].SetActive(true);
					return pooled[i];	
				}
			}
		
			if (maxCount <= 0 || Count < maxCount) {
				GameObject obj = GameObject.Instantiate(Prefab,parent);
				obj.SetActive(true);
				pooled.Add(obj);
				return obj;
			}
			return null;
		}

		public GameObject Get (out bool isNewInstance,Transform parent = null) {
			pooled.RemoveAll(o => !o);

			for (int i = 0;Count > i;i++) {
				if (!pooled[i].activeSelf) {
					pooled[i].transform.SetParent(parent);
					pooled[i].SetActive(true);
					isNewInstance = false;
					return pooled[i];	
				}
			}
		
			if (maxCount <= 0 || Count < maxCount) {
				GameObject obj = GameObject.Instantiate(Prefab,parent);
				obj.SetActive(true);
				pooled.Add(obj);
				isNewInstance = true;
				return obj;
			} else {
				isNewInstance = false;
				return null;
			}
		}

		public GameObject Get (out bool isNewInstance,Vector3 position,Quaternion rotation,Transform parent = null) {
			pooled.RemoveAll(o => !o);

			for (int i = 0;Count > i;i++) {
				if (!pooled[i].activeSelf) {
					pooled[i].transform.SetPositionAndRotation(position,rotation);
					pooled[i].transform.SetParent(parent);
					pooled[i].SetActive(true);
					isNewInstance = false;
					return pooled[i];	
				}
			}
		
			if (maxCount <= 0 || Count < maxCount) {
				GameObject obj = GameObject.Instantiate(Prefab,position,rotation,parent);
				obj.SetActive(true);
				pooled.Add(obj);
				isNewInstance = true;
				return obj;
			} else {
				isNewInstance = false;
				return null;
			}
		}

		/// <summary>
		/// Get the attached component of instance.
		/// </summary>
		/// <param name="position"> Position for the instance. </param>
		/// <param name="rotation"> Orientation of the instance. </param>
		/// <param name="parent"> Parent that will be assigned to instance. </param>
		public T Get<T> (Vector3 position,Quaternion rotation,Transform parent = null) where T : Component {
			GameObject ins = Get(position,rotation,parent);
			return ins ? ins.GetComponent<T>() : null;
		}

		/// <summary>
		/// Get the attached component of instance.
		/// </summary>
		/// <param name="parent"> Parent that will be assigned to instance. </param>
		public T Get<T> (Transform parent = null) where T : Component {
			GameObject ins = Get(parent);
			return ins ? ins.GetComponent<T>() : null;
		}

		/// <summary>
		/// Get the attached component of instance.
		/// </summary>
		/// <param name="position"> Position for the instance. </param>
		/// <param name="rotation"> Orientation of the instance. </param>
		/// <param name="parent"> Parent that will be assigned to instance. </param>
		public T Get<T> (out bool isNewInstance,Vector3 position,Quaternion rotation,Transform parent = null) where T : Component {
			GameObject ins = Get(out isNewInstance,position,rotation,parent);
			return ins ? ins.GetComponent<T>() : null;
		}

		/// <summary>
		/// Get the attached component of instance.
		/// </summary>
		/// <param name="parent"> Parent that will be assigned to instance. </param>
		public T Get<T> (out bool isNewInstance,Transform parent = null) where T : Component {
			GameObject ins = Get(out isNewInstance,parent);
			return ins ? ins.GetComponent<T>() : null;
		}
		
		/// <summary>
		/// Call <see cref="RemoveObject(int)"/> every <see cref="Interval"/> seconds.
		/// </summary>
		public IEnumerator RemoveCoroutine () {
			while (true) {
				RemoveObject(prepareCount);
				yield return new WaitForSeconds(_Interval);
			}
		}
		
		/// <summary>
		/// Destroy unused instances.
		/// </summary>
		/// <param name="max"> Leave as many instances as this number. </param>
		public void RemoveObject (int max) {
			if (Count > max) {
				int needRemoveCount = Count - max;
				foreach (GameObject obj in pooled.ToArray()) {
					if (needRemoveCount == 0) break;

					if (!obj.activeSelf) {
						pooled.Remove(obj);
						GameObject.Destroy(obj);
						needRemoveCount--;
					}
				}
			}
		}

		/// <summary>
		/// Destroy all instances.
		/// </summary>
		public void DestroyObjects () {
			for (int i = 0;Count > i;i++)
				GameObject.Destroy(pooled[i]);
			pooled.Clear();
		}

	}
}