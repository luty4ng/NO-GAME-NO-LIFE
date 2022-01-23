using UnityEngine;
using GameKit;

public class MapRegulator : Regulator<MapRegulator>
{
    private int _dialogUICount = 0;
    public bool DialogUIActive => _dialogUICount > 0;
    public void DialogIn() => _dialogUICount += 1;
    public void DialogOut() => _dialogUICount -= 1;
    public void ReportDialogSetActive(bool active) => _dialogUICount += active ? 1 : -1;
}
