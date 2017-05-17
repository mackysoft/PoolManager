using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MackySoft {
	
	[HelpURL("https://github.com/MackySoft/Unity_PoolManager.git")]
	public class PoolManager : MonoBehaviour {

		#region Variables
		
		[SerializeField]
		private List<Pool> poolList = new List<Pool>();
		private Dictionary<GameObject,Pool> pools = new Dictionary<GameObject,Pool>();

		#endregion

		#region Properties

		/// <summary>
		/// Instance of <see cref="PoolManager"/>.
		/// </summary>
		public static PoolManager Instance {
			get {
				if (!_Instance) {
					_Instance = FindObjectOfType<PoolManager>();
					if (!_Instance)
						_Instance = _Instance = new GameObject("Pool Manager").AddComponent<PoolManager>();
				}
				return _Instance;
			}
		}
		private static PoolManager _Instance = null;
		
		/// <summary>
		/// Count of pools.
		/// </summary>
		public int Count {
			get { return poolList.Count; }
		}

		/// <summary>
		/// Get a pool with the specified index.
		/// </summary>
		public Pool this[int index] {
			get { return poolList[index]; }
		}
		
		/// <summary>
		/// Get a pool with the specified prefab.
		/// </summary>
		public Pool this[GameObject prefab] {
			get { return pools[prefab]; }
		}

		#endregion

		#region Component Segments

		private void Awake () {
			foreach (Pool pool in poolList) {
				if (pool.Prefab) pools.Add(pool.Prefab,pool);
			}
			if (poolList.RemoveAll(p => !p.Prefab) > 0)
				throw new PoolException("There was a pool where there is no prefab.");
		}

		private void OnEnable () {
			for (int i = 0;Count > i;i++)
				StartCoroutine(this[i].RemoveCoroutine());
		}

		private void OnDisable () {
			StopAllCoroutines();
		}

		private void OnDestroy () {
			for (int i = 0;Count > i;i++)
				RemovePool(this[i].Prefab);
		}

#if UNITY_EDITOR
		private void OnValidate () {
			for (int i = 0;Count > i;i++) {
				this[i].Interval = this[i].Interval;
			}
		}
#endif

		#endregion

		/// <summary>
		/// <para> Returns the specified prefab pool. </para>
		/// <para> If not has pool, it create a new pool with <see cref="AddPool(GameObject, int, int, float)"/> and returns it. </para>
		/// </summary>
		/// <param name="prefab">
		/// <para> Prefab of pool to get. </para>
		/// <para> If not has pool, <see cref="Pool.Prefab"/> for the new pool. </para>
		/// </param>
		/// <param name="maxCount"> <see cref="Pool.maxCount"/> for the new pool. </param>
		/// <param name="prepareCount"> <see cref="Pool.prepareCount"/> for the new pool. </param>
		/// <param name="interval"> <see cref="Pool.Interval"/> for the new pool.</param>
		public static Pool GetPoolSafe (GameObject prefab,int maxCount = 0,int prepareCount = 0,float interval = 1) {
			if (!prefab)
				throw new ArgumentNullException("prefab");
			return HasPool(prefab) ? Instance[prefab] : AddPool(prefab,maxCount,prepareCount,interval);
		}
		
		/// <summary>
		/// Whether the specified prefab pool exists.
		/// </summary>
		/// <param name="prefab"> Prefab of pool to check. </param>
		public static bool HasPool (GameObject prefab) {
			return prefab && Instance.pools.ContainsKey(prefab);
		}

		/// <summary>
		/// Add a new pool by specified prefab.
		/// </summary>
		/// <param name="prefab"> <see cref="Pool.Prefab"/> for the new pool. </param>
		/// <param name="maxCount"> <see cref="Pool.maxCount"/> for the new pool. </param>
		/// <param name="prepareCount"> <see cref="Pool.prepareCount"/> for the new pool. </param>
		/// <param name="interval"> <see cref="Pool.Interval"/> for the new pool.</param>
		public static Pool AddPool (GameObject prefab,int maxCount = 0,int prepareCount = 0,float interval = 1) {
			if (!prefab)
				throw new ArgumentNullException("prefab");
			if (HasPool(prefab))
				throw new PoolException(string.Format("\"{0}\" pool already exist.",prefab.name));
			Pool pool = new Pool(prefab,maxCount,prepareCount,interval);
			Instance.poolList.Add(pool);
			Instance.pools.Add(prefab,pool);
			Instance.StartCoroutine(pool.RemoveCoroutine());
			return pool;
		}

		/// <summary>
		/// Remove a specified prefab pool.
		/// </summary>
		/// <param name="prefab"> Prefab of the pool to remove. </param>
		/// <param name="destroyObjects"> Whether the destroy pooled objects. </param>
		public static void RemovePool (GameObject prefab,bool destroyObjects = true) {
			if (!prefab)
				throw new ArgumentNullException("prefab");
			if (!HasPool(prefab))
				throw new PoolException(string.Format("\"{0}\" pool does not exist.",prefab.name));

			if (destroyObjects)
				Instance[prefab].DestroyObjects();

			Instance.poolList.Remove(Instance[prefab]);
			Instance.pools.Remove(prefab);
		}

	}
}