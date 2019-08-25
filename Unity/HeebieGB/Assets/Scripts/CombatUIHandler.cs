﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIHandler : MonoBehaviour
{
    public List<UIBeat> beatSprites = new List<UIBeat>();
    private int spriteIdx = 0;
    private int recentBeatIdx = -1;

    public RectTransform beatStart;
    public RectTransform beatEnd;
    private float goalHeight = -1;
    private float startHeight = -1;

    [SerializeField]
    private Image enemyHealth;
    [SerializeField]
    private Image playerHealth;


    // Start is called before the first frame update
    void Start()
    {
        if (beatStart != null && beatEnd != null)
        {
            startHeight = beatStart.position.y;
            goalHeight = beatEnd.position.y;
        }

        CombatCoordinator.Instance.BeatTrackNoteCall += PlaceNote;
        CombatCoordinator.Instance.NoMoreBeatsCall += CleanExtras;
        CombatCoordinator.Instance.AdvanceIndexCall += AdvanceQueueIndex;
    }


    public void PlaceNote(int beatIdx, CombatNote note)
    {
        Debug.Log($"Placing beat: {beatIdx}");
        CombatCoordinator cc = CombatCoordinator.Instance;
        recentBeatIdx = beatIdx;
        int idx = (beatIdx + spriteIdx) % beatSprites.Count;
        float progress = (note.Status == 0 ? cc.SongProgress : (cc.SongProgress < note.PlayTime ? cc.SongProgress : (note.LockTime < note.PlayTime ? note.PlayTime : note.LockTime)));
        beatSprites[idx].transform.position = new Vector2(beatStart.position.x,
            ((1 - (note.PlayTime - progress) / cc.noteTrackPeriod) * (goalHeight - startHeight)) + startHeight);
        beatSprites[idx].SetSprite(note);
    }

    public void CleanExtras()
    {
        Debug.Log($"Cleaning from beat {recentBeatIdx}.");
        for (int i = recentBeatIdx + 1; i < beatSprites.Count; ++i)
        {
            beatSprites[(spriteIdx + i) % beatSprites.Count].CleanUI();
        }
    }

    public void AdvanceQueueIndex()
    {
        spriteIdx++;
        spriteIdx %= beatSprites.Count;
    }
}