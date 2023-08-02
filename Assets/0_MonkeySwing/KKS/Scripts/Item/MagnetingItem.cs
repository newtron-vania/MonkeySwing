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
            MonkeyController monkey = GameManagerEx.Instance.monkey;
            StartCoroutine(MoveToPlayer(monkey));
            Debug.Log("Banana is MagnetField!");
        }
    }

    private void OnDisable()
    {
        Magneting = false;
    }

    IEnumerator MoveToPlayer(MonkeyController monkey)
    {
        float speed = -4f;
        while (true)
        {
            Vector3 dirVec = monkey.transform.position - this.transform.position;
            this.transform.position += dirVec.normalized * speed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            speed += force;
        }
    }
}
