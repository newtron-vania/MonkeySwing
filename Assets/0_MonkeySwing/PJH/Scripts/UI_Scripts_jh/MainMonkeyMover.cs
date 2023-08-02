using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMonkeyMover : MonoBehaviour
{
    [SerializeField]
    private float _normalFaceChangeTime = 10f;

    [SerializeField]
    private SpriteRenderer _monkeyFace;

    [SerializeField]
    private Sprite[] _monkeyFaceArray;
    
    private float currentTime = 0;
    private float _currentFaceChangeTime = 10f;

    private void Start()
    {
        _currentFaceChangeTime = _normalFaceChangeTime;
    }
    private void Update()
    {
        if(currentTime >= _currentFaceChangeTime)
        {
            currentTime = 0f;
            _currentFaceChangeTime = Random.Range(Mathf.Max(1f, _normalFaceChangeTime - 1f), _normalFaceChangeTime + 1f);
            ChangeFace();
        }
        currentTime += Time.unscaledDeltaTime;
    }

    private void ChangeFace()
    {
        _monkeyFace.sprite = _monkeyFaceArray[Random.Range(0, _monkeyFaceArray.Length)];
    }

}
