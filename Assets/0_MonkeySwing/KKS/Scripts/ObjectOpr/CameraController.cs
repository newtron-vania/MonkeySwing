using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraPos;
    float shakeTerm = 0.1f;

    public static Action<float, float> CameraShakeEvent;
    private void Start()
    {
        cameraPos = transform.position;
        CameraShakeEvent -= CameraShake;
        CameraShakeEvent += CameraShake;
    }
    public void CameraShake(float time, float force)
    {
        StartCoroutine(StartShake(time, force));
    }

    IEnumerator StartShake(float maxTime, float force)
    {
        float time = 0;
        float basicForce = 0.1f * force;
        while(time <= maxTime)
        {
            Vector3 nextCameraPos = cameraPos;
            float randX = UnityEngine.Random.Range(-basicForce, basicForce);
            float randY = UnityEngine.Random.Range(-basicForce, basicForce);
            Vector3 nextMove = new Vector3(randX, randY, 0);
            transform.position = nextCameraPos + nextMove;
            yield return new WaitForSeconds(shakeTerm);
            time += shakeTerm;
        }
        transform.position = cameraPos;
    }
}
