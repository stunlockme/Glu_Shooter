using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField]
    private BarScript barScript;
    public BarScript Barscript
    {
        get { return barScript; }
        set { barScript = value; }
    }
    [SerializeField]
    private float maxValue;
    [SerializeField]
    private float currentValue;

    public float CurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = Mathf.Clamp(value, 0, MaxValue);
            barScript.Value = currentValue;
        }
    }

    public float MaxValue
    {
        get { return maxValue; }
        set
        {
            maxValue = value;
            barScript.MaxValue = maxValue;
        }
    }

    public void Initialize()
    {
        this.MaxValue = maxValue;
        this.CurrentValue = currentValue;
    }

}
