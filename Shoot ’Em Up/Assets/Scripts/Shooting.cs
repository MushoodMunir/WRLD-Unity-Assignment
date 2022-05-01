using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject firingEndPoint;
    public float range = 15f;

    [Header("Use Bullets (default)")]

    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public GameManager gameManager;
    private Transform target;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fireCountdown <= 0f)
        {
            UpdateTarget();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }


    void Shoot()
    {
        //GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firingEndPoint.transform.position, firingEndPoint.transform.rotation);
        GameObject bulletGO = ObjectPooler.SharedInstance.GetPooledObject(0);
        bulletGO.transform.SetPositionAndRotation(firingEndPoint.transform.position, firingEndPoint.transform.rotation);

        if (bulletGO != null)
        {
            bulletGO.SetActive(true);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            bullet.Seek(target);
        }
    }


    void UpdateTarget()
    {
        if (gameManager.enemyList.Count > 0)
        {
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in gameManager.enemyList)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                float angel = Vector3.Angle(transform.forward, enemy.transform.position - transform.position);

                Vector3 dir = enemy.transform.position - transform.position;
                var dot = Vector3.Dot(dir, transform.forward);

                if (distanceToEnemy < shortestDistance && dot > 0)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }


            //var enemy = gameManager.enemyList.OrderBy(item => Vector3.Distance(item.transform.position, firingEndPoint.transform.position)).First();
            //var direction = enemy.transform.position - firingEndPoint.transform.position;
            //Debug.DrawLine(firingEndPoint.transform.position, enemy.transform.position, Color.black);


            if (nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.transform;
                Shoot();
                //targetEnemy = nearestEnemy.GetComponent<Enemy>();
            }
            else
            {
                target = null;
            }
        }

    }
}



//foreach (GameObject enemy in enemies)
//{
//    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
//    if (distanceToEnemy < shortestDistance)
//    {
//        shortestDistance = distanceToEnemy;
//        nearestEnemy = enemy;
//    }
//}