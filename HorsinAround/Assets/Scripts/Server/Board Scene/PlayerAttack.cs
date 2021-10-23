using UnityEngine;

namespace Server.Board_Scene
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Transform debugHitBox;
    
        private Collider[] buffer = new Collider[5];

        private LayerMask objectLayerMask; 

        private void Awake()
        {
            objectLayerMask = LayerMask.GetMask("Objects");
        }

        public void DebugAttack()
        {
            Attack(debugHitBox, 1);
        }

        public void Attack(Transform hitBox, int damage)
        {
            int count = Physics.OverlapBoxNonAlloc(hitBox.position, hitBox.localScale, buffer, transform.rotation, objectLayerMask);
            for (int i = 0; i < count; i++)
            {
                buffer[i].GetComponent<Rigidbody>().AddExplosionForce(1000, transform.position, 0, 1);
                buffer[i].GetComponent<Object>().Hit(damage);
            }
        }
    }
}
