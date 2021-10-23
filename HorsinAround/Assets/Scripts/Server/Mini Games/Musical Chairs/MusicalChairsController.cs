using Server.Global;

namespace Server.Mini_Games.Musical_Chairs
{
  public class MusicalChairsController : Controller
  {
    public override void TapDown()
    {
      MusicalChairs.Instance.SitDown(Id);
    }

    public override void TapUp()
    {
      //throw new System.NotImplementedException();
    }

    public override void Up()
    {
      throw new System.NotImplementedException();
    }

    public override void Down()
    {
      throw new System.NotImplementedException();
    }

    public override void Left()
    {
      throw new System.NotImplementedException();
    }

    public override void Right()
    {
      throw new System.NotImplementedException();
    }
  }
}
