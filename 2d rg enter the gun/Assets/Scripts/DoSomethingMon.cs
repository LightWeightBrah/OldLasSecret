using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSomethingMon : MonoBehaviour
{
    void OnEnable()
    {
        WMBroadcaster.OnMonetizationStart += OnMonetizationStart;
        WMBroadcaster.OnMonetizationProgress += OnMonetizationProgress;
    }

    void OnDisable()
    {
        WMBroadcaster.OnMonetizationStart -= OnMonetizationStart;
        WMBroadcaster.OnMonetizationProgress -= OnMonetizationProgress;
    }

    void OnMonetizationStart(Dictionary<string, object> detail)
    {

    }

    void OnMonetizationProgress(Dictionary<string, object> detail)
    {

    }
}
