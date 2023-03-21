using System.Collections.Generic;
using UnityEngine;
public class PoolManager
{
    #region Pool
    class Pool
    {
        //���� ������Ʈ
        public GameObject Original { get; private set; }
        //������Ʈ Ǯ ������ Root Object
        public Transform Root { get; set; }
        //Poolable Object�� �����ϴ� poolStack. stack�� �ƴ϶� queue�� ����ص� �ȴ�.
        Stack<Poolable> _poolStack = new Stack<Poolable>();
        //original Object�� Pooling�� Pool�� �������� ���� ��� Pool�� �����Ѵ�.
        public void Init(GameObject original, int count = 2)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{Original.name}_Root";
            for (int i = 0; i < count; i++)
                Push(Create());
        }
        //Pool�� ����Ǵ� Poolable Object�� �����Ѵ�.
        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }
        //������ Poolable Object�� pool�� ��ġ�Ѵ�. 
        public void Push(Poolable poolable)
        {
            //object�� �������� ���� �� ��ġ���� �ʴ´�.
            if (poolable == null)
                return;
            //pool������ �ϴ� Object�� �ڽĿ�����Ʈ�� ��ġ�ϰ�, ��Ȱ��ȭ�Ͽ� ������ ������ Ȯ�ν�Ų��.
            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;
            //poolStack�� ����
            _poolStack.Push(poolable);
        }
        //pool�� ��ġ�� ������Ʈ�� �����´�.
        public Poolable Pop(Transform parent)
        {
            Poolable poolable = null;
            //pool�� �����ִ� Poolable Object�� �ִ��� Ȯ���Ѵ�. ���� ��� ���� ����.
            while (_poolStack.Count > 0)
            {
                poolable = _poolStack.Pop();
                if (poolable.gameObject.activeSelf == false)
                    break;
            }
            //pool�� �����ִ� Object�� ���ٸ� ������ ���� ���� �����Ͽ� �����´�.
            if (poolable == null || poolable.gameObject.activeSelf == true)
                poolable = Create();
            poolable.gameObject.SetActive(true);
            //�θ� ������Ʈ�� ��ġ���� ���� ��� Scene ��ġ�� ��ġ�ȴ�.
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.isUsing = true;
            return poolable;
        }
    }
    #endregion
    //��ü Pool�� ����ϴ� ��ųʸ�. �� Pool���� ��ġ�Ǵ� Poolable Object�� �ٸ��� ������ ������ �������� Pool�� ���� �����ϰ� ����������Ѵ�.
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;
    public void Init()
    {
        //��ü Pool Object�� �θ� ������Ʈ ��@Pool_Root���� ��ġ�Ͽ� �����Ѵ�.
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }
    //Pool����� �� �� Object�� �ش� ������ �����ϴ� Pool�� �ٽ� ��ġ�Ѵ�.
    public void Push(Poolable poolable, float time)
    {
        //�ش� ������ ����ϴ� Pool�� �����ϴ��� Ȯ��. �������� ���� ��� Poolable 	Object�� �ƴ϶�� �ǹ��̹Ƿ� �ش� ������Ʈ�� �ı��Ѵ�.
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            Managers.Resource.Destroy(poolable.gameObject, time);
            return;
        }
        //Pool�� ������ ��� �ش� pool�� �����Ѵ�.
        _pool[name].Push(poolable);
    }
    //�ش� ������ �ش��ϴ� ������Ʈ�� pool���� ������.
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        //������ �ش�Ǵ� pool�� �������� ���� ��� pool�� ����
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);
        //�ش� pool�� Object�� ������.
        return _pool[original.name].Pop(parent);
    }
    //������ ��ġ�ϴ� pool�� �����Ѵ�.
    public void CreatePool(GameObject original, int count = 2)
    {
        //pool ��ü�� �����ϰ� �ش� ������ ���� ������ ���� pool�� �ʱ�ȭ�Ѵ�.
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;
        //�ش� Ǯ�� pool�� _pool ��ųʸ��� �߰�
        _pool.Add(original.name, pool);
    }
    //�ش� Ǯ�� ������ �����´�.
    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;
        return _pool[name].Original;
    }
    //��� pool�� �����Ͽ� �ʱ�ȭ�Ѵ�.
    public void Clear()
    {
        foreach (Transform child in _root)
        {
            Managers.Resource.Destroy(child.gameObject);
        }
        _pool.Clear();
    }
}
