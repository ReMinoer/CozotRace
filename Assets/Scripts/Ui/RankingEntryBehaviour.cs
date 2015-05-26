using System;
using DesignPattern;
using UnityEngine;
using UnityEngine.UI;

public class RankingEntryBehaviour : Factory<RankingEntryBehaviour>
{
    public Text Position;
    public Text Name;
    public Text Chrono;

    public void ChangeEntry(int position, string playerName, TimeSpan chrono)
    {
        Position.text = GetPositionLabel(position);
        Name.text = playerName;
        Chrono.text = GetChronometerText(chrono, true);
    }

    private static string GetPositionLabel(int position)
    {
        if (position == 1)
            return "1st";
        if (position == 2)
            return "2nd";
        
        return position + "th";
    }

    static private string GetChronometerText(TimeSpan timeSpan, bool alwaysDisplayMinutes, bool displaySignPlus = false)
    {
        string sign = timeSpan.Ticks >= 0 ? (displaySignPlus ? "+" : "") : "-";

        TimeSpan tempSpan = timeSpan;
        if (tempSpan.Ticks < 0)
            tempSpan = tempSpan.Negate();

        if (Mathf.Abs(tempSpan.Minutes) >= 10)
            return sign + string.Format("{0:00}:{1:00}.{2:000}",
                                        tempSpan.Minutes,
                                        tempSpan.Seconds,
                                        tempSpan.Milliseconds);

        if (Mathf.Abs(tempSpan.Minutes) >= 1 || alwaysDisplayMinutes)
            return sign + string.Format("{0:0}:{1:00}.{2:000}",
                                        tempSpan.Minutes,
                                        tempSpan.Seconds,
                                        tempSpan.Milliseconds);

        if (Mathf.Abs(tempSpan.Seconds) >= 10)
            return sign + string.Format("{0:00}.{1:000}",
                                        tempSpan.Seconds,
                                        tempSpan.Milliseconds);

        return sign + string.Format("{0:0}.{1:000}",
                                        tempSpan.Seconds,
                                        tempSpan.Milliseconds);
    }
}
