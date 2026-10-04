using UnityEngine;
using System.Collections.Generic;

namespace SpatialPartitionPattern {
    public class Bullet : MonoBehaviour {
        public Rigidbody rb;
        public Soldier target;
        
        void Start() {
            rb.AddForce(transform.forward * 75f, ForceMode.Impulse);
            Destroy(this.gameObject, 1.5f);
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Player")) {
                if (other.gameObject != target.soldierObject) { 
                    Destroy(this.gameObject);
                    return; 
                }

                foreach (Soldier soldier in GameController.instance.enemySoldiers) {
                    if (soldier.soldierObject == other.gameObject) {
                        GameController.instance.RemoveEnemy(soldier);
                        Destroy(this.gameObject);
                        return;
                    }
                }
                foreach (Soldier soldier in GameController.instance.friendlySoldiers) {
                    if (soldier.soldierObject == other.gameObject) {
                        GameController.instance.RemoveEnemy(soldier);
                        Destroy(this.gameObject);
                        return;
                    }
                }
            }
        }
    }
}
