using UnityEngine;
using GameKit;

public class MapRegulator : Regulator<MapRegulator>
{
    public bool gamePaused = false;
    
    private int dialogUICount = 0;
    public bool DialogUIActive => dialogUICount > 0;
    public void DialogIn() => dialogUICount += 1;
    public void DialogOut() => dialogUICount -= 1;
    public void ReportDialogSetActive(bool active) => dialogUICount += active ? 1 : -1;
}
