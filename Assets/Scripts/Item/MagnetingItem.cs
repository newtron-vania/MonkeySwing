using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetingItem : MonoBehaviour
{
    [SerializeField]
    private bool Magneting = false;

    [SerializeField]
    float force = 0.5f;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MagnetField" && !Magneting)
        {
            Magneting = true;
            StartCoroutine(MoveToPlayer(collision));
            Debug.Log("Banana is MagnetField!");
        }
    }

    IEnumerator MoveToPlayer(Collider2D magnet)
    {
        float speed = -4f;
        while (true)
        {
            Vector3 dirVec = magnet.transform.position - this.transform.position;
            this.transform.position += dirVec.normalized * speed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            speed += force;
        }
    }
}
