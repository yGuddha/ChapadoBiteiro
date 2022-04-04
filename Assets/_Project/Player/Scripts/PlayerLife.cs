using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    Stats _stats;

    void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    void Update()
    {
        _stats.Life -= Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Soul"))
        {
            Destroy(coll.gameObject);
            _stats.Life += 2;
        }
    }
}

