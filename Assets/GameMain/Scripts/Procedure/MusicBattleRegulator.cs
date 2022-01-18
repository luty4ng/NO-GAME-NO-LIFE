using UnityEngine;
using GameKit;

public class MusicBattleRegulator : Regulator<MusicBattleRegulator>
{
    public string pausePanelName = "UI_Settings";
    public RhythmController rhythmController;
    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GetUI(pausePanelName).isActive)
            {
                GetUI(pausePanelName).Show();
                Timer.isPause = true;
                EventManager.instance.EventTrigger<bool>(EventConfig.Game_Pase, true);
            }
            else
            {
                GetUI(pausePanelName).Hide();
                Timer.isPause = false;
                EventManager.instance.EventTrigger<bool>(EventConfig.Game_Pase, false);
            }
        }
    }

    private void Update()
    {
        CheckPause();
    }
}