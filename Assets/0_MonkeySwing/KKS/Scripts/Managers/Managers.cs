using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }


    #region Contents
    #endregion

    #region core
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    DataManager _data = new DataManager();


    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static DataManager Data { get { return Instance._data; } }

    #endregion
    void Start()
    {
        Init();
    }
    private void Update()
    {
        Input.OnUpdate();
    }
    static void Init()
    {
        if (s_instance == null)
        {

            //�Ŵ��� �ʱ�ȭ
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            //�������� �ʰԲ� ���� -> Scene �̵��� �ϴ��� �ı����� ����
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            s_instance._input.Init();
            s_instance._sound.Init();
            s_instance._pool.Init();
            s_instance._data.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
