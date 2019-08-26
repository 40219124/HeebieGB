using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Transform trans;
    Animator anim;
    [SerializeField]
    float speed = 1;
    public static bool attacking = false;

    // Start is called before the first frame update
    void Awake()
    {
        trans = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameState != GameManager.eGameState.OverWorld)
            return;

        Vector3 pos = trans.position;

        if (InputManager.AButton || InputManager.BButton)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(WaitFrameForAttack());
        }
        else if (InputManager.UpArrowHeld)
        {
            attacking = false;
            anim.SetTrigger("Walk");
            pos.y += speed * Time.deltaTime;
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(Vector3.forward, Vector3.up);
            transform.rotation = q;
        }
        else if (InputManager.DownArrowHeld)
        {
            attacking = false;
            anim.SetTrigger("Walk");
            pos.y -= speed * Time.deltaTime;
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(Vector3.forward, Vector3.down);
            transform.rotation = q;
        }
        else if (InputManager.LeftArrowHeld)
        {
            attacking = false;
            anim.SetTrigger("Walk");
            pos.x -= speed * Time.deltaTime;
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(Vector3.forward, Vector3.left);
            transform.rotation = q;
        }
        else if (InputManager.RightArrowHeld)
        {
            attacking = false;
            anim.SetTrigger("Walk");
            pos.x += speed * Time.deltaTime;
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(Vector3.forward, Vector3.right);
            transform.rotation = q;
        }
        else
        {
            attacking = false;
            anim.SetTrigger("Idle");
        }

        trans.position = pos;
    }

    IEnumerator<YieldInstruction> WaitFrameForAttack()
    {
        yield return null;
        attacking = true;
    }
}
