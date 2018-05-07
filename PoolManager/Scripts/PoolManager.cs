using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MackySoft.Pooling {
	
	/// <summary>
	/// A component that manages the <see cref="Pool"/>.
	/// </summary>
	[HelpURL("https://github.com/MackySoft/Unity_PoolManager.git")]
	public class PoolManager : MonoBehaviour {

		#region Variables

		private static bool isQuitting = false;
		
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
						_Instance = _Instance = new GameObject("PoolManager").AddComponent<PoolManager>();
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
		/// <para> Get a pool with the specified prefab. </para>
		/// <para> See also: <see cref="GetPool(GameObject)"/> </para>
		/// </summary>
		public Pool this[GameObject prefab] {
			get { return GetPool(prefab); }
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
			if (isQuitting) return;
			for (int i = 0;Count > i;i++)
				RemovePool(this[i].Prefab,true);
		}

		private void OnApplicationQuit () {
			isQuitting = true;
		}

#if UNITY_EDITOR
		private void OnValidate () {
			for (int i = 0;Count > i;i++)
				this[i].Interval = this[i].Interval;
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
			return HasPool(prefab) ? GetPool(prefab) : AddPool(prefab,maxCount,prepareCount,interval);
		}

		/// <summary>
		/// <para> Returns the specified prefab pool. </para>
		/// <para> If not has pool, throw <see cref="PoolException"/>. </para>
		/// </summary>
		/// <param name="prefab"> Prefab of pool to get. </param>
		public static Pool GetPool (GameObject prefab) {
			if (!prefab)
				throw new ArgumentNullException("prefab");
			if (!HasPool(prefab))
				throw new PoolException(string.Format("\"{0}\" pool does not exist.",prefab.name));
			return Instance.pools[prefab];
		}
		
		/// <summary>
		/// Whether the specified prefab pool exists.
		/// </summary>
		/// <param name="prefab"> Prefab of pool to check. </param>
		public static bool HasPool (GameObject prefab) {
			return prefab && Instance.pools.ContainsKey(prefab);
		}

		/// <summary>
		/// Whether the specified pool exists.
		/// </summary>
		/// <param name="pool"> Pool to check. </param>
		public static bool HasPool (Pool pool) {
			return pool != null && Instance.poolList.Contains(pool);
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
		public static void RemovePool (GameObject prefab,bool destroyObjects) {
			if (!prefab)
				throw new ArgumentNullException("prefab");
			if (!HasPool(prefab))
				throw new PoolException(string.Format("\"{0}\" pool does not exist.",prefab.name));

			if (destroyObjects)
				Instance[prefab].DestroyObjects();

			Instance.poolList.Remove(Instance[prefab]);
			Instance.pools.Remove(prefab);
		}

		/// <summary>
		/// Remove a pool.
		/// </summary>
		/// <param name="pool"> Pool to remove. </param>
		/// <param name="destroyObjects"> Whether the destroy pooled objects. </param>
		public static void RemovePool (Pool pool,bool destroyObjects) {
			if (pool == null)
				throw new ArgumentNullException("pool");
			if (!HasPool(pool))
				throw new PoolException("Pool does not exist.");

			if (destroyObjects)
				pool.DestroyObjects();

			Instance.poolList.Remove(pool);
			Instance.pools.Remove(pool.Prefab);
		}

	}
}