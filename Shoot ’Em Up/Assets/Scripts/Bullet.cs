using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;

    public float speed = 100;

    public int damage = 50;

    //public float explosionRadius = 0f;
    //public GameObject impactEffect;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {

        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

    }

    void HitTarget()
    {
        Damage(target);
        gameObject.SetActive(false);
    }



    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponentInParent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(damage, enemy.gameObject);
        }
    }
}