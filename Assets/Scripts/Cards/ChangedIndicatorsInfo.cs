using UnityEngine;

public class ChangedIndicatorsInfo : MonoBehaviour
{
    public enum IndicatorType { Health, Wealth, Happiness }
    public IndicatorType[] AffectedIndicators;
    public int[] IndicatorChanges;

    public void SetEffects(IndicatorType[] indicators, int[] changes)
    {
        AffectedIndicators = indicators;
        IndicatorChanges = changes;
    }
}
