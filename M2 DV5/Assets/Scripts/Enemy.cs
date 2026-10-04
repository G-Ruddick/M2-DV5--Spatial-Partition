using UnityEngine;
using System.Collections;

namespace SpatialPartitionPattern {
    public class Enemy : Soldier {
        public GameObject soldierObj;
        private Vector3 currentTarget;
        private Vector3 oldPos;
        private float mapWidth;
        private Grid grid;

        public Enemy(GameObject soldierObj, float mapWidth, Grid grid) {
            soldierObject = soldierObj;
            this.soldierTrans = soldierObj.transform;
            this.soldierMeshRenderer = soldierObj.GetComponent<MeshRenderer>();
            this.mapWidth = mapWidth;
            this.grid = grid;
            this.walkSpeed = 5f;

            grid.Add(this);

            oldPos = soldierTrans.position;

            GetNewTarget();
        }

        public override void Move() {
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);

            grid.Move(this, oldPos);
            oldPos = soldierTrans.position;

            if ((soldierTrans.position - currentTarget).magnitude < 1f) {
                GetNewTarget();
            }
        }

        private void GetNewTarget() {
            currentTarget = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));

            soldierTrans.rotation = Quaternion.LookRotation(currentTarget - soldierTrans.position);
        }
    }
}