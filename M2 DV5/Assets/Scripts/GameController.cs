using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitionPattern {
    public class GameController : MonoBehaviour {
        public GameObject friendlyObj;
        public GameObject enemyObj;
        public Material enemyMaterial;
        public Material closestEnemyMaterial;
        public Transform enemyParent;
        public Transform friendlyParent;

        public TextMeshProUGUI updateTimeText;
        private float updateTime = 0f;
        public Button togglePartitionButton;
        private bool partitionToggle = true;

        public List<Soldier> enemySoldiers = new List<Soldier>();
        public List<Soldier> friendlySoldiers = new List<Soldier>();
        public List<Soldier> closestEnemies = new List<Soldier>();

        private float mapWidth = 50f;
        private int cellSize = 10;
        private int numberOfSoldiers = 100;

        private Grid grid;
        public static GameController instance;

        private void Awake() {
            instance = this;
        }

        private void Start() {
            grid = new Grid((int)mapWidth, cellSize);

            for (int i = 0; i < numberOfSoldiers; i++) {
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));
                newEnemy.transform.parent = enemyParent;
                
                if (i % 2 == 0) {
                    randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                    GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;
                    friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));
                    newFriendly.transform.parent = friendlyParent;
                }
            }
        }

        float cooldown = 0f;
        bool fired = false;
        private void Update() {
            cooldown += Time.deltaTime;
            updateTime += Time.deltaTime;
            updateTimeText.text = "Time in Update: " + updateTime.ToString("F2");

            for (int i = 0; i < enemySoldiers.Count; i++) {
                enemySoldiers[i].Move();
            }
            
            for (int i = 0; i < closestEnemies.Count; i++) {
                if (closestEnemies[i] != null && closestEnemies[i].soldierMeshRenderer != null) {
                    closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
                }
            }

            closestEnemies.Clear();

            for (int i = 0; i < friendlySoldiers.Count; i++) {
                if (partitionToggle) {
                    Soldier closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);
                    
                    if (closestEnemy != null) {
                        closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;
                        closestEnemies.Add(closestEnemy);

                        if (cooldown > 3.0f) {
                            fired = true;
                            friendlySoldiers[i].Move(closestEnemy);
                        }
                    }
                }
            }
            if (fired) {
                fired = false;
                cooldown = 0;
            }
        }

        public void ToggleParition() {
            partitionToggle = !partitionToggle;
        }

        private Soldier FindClosestEnemySlow(Soldier soldier) {
            Soldier closestEnemy = null;
            float bestDistSqr = Mathf.Infinity;

            for (int i = 0; i < enemySoldiers.Count; i++) {
                float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;

                if (distSqr < bestDistSqr) {
                    bestDistSqr = distSqr;
                    closestEnemy = enemySoldiers[i];
                }
            }

            return closestEnemy;
        }

        public void RemoveEnemy(Soldier soldier) {
            if (soldier == null) {
                return;
            }

            grid.Remove(soldier);
            enemySoldiers.Remove(soldier);
            friendlySoldiers.Remove(soldier);

            if (soldier.soldierObject != null) {
                Destroy(soldier.soldierObject);
            }
        }

        public void SpawnEnemy() {
            for (int i = 0; i < 25; i++) {
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));
                newEnemy.transform.parent = enemyParent;
            }
        }
    }
}
