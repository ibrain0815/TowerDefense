using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Tuttet : MonoBehaviour
{
    private Transform target; //타겟을 추적하는데 사용될 Transform변수
    private Enemy targetEnemy;

    [Header("General")]
    public float range = 15f; //포탑의 사정거리

    [Header("Use Bullets(default")]
    public GameObject bulletPrefab; //발사될 총알의 프리팹
    public float fireRate = 1f;   //포탑의 발사 속도(초당 발사 수)
    public float fireCountdown = 0f; //다음 발사까지의 카운트 다운

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
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //반복
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies) //모든 적 중에서 제일 가까운놈과 그 거리를 찾기
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        //가장 가까운 적이 사정거리 안에 있으면 타겟으로 설정
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
      
    //매 프레임마다 호출되는 함수
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
        } //타겟이 없으면 함수를 종료


        LockOnTarget();

        if(useLaser)
        {
            Laser();
        }
        else
        {
            //발사 카운트다운이 0이하면 발사
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime; //카운트다운 감소
        }


    }

    void LockOnTarget()
    {
        //타겟을 향해 회전
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
    private void OnDrawGizmosSelected() //동그라미 범위 생성
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
