using UnityEngine;
using System.Collections;

public class DemoSpawnerControl : MonoBehaviour {

	public Transform[] spawners;
	public GameObject enemyMeleePrefab, enemyRangedPrefab, enemySpecialPrefab;
	[HideInInspector]public int enemyCount, specialEnemyCount;
	
	private const int maxEnemyCount = 50;
	private float spawnMeleeNext = 0.0f;
	private float spawnMeleeRate = 1.0f;
	
	void Update () {
		CheckSpawnRate();
		SpawnEnemy();
	}
	
	private void CheckSpawnRate(){
		if(enemyCount >= 10 && enemyCount < 20){
			spawnMeleeRate = 0.6f;
		} else if (enemyCount >= 20 && enemyCount < 30){
			spawnMeleeRate = 0.5f;
		} else if (enemyCount >= 30 && enemyCount < 40){
			spawnMeleeRate = 0.4f;
		} else if (enemyCount >= 40){
			spawnMeleeRate = 0.3f;
		} else {
			spawnMeleeRate = 0.7f;
		}
	}
	
	private void SpawnEnemy(){
		if(Time.time > spawnMeleeNext && enemyCount != maxEnemyCount){
			spawnMeleeNext = Time.time + spawnMeleeRate;
			GameObject spawnEnemyPrefab;
			int rand = Random.Range(0, spawners.Length);
			Vector3 spawnPos = spawners[rand].position;
			float randEnemy = Random.value;
			if(randEnemy <= 0.15f && randEnemy >= 0.05f){
				spawnEnemyPrefab = enemyRangedPrefab;
			} else if(randEnemy < 0.05f){
				if(specialEnemyCount == 0){
					spawnEnemyPrefab = enemySpecialPrefab;
					specialEnemyCount++;
				} else {
					spawnEnemyPrefab = enemyMeleePrefab;
				}
			} else {
				spawnEnemyPrefab = enemyMeleePrefab;
			}
			Instantiate(spawnEnemyPrefab,spawnPos,Quaternion.identity);
			enemyCount++;
		}
	}
}