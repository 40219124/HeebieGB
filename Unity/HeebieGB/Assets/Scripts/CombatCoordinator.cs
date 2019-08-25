using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public delegate void BeatTrackNote(int beatIdx, CombatNote note);
    public BeatTrackNote BeatTrackNoteCall;

    public delegate void AdvanceIndex();
    public AdvanceIndex AdvanceIndexCall;

    public delegate void NoMoreBeats();
    public NoMoreBeats NoMoreBeatsCall;

    private float songProgress = -1;
    public float SongProgress { get { return songProgress; } }
    public float noteTrackPeriod = -1;
    private EnumCombatState combatState = EnumCombatState.Ongoing;

    void Start()
    {
        HealthLevels.Instance.SetEnemyHealth(200);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Victory()
    {
        combatState = EnumCombatState.Victory;
    }
    public void Defeat()
    {
        combatState = EnumCombatState.Defeat;
    }

    public IEnumerator BeginCombat(string enemy)
    {
        yield return StartCoroutine(CombatDecoder.Instance.DecodeFile("Assets/Resources/CombatTiming/" + enemy + ".txt"));
        Debug.Log("Read successful");
        // ~~~ Bar of lead in
        // ~~~ Begin combat proper
        yield return StartCoroutine(CombatLoop());
    }

    //private Dictionary<KeyCode, EnumAttackType> inputToOutput = new Dictionary<KeyCode, EnumAttackType> {
    //    { KeyCode.X, EnumAttackType.AtkA }, { KeyCode.C, EnumAttackType.AtkB },
    //    { KeyCode.UpArrow, EnumAttackType.DefUp }, { KeyCode.LeftArrow, EnumAttackType.DefLeft }
    //};
    //private List<KeyCode> inputs = new List<KeyCode> { KeyCode.X, KeyCode.C, KeyCode.UpArrow, KeyCode.LeftArrow };

    private void ResolveAction(EnumAttackType action, bool success)
    {
        CombatInfoCall?.Invoke(action, success);
    }

    private IEnumerator CombatLoop()
    {
        songProgress = -CombatDecoder.Instance.beatLength * CombatDecoder.Instance.beats;
        noteTrackPeriod = CombatDecoder.Instance.beatLength * 3.0f;
        while (true)
        {
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
                            ResolveAction(NoteQueueItem(i).AtkType, NoteQueueItem(i).IsSatisfied(btnInput, songProgress));
                            break;
                        }
                    }
                    else if (songProgress - NoteQueueItem(i).LockTime < 0.1)
                    {
                        break;
                    }
                }
            }

            if (combatState != EnumCombatState.Ongoing)
            {
                // ~~~ End combat things
            }

            // advance notes/animations
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

    private bool NoteInRange(CombatNote note)
    {
        return note.PlayTime - songProgress < noteTrackPeriod;
    }
}
