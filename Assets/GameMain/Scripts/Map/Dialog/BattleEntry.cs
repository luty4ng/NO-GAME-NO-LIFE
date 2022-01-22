public class BattleEntry : Phase
{
    public static string PhaseType = "BattleEntry";
    public string BattleScene;
    public BattleEntry(string text, string battleScene) : base(PhaseType)
    {
        Text = text;
        BattleScene = battleScene;
    }
}