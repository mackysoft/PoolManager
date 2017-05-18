# PoolManager for Unity
PoolManager implements an object pool in Unity.

![Inspector_NotPlaying](https://cdn-ak.f.st-hatena.com/images/fotolife/M/MackySoft/20170517/20170517181308.jpg)
# Scripting (May, 2017)
First, add "MackySoft" to using area.
```cs
using MackySoft;
```
## Get Pool
You can obtain the pool in either of the following ways.

"GetPoolSafe" automatically add a pool if there is no pool.
```cs
Pool pool = PoolManager.Insance[prefab];

Pool pool = PoolManager.GetPoolSafe(prefab);
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
# Inspector
During playback, the number of pooled objects is displayed.

Also, you can not change prefabs and add pools from inspector.

![Inspector_Playing](https://cdn-ak.f.st-hatena.com/images/fotolife/M/MackySoft/20170517/20170517181344.jpg)
# Credits
- [新・オブジェクトプール](http://tsubakit1.hateblo.jp/entry/20140309/1394296581)
