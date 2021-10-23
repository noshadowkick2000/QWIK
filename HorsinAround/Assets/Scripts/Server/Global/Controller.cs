using UnityEngine;

namespace Server.Global
{
    public abstract class Controller : MonoBehaviour
    {
        protected int Id;

        private void Awake()
        {
            SetId(GetComponent<Player>().SetController(this));
        }

        private void SetId(int newId)
        {
            Id = newId;
        }

        public abstract void TapDown();
        public abstract void TapUp();
        public abstract void Up();
        public abstract void Down();
        public abstract void Left();
        public abstract void Right();
    }
}
