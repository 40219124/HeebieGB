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

    #endregion

    public List<CombatNote> noteQueue = new List<CombatNote>();
    public int queueHeadIdx = 0;
    public CombatNote NoteQueueItem(int idx)
    {
        return noteQueue[(queueHeadIdx + idx) % noteQueue.Count];
    }

    public List<RectTransform> beatSprites = new List<RectTransform>();
    public RectTransform beatStart;
    public RectTransform beatEnd;

    private float goalHeight = -1;
    private float startHeight = -1;

    private float songProgress = -1;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        startHeight = beatStart.position.y;
        goalHeight = beatEnd.position.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Victory()
    {

    }

    public IEnumerator BeginCombat(string enemy)
    {
        yield return StartCoroutine(CombatDecoder.Instance.DecodeFile("Assets/Resources/CombatTiming/" + enemy + ".txt"));
        Debug.Log("Read successful");
        // ~~~ Bar of lead in
        // ~~~ Begin combat proper
        yield return StartCoroutine(CombatLoop());
    }

    private Dictionary<KeyCode, EnumAttackType> inputToOutput = new Dictionary<KeyCode, EnumAttackType> {
        { KeyCode.X, EnumAttackType.AtkA }, { KeyCode.C, EnumAttackType.AtkB },
        { KeyCode.UpArrow, EnumAttackType.DefUp }, { KeyCode.LeftArrow, EnumAttackType.DefLeft }
    };
    private List<KeyCode> inputs = new List<KeyCode> { KeyCode.X, KeyCode.C, KeyCode.UpArrow, KeyCode.LeftArrow };

    private void ResolveAction(EnumAttackType action, bool success)
    {
        // ~~~ Set player anim with action/success
        // ~~~ Set enemy anim with action/success

        if (success)
        {
            if (action == EnumAttackType.AtkA || action == EnumAttackType.AtkB)
            {
                // ~~~ damage opponent
            }
        }
        else if (action == EnumAttackType.DefUp || action == EnumAttackType.DefLeft)
        {
            // ~~~ damage player
        }
    }

    private IEnumerator CombatLoop()
    {
        songProgress = 0;
        while (true)
        {
            // ~~~ input resolution
            EnumAttackType btnInput = EnumAttackType.None;
            for (int i = 0; i < inputs.Count; ++i)
            {
                if (Input.GetKeyDown(inputs[i]))
                {
                    btnInput = inputToOutput[inputs[i]];
                    break;
                }
            }

            if (btnInput != EnumAttackType.None)
            {
                ResolveAction(btnInput, NoteQueueItem(0).IsSatisfied(btnInput));
            }

            // ~~~ advance notes/animations
            //if (songProgress > NoteQueueItem(0).PlayTime)
            //{
            //    NoteQueueItem(0).Reincarnate();
            //    queueHeadIdx++;
            //    queueHeadIdx %= noteQueue.Count;
            //}
            //if (NoteQueueItem(0).PlayTime - songProgress < CombatDecoder.Instance.beatLength)
            //{
            //    beatSprites[0].position = new Vector2(beatSprites[0].position.x, (1 - ((NoteQueueItem(0).PlayTime - songProgress) / CombatDecoder.Instance.beatLength)) * (goalHeight - startHeight) + startHeight);
            //}
            for (int i = 0; i < noteQueue.Count && i < beatSprites.Count; ++i)
            {
                if (songProgress > NoteQueueItem(i).PlayTime)
                {
                    NoteQueueItem(i).Reincarnate();
                    queueHeadIdx++;
                }
                if (NoteQueueItem(i).PlayTime - songProgress < CombatDecoder.Instance.beatLength)
                {
                    int spriteIdx = (queueHeadIdx + i) % beatSprites.Count;
                    beatSprites[spriteIdx].position = new Vector2(beatSprites[spriteIdx].position.x, 
                        (1 - ((NoteQueueItem(i).PlayTime - songProgress) / CombatDecoder.Instance.beatLength)) * (goalHeight - startHeight) + startHeight);
                }
                else
                {
                    break;
                }
            }

            yield return null;
            songProgress += Time.deltaTime;
        }
    }
}
