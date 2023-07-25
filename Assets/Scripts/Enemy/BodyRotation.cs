using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRotation : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    public Transform target;
    private Animator anime;

    private void Start()
    {
        anime = GetComponent<Animator>();
    }
    void Update()
    {
        direction = anime.deltaPosition - transform.position;
        Debug.Log(direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }
}
