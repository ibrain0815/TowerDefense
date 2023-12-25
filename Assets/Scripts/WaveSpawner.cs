using System.Collections; //�ڷ�ƾ Ŭ����
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public float timeBetweenWaves = 5f;
    public float countdown = 2f;
    public Transform spawnPoint;

    public TextMeshProUGUI waveCountdownText;
    public int waveNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countdown <= 0f)
        {
            StartCoroutine(SpawnWave()); //�� ��ȯ �ڷ�ƾ �Լ� 
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime; // 5�ʺ��� -1�� ī��Ʈ �ٿ�

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        waveCountdownText.text = string.Format("{0:00:00}", countdown);
    }
    IEnumerator SpawnWave()
    {
        waveNumber++;
        Debug.Log("WaveStart");
        for(int i = 0; i< waveNumber; i++)
        {
            SpawnEmemy();
            yield return new WaitForSeconds(0.5f);
        }
       
    }

    void SpawnEmemy() // ����ȯ
    {
        Instantiate(enemyPrefab, spawnPoint.position,spawnPoint.rotation); 

    }
}
