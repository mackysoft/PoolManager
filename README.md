**This library is a legacy. A more advanced object pooling system is available [here](https://github.com/mackysoft/XPool).**

XPool: https://github.com/mackysoft/XPool

# PoolManager for Unity
PoolManager implements an object pool in Unity.

![Inspector_NotPlaying](https://cdn-ak.f.st-hatena.com/images/fotolife/M/MackySoft/20170517/20170517181308.jpg)
# Environment
Unity 2018.1.0f2
# Scripting (July, 2017)
First, add "MackySoft.Pooling" to using area.
```cs
using MackySoft.Pooling;
```
## Get Pool
You can obtain the pool in either of the following ways.  
"GetPoolSafe" automatically add a pool if there is no pool.  
(PoolManager.Instance [prefab] and PoolManager.GetPool (prefab) are the same.)
```cs
Pool pool = PoolManager.Insance[prefab];

Pool pool = PoolManager.GetPool(prefab);

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
# Links
- [Video](https://youtu.be/n9iQ2-71HdE)
- [Twitter](https://twitter.com/macky_soft)
- [オブジェクトプール](http://mackysoft.hatenablog.com/entry/2017/05/18/210000)
# Credits
- [新・オブジェクトプール](http://tsubakit1.hateblo.jp/entry/20140309/1394296581)
