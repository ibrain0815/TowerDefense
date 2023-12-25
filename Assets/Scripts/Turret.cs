using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Tuttet : MonoBehaviour
{
    private Transform target; //Ÿ���� �����ϴµ� ���� Transform����
    private Enemy targetEnemy;

    [Header("General")]
    public float range = 15f; //��ž�� �����Ÿ�

    [Header("Use Bullets(default")]
    public GameObject bulletPrefab; //�߻�� �Ѿ��� ������
    public float fireRate = 1f;   //��ž�� �߻� �ӵ�(�ʴ� �߻� ��)
    public float fireCountdown = 0f; //���� �߻������ ī��Ʈ �ٿ�

    [Header("Use Laser")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    public int damageOverTime = 30;
    public float slowAmount = .5f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";

    public Transform PartToRotate;
    public float turnSpeed = 10f;


    public Transform firePoint;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //�ݺ�
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies) //��� �� �߿��� ���� ������� �� �Ÿ��� ã��
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        //���� ����� ���� �����Ÿ� �ȿ� ������ Ÿ������ ����
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }
      
    //�� �����Ӹ��� ȣ��Ǵ� �Լ�
    void Update()
    {
        if (target == null) 
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                { 
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
                }
            } 

            return;
        } //Ÿ���� ������ �Լ��� ����


        LockOnTarget();

        if(useLaser)
        {
            Laser();
        }
        else
        {
            //�߻� ī��Ʈ�ٿ��� 0���ϸ� �߻�
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime; //ī��Ʈ�ٿ� ����
        }


    }

    void LockOnTarget()
    {
        //Ÿ���� ���� ȸ��
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        targetEnemy.TakeDamege(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0,firePoint.position);
        lineRenderer.SetPosition(1,target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);

        impactEffect.transform.position = target.position;
    }
    void Shoot()
    {
         GameObject bulletGo= (GameObject)Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
         Bullet bullet = bulletGo.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }
    private void OnDrawGizmosSelected() //���׶�� ���� ����
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
