using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumCombatState
{
    Nothing = -1, Ongoing, Victory, Defeat
}

public class CombatCoordinator : MonoBehaviour
{

    #region singleton
    private CombatCoordinator() { }
    private static CombatCoordinator instance = null;
    public static CombatCoordinator Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public List<CombatNote> noteQueue = new List<CombatNote>();
    public int queueHeadIdx = 0;
    public CombatNote NoteQueueItem(int idx)
    {
        return noteQueue[(queueHeadIdx + idx) % noteQueue.Count];
    }

    public delegate void CombatInformation(EnumAttackType action, bool successful);
    public CombatInformation CombatInfoCall;

    public delegate void AnimationSpeed(float beatLength);
    public AnimationSpeed AnimationSpeedCall;

    public delegate void BeatTrackNote(int beatIdx, CombatNote note);
    public BeatTrackNote BeatTrackNoteCall;

    public delegate void AdvanceIndex();
    public AdvanceIndex AdvanceIndexCall;

    public delegate void NoMoreBeats();
    public NoMoreBeats NoMoreBeatsCall;

    public delegate void CombatOver();
    public CombatOver CombatOverCall;

    private float songProgress = -1;
    public float SongProgress { get { return songProgress; } }
    public float noteTrackPeriod = -1;
    private EnumCombatState combatState = EnumCombatState.Ongoing;

    private bool playing = false;

    public void Victory()
    {
        combatState = EnumCombatState.Victory;
        GameManager.GameWin();
    }
    public void Defeat()
    {
        combatState = EnumCombatState.Defeat;
        GameManager.GameOver();
    }

    public IEnumerator BeginCombat(string enemy)
    {
        combatState = EnumCombatState.Ongoing;
        yield return StartCoroutine(CombatDecoder.Instance.DecodeFile("Assets/Resources/CombatTiming/" + enemy + ".txt"));
        Debug.Log("Read successful");
        // ~~~ Bar of lead in
        // ~~~ Begin combat proper
        yield return StartCoroutine(CombatLoop());
        CombatOverCall?.Invoke();
    }

    private bool NoteInRange(CombatNote note)
    {
        return note.PlayTime - songProgress < noteTrackPeriod;
    }

    private void ResolveAction(EnumAttackType action, bool success)
    {
        CombatInfoCall?.Invoke(action, success);
    }

    private IEnumerator CombatLoop()
    {
        AnimationSpeedCall?.Invoke(CombatDecoder.Instance.beatLength);
        songProgress = -CombatDecoder.Instance.beatLength * CombatDecoder.Instance.beats;
        noteTrackPeriod = CombatDecoder.Instance.beatLength * 3.0f;
        while (true)
        {
            if (!playing && songProgress >= 0.0f)
            {
                GetComponent<AudioSource>()?.Play();
                playing = true;
            }

            // input resolution
            EnumAttackType btnInput = EnumAttackType.None;

            if (InputManager.AButton)
            {
                btnInput = EnumAttackType.AtkA;
            }
            else if (InputManager.BButton)
            {
                btnInput = EnumAttackType.AtkB;
            }
            else if (InputManager.LeftArrow)
            {
                btnInput = EnumAttackType.DefLeft;
            }
            else if (InputManager.UpArrow)
            {
                btnInput = EnumAttackType.DefUp;
            }

            // Input response
            Debug.Log($"Found input: {btnInput.ToString()}");

            for (int i = 0; i < noteQueue.Count; ++i)
            {
                if (NoteQueueItem(i).Status == 0)
                {
                    if (NoteInRange(NoteQueueItem(i)))
                    {
                        if (btnInput != EnumAttackType.None || NoteQueueItem(i).TimeOutCheck(songProgress))
                        {
                            Debug.Log("Note in range.");
                            NoteQueueItem(i).IsSatisfied(btnInput, songProgress);
                            break;
                        }
                    }
                    else if (songProgress - NoteQueueItem(i).LockTime < 0.1)
                    {
                        break;
                    }
                }
            }

            // Response to beats reaching goal
            for (int i = 0; i < noteQueue.Count; ++i)
            {
                CombatNote note = NoteQueueItem(i);
                if (songProgress > note.PlayTime)
                {
                    if (!note.Resolved && note.Status != 0)
                    {
                        // Note resolution
                        ResolveAction(note.AtkType, !note.Failed);
                        note.Resolved = true;
                    }
                }
                else
                {
                    break;
                }
            }

            // Check for combat finish
            if (combatState != EnumCombatState.Ongoing)
            {
                // ~~~ End combat things
                break;
            }

            // Render beats
            for (int i = 0; i < noteQueue.Count; ++i)
            {
                if (NoteInRange(NoteQueueItem(i)))
                {
                    if (songProgress - NoteQueueItem(i).PlayTime >= CombatDecoder.Instance.beatLength)
                    {
                        Debug.Log("Beat has left end of track.");
                        NoteQueueItem(i).Reincarnate();
                        queueHeadIdx++;
                        AdvanceIndexCall?.Invoke();
                        continue;
                    }
                    if (NoteQueueItem(i).PlayTime < songProgress && NoteQueueItem(i).Status == 0)
                    {
                        Debug.Log("Note close to time out.");
                        if (NoteQueueItem(i).TimeOutCheck(songProgress))
                        {
                            Debug.LogError("A Christmas miracle!");
                        }
                    }
                    BeatTrackNoteCall(i, NoteQueueItem(i));
                }
            }
            NoMoreBeatsCall?.Invoke();

            yield return null;
            songProgress += Time.deltaTime;
        }
    }

    private void RefreshCombat()
    {
        noteQueue.Clear();
        queueHeadIdx = 0;
        songProgress = -1;
        noteTrackPeriod = -1;
        combatState = EnumCombatState.Nothing;
    }

    public void ClearDelegates()
    {
        CombatInfoCall = null;
        AnimationSpeedCall = null;
        BeatTrackNoteCall = null;
        AdvanceIndexCall = null;
        NoMoreBeatsCall = null;
        CombatOverCall = null;
    }

    public void FillDelegates()
    {
        HealthLevels.Instance.SetEnemyHealth(200);
        HealthLevels.Instance.CombatEndVictoryCall += Victory;
        HealthLevels.Instance.CombatEndDefeatCall += Defeat;

        CombatOverCall += RefreshCombat;
    }
}
