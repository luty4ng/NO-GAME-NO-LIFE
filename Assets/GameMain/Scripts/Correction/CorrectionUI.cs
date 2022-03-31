using UnityEngine;
using UnityEngine.UI;
using GameKit;

public class CorrectionUI : MonoBehaviour
{
    public Slider offsetSlider;
    public ParserC parserC;
    public TrackC trackC;
    public void OnSliderValueChange()
    {
        Rhythm.rhythmOffset = Rhythm.minOffset + offsetSlider.value * (Rhythm.maxOffset - Rhythm.minOffset);
    }

    public void Return()
    {
        Scheduler.instance.UnloadSceneSwipe("Level Correction");
    }
}