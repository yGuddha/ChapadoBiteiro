using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    Stats _stats;

    [SerializeField] float gunOffset;

    void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var proj = Instantiate(projectile,
                                    new Vector3(this.transform.position.x + (gunOffset * _stats.LookDir), this.transform.position.y, this.transform.position.z),
                                    Quaternion.Euler(0, _stats.LookDir == 1 ? 0:180, 0));

            proj.GetComponent<Projectile>().Shoot(Vector3.right * _stats.LookDir, _stats);
        }
    }

}
