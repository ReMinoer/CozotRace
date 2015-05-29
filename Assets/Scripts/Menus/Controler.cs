using UnityEngine;

public class Controler : MonoBehaviour
{
    public int ValueMax;
    public int Value { get; set; }

    public void NextSelection()
    {
        Value += 1;
        if (Value > ValueMax)
            Value = 0;
    }

    public void PreviousSelection()
    {
        Value -= 1;
        if (Value < 0)
            Value = ValueMax;
    }
}