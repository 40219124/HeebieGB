using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [SerializeField]
    Transform bottom;
    [SerializeField]
    Transform Top;
    [SerializeField]
    float speed = 0.5f;
    [SerializeField]
    bool horizontal = false;
    GameObject Fade;
    int direction = -1;

    void Awake()
    {
        Fade = GameObject.FindGameObjectWithTag("Fade");
        if (!horizontal)
        {
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(Vector3.forward, new Vector3(0, direction * -1, 0));
            transform.rotation = q;
        }
        else
        {
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(Vector3.forward, new Vector3(direction * -1, 0, 0));
            transform.rotation = q;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameState != GameManager.eGameState.OverWorld)
            return;
        Vector3 pos = transform.position;

        if (!horizontal)
        {
            pos.y += direction * speed * Time.deltaTime;

            if ((pos.y <= bottom.position.y && direction != 1) ||
                (pos.y >= Top.position.y && direction != -1))
            {
                direction *= -1;
                Quaternion q = Quaternion.identity;
                q.SetLookRotation(Vector3.forward, new Vector3(0, direction * -1, 0));
                transform.rotation = q;
            }
        }
        else
        {
            pos.x += direction * speed * Time.deltaTime;

            if ((pos.x <= bottom.position.x && direction != 1) ||
                (pos.x >= Top.position.x) && direction != -1)
            {
                direction *= -1;
                Quaternion q = Quaternion.identity;
                q.SetLookRotation(Vector3.forward, new Vector3(direction * -1, 0, 0));
                transform.rotation = q;
            }
        }

        transform.position = pos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "axe" && Player.attacking)
        {
            StartCoroutine(WaitThenSetHealth(true));
        }

        if (collision.tag == "Player")
        {
            StartCoroutine(WaitThenSetHealth(false));
        }
    }

    IEnumerator<YieldInstruction> WaitThenSetHealth(bool player)
    {
        Fade.SetActive(true);
        Fade.GetComponent<Animator>().Update(0);
        Fade.GetComponent<Animator>().SetTrigger("Fade");
        GameManager.GameState = GameManager.eGameState.Playing;
        StartCoroutine(ScreenChanger.SetActiveWhenLoaded(ScreenChanger.FightScene));

        while (!ScreenChanger.IsSceneLoaded(ScreenChanger.FightScene))
        {
            yield return null;
        }
        Fade.SetActive(false);

        if (player)
        {
            Debug.Log("test");
            HealthLevels.Instance.CurrentEnemyHealth = HealthLevels.Instance.GetTotalEnemyHealth() - (2*HealthLevels.Instance.GetTotalEnemyHealth()) / 10;
        }
        else
        {
            Debug.Log("boost");
            HealthLevels.Instance.CurrentPlayerHealth = HealthLevels.Instance.GetTotalPlayerHealth() - (2*HealthLevels.Instance.GetTotalPlayerHealth()) / 10;
        }

        gameObject.SetActive(false);
        VictoryChecker.EnemiesLeft--;

    }
}
