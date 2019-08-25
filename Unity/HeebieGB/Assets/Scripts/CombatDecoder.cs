using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CombatDecoder : MonoBehaviour
{
    #region singleton
    private CombatDecoder() { }
    private static CombatDecoder instance = null;
    public static CombatDecoder Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public int beats = -1;
    public int beatType = -1;
    public int bpm = -1;
    public float beatLength = -1;
    public float rotationLength = -1;

    private int BPM
    {
        set
        {
            bpm = value;
            beatLength = 60.0f / bpm;
        }
    }

    public List<CombatNote> noteQueue = new List<CombatNote>();

    public IEnumerator DecodeFile(string path)
    {
        Debug.Log("Starting Decode.");
        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();
        if (text != null && text.Length != 0)
        {
            Debug.Log("File open.");
            yield return StartCoroutine(DecodeRoutine(text));
        }
        else
        {
            Debug.LogError("File failed.");
        }
    }

    private IEnumerator DecodeRoutine(string text)
    {
        float lastCheck = Time.time;
        float frameLimit = 1.0f / 120.0f;
        string[] bits = text.Split(';');
        int currentBar = -1;
        bool decodingSuccessful = false;

        int framesUsed = 1;

        for (int i = 0; i < bits.Length; ++i)
        {
            if (i == 0)
            {
                string[] info = bits[i].Split(',');
                if (int.TryParse(info[0], out beats) && int.TryParse(info[1], out beatType))
                {
                    beatType /= 4;
                }
                else
                {
                    Debug.LogError("No time signiature.");
                    i = bits.Length;
                }
            }
            else if (i == 1)
            {
                int tempBpm = -1;
                if (int.TryParse(bits[i], out tempBpm))
                {
                    BPM = tempBpm;
                }
                else
                {
                    Debug.LogError("No BPM.");
                    i = bits.Length;
                }
            }
            else if (i == 2)
            {
                if (bits[i] != "s")
                {
                    Debug.LogError("No start");
                    i = bits.Length;
                }
            }
            else
            {
                string[] info = bits[i].Split(',');
                switch (info[0])
                {
                    case "b": // Bar
                        int tempBar = -1;
                        if (int.TryParse(info[1], out tempBar))
                        {
                            if (currentBar < tempBar)
                            {
                                currentBar = tempBar;
                            }
                            else
                            {
                                Debug.LogError("Bars going backwards.");
                                i = bits.Length;
                            }
                        }
                        break;
                    case "a": // Attack
                        {
                            float timeToNote = TimeToNote(currentBar, info[1], info[2]);
                            if (timeToNote != -1)
                            {
                                AddNoteToQueue(MakeRandomBasicAttack(), timeToNote);
                            }
                            break;
                        }
                    case "d": // Defend
                        {
                            float timeToNote = TimeToNote(currentBar, info[1], info[2]);
                            if (timeToNote != -1)
                            {
                                AddNoteToQueue(MakeRandomDefence(), timeToNote);
                            }
                            break;
                        }
                    case "e": // Exit
                        decodingSuccessful = true;
                        i = bits.Length;
                        break;
                    default: // Something else?
                        break;
                }
            }


            // Make sure decoding doesn't occupy too long
            if (Time.time - lastCheck > frameLimit)
            {
                yield return null;
                lastCheck = Time.time;
                framesUsed++;
            }
        }

        if (decodingSuccessful)
        {
            // ~~~ something
            rotationLength = currentBar * beats * beatLength;
            CombatNote.loopLength = rotationLength;
            Debug.Log($"Frames to read {framesUsed}");
        }
    }

    private EnumAttackType MakeRandomBasicAttack()
    {
        int val = Random.Range(0, 2);
        switch (val)
        {
            case 0:
                return EnumAttackType.AtkA;
            case 1:
                return EnumAttackType.AtkB;
            case 2:
                return EnumAttackType.AtkEither;
            default:
                return EnumAttackType.AtkEither;
        }
    }

    private EnumAttackType MakeRandomDefence()
    {
        int val = Random.Range(0, 2);
        switch (val)
        {
            case 0:
                return EnumAttackType.DefUp;
            case 1:
                return EnumAttackType.DefLeft;
            case 2:
                return EnumAttackType.DefDown;
            default:
                return EnumAttackType.DefRight;
        }
    }

    // Creates notes in the queue
    private void AddNoteToQueue(EnumAttackType type, float timeTill)
    {
        noteQueue.Add(new CombatNote(type, timeTill));
        CombatCoordinator.Instance.noteQueue.Add(new CombatNote(type, timeTill));
    }

    private float TimeToNote(int bar, string count, string type)
    {
        float num = -1;
        float denom = -1;
        if (float.TryParse(count, out num) && float.TryParse(type, out denom))
        {
            return (beats * (bar - 1) + (num - 1) / denom) * beatLength;
        }
        else
        {
            Debug.LogError("Invalid note information.");
            return -1;
        }
    }

}
