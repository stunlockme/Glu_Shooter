using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    private float fillAmount;
    public float lerpSpeed;
    public Image content;
    public Color fullColour;
    public Color lowColour;
    public bool lerpColours;

    public float MaxValue { get; set; }
    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, MaxValue, 0, 1.0f);
        }
    }

    private void Start()
    {
        if(lerpColours)
        {
            content.color = fullColour;
        }
    }

    private void Update()
    {
        HandleBar();    
    }

    private void HandleBar()
    {
        if(fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, lerpSpeed * Time.deltaTime);
        }

        if(lerpColours)
        {
            content.color = Color.Lerp(lowColour, fullColour, fillAmount);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
