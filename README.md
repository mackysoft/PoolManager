# PoolManager for Unity

# Scripting
First, add "MackySoft" to using area.
```cs
using MackySoft;
```
## Get Pool
You can obtain the pool in either of the following ways.

"GetPoolSafe" automatically add a pool if there is no pool.
```cs
PoolManager.Insance[prefab];

PoolManager.GetPoolSafe(prefab);
```

Or you can use it by caching in the following way.
```cs
Pool pool = PoolManager.AddPool(prefab);
```
## Get Instance
Instances can be obtained from the pool in the following way.
```cs
GameObject ins = pool.Get(parent);

GameObject ins = pool.Get(position,rotation,parent);

var ins = pool.Get<COMPONENT_NAME>(parent);

var ins = pool.Get<COMPONENT_NAME>(position,rotation,parent);
```
## Disable Object
This PoolManager recognizes inactive objects as being unused.
```cs
Destroy(pooledObject);//NO NO NO

pooledGameObject.SetActive(false);//OK with this!
```
