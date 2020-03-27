using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    public GameObject[] enemyPrefs;
    public Transform spwnPostition;
    // Start is called before the first frame update
    void Start()
    {
        spwnPostition = transform;
        StartCoroutine("Spwn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spwn()
    {
        while(true)
        {
            if (transform.childCount < 25)
            {
                int index = Random.Range(0, enemyPrefs.Length);
                GameObject enemy = enemyPrefs[index];
                enemy.transform.position = spwnPostition.position;
                Instantiate(enemy, transform, true);
            }
            yield return new WaitForSeconds(0.8f);
        }
    }
}
