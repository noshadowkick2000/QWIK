using Server.Global;
using UnityEngine;

namespace Debug_resources
{
    public class DebugController : Controller
    {
        public override void TapDown()
        {
            print(Id + ": Tap");
        }

        public override void TapUp()
        {
            print(Id + ": Release");
        }

        public override void Up()
        {
            print(Id + ": Up");
        }

        public override void Down()
        {
            print(Id + ": Down");
        }

        public override void Left()
        {
            print(Id + ": Left");
        }

        public override void Right()
        {
            print(Id + ": Right");
        }
    }
}
