using UnityEngine;

public class Projectile : MonoBehaviour
{
    float _lifeTime;
    float _damage;

    bool _isActive = false;

    void Update()
    {
        if (!_isActive) return;

        _lifeTime -= Time.deltaTime;
        if(_lifeTime <= 0f)
            Destroy(this.gameObject);
    }

    public void Shoot(Vector3 dir, Stats stats)
    {
        this._lifeTime = stats.ProjectileLifeTime;
        this._damage = stats.Damage + stats.DamageModifiers;

        _isActive = true;

        GetComponent<Rigidbody2D>().AddForce(dir * (stats.ProjectileSpeed + stats.ProjectileSpeedModifiers));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        { 
            Destroy(this.gameObject);
            var stats = col.gameObject.GetComponent<Stats>();
            stats.Life -= 1;
            return;
        }

        if(col.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
            var stats = col.gameObject.GetComponent<Stats>();
            stats.Life -= 5;
            return;
        }

        if(col.gameObject.layer == 6)
        {
            Destroy(this.gameObject);
            return;
        }
    }

}
