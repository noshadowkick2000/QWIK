using UnityEngine;

namespace Server.Board_Scene
{
    public class Object : MonoBehaviour
    {
        [SerializeField] private int baseHp;
        private int hp;

        private void Awake()
        {
            hp = baseHp;
        }

        public void Hit(int damage)
        {
            hp -= damage;

            if (hp <= 0)
                BreakObject();
        }

        private void BreakObject()
        {
            Destroy(gameObject);
        }
    }
}
