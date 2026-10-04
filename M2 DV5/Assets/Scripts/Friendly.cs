using UnityEngine;
using System.Collections;

namespace SpatialPartitionPattern {
    public class Friendly : Soldier {
        public Friendly(GameObject soldierObj, float mapWidth) {
            soldierObject = soldierObj;
            this.soldierTrans = soldierObj.transform;
            // this.walkSpeed = 2f;
        }

        public override void Move(Soldier closestEnemy) {
            soldierTrans.rotation = Quaternion.LookRotation(closestEnemy.soldierTrans.position - soldierTrans.position);
            // soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
            Shoot(closestEnemy);
        }

        private void Shoot(Soldier soldier) {
            GameObject prefab = Resources.Load<GameObject>("Bullet");
            GameObject bullet = Object.Instantiate(prefab, soldierTrans.position, soldierTrans.rotation);
            bullet.GetComponent<Bullet>().target = soldier;
            Debug.Log("shooting");
        }
    }
}