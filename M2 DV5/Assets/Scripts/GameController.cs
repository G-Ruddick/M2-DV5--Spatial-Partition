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

        private List<Soldier> enemySoldiers = new List<Soldier>();
        private List<Soldier> friendlySoldiers = new List<Soldier>();
        private List<Soldier> closestEnemies = new List<Soldier>();

        private float mapWidth = 50f;
        private int cellSize = 10;
        private int numberOfSoldiers = 100;

        private Grid grid;

        private void Start() {
            grid = new Grid((int)mapWidth, cellSize);

            for (int i = 0; i < numberOfSoldiers; i++) {
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));
                newEnemy.transform.parent = enemyParent;
                
                randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;
                friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));
                newFriendly.transform.parent = friendlyParent;
            }
        }

        private void Update() {
            updateTime += Time.deltaTime;
            updateTimeText.text = "Time in Update: " + updateTime.ToString("F2");

            for (int i = 0; i < enemySoldiers.Count; i++) {
                enemySoldiers[i].Move();
            }
            
            for (int i = 0; i < closestEnemies.Count; i++) {
                closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
            }

            closestEnemies.Clear();

            for (int i = 0; i < friendlySoldiers.Count; i++) {
                if (partitionToggle) {
                    Soldier closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);
                    
                    if (closestEnemy != null) {
                        closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;
                        closestEnemies.Add(closestEnemy);
                        friendlySoldiers[i].Move(closestEnemy);
                    }   
                }
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
    }
}
