using UnityEngine;
using SonicBloom.Koreo;
public class EventSubscriber : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Koreographer.Instance.RegisterForEvents("MagicksTrack", FireEventDebugLog);
    }
    void FireEventDebugLog(KoreographyEvent koreoEvent)
    {
        
        // koreoEvent.
        // Debug.Log("WTFFFFFFFFFFFF");
        if(!koreoEvent.IsOneOff() && koreoEvent.HasTextPayload())
        {
            string output = koreoEvent.GetTextValue();
            Debug.Log(output);
        }
    }
}
