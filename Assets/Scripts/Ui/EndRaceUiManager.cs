using System.Collections.Generic;
using DesignPattern;
using UnityEngine;

public class EndRaceUiManager : Factory<EndRaceUiManager>
{
    public GameObject RankingTable;
    public int VehicleNumber = 6;
    private int _contestantCount;
    private List<RankingEntryBehaviour> _rankingEntries;
    public const float EntrySize = 100;

    public void Awake()
    {
        if (_rankingEntries != null)
            foreach (RankingEntryBehaviour entry in _rankingEntries)
                Destroy(entry.gameObject);

        _rankingEntries = new List<RankingEntryBehaviour>();

        for (int i = 0; i < VehicleNumber; i++)
        {
            RankingEntryBehaviour entry = Factory<RankingEntryBehaviour>.New("Ui/RankingEntry");
            entry.transform.SetParent(RankingTable.transform, false);
            entry.GetComponent<RectTransform>().anchoredPosition = -Vector2.up * i * EntrySize;
            _rankingEntries.Add(entry);
        }
    }

    public void AddToRanking(Contestant contestant, int lastSplitIndex)
    {
        if (VehicleNumber != _rankingEntries.Count)
            Awake();

        _rankingEntries[_contestantCount].ChangeEntry(_contestantCount + 1, contestant.PlayerName,
            contestant.SplitTimes[lastSplitIndex]);
        _contestantCount++;
    }
}