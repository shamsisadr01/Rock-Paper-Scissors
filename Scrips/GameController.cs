using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Slider timer;
    [SerializeField] private Text labelTime;

    [SerializeField] private float speedRotate = 150f;
    [SerializeField] private Transform ready;

    [SerializeField] private Animation playerAnim;
    [SerializeField] private Animation aiAnim;

    [SerializeField] private Text player_Score_Text;
    [SerializeField] private Text ai_Score_text;

    [SerializeField] private Sprite[] handsMode;

    private bool startGame = false;
    private int playerState = 0;

    private int playerScore;
    private int aiScore;

    private IEnumerator Start()
    {
        float time = 3f;

        timer.value = time;
        labelTime.text = time.ToString();

        while (time > 0)
        {
            timer.value = time / 3f;
            labelTime.text = Mathf.RoundToInt(time) + "";
            time -= Time.deltaTime;
            yield return null;
        }

        labelTime.text = "Start Game";
        startGame = true;

        while (playerState == 0)
        {
            ready.Rotate(Vector3.forward * Time.deltaTime * speedRotate, Space.World);

            yield return null;
        }

        ready.gameObject.SetActive(false);
        playerAnim.gameObject.SetActive(true);
        aiAnim.gameObject.SetActive(true);

        ShowResults(playerAnim,handsMode[playerState-1]);
        int aiState = Random.Range(0, handsMode.Length);
        ShowResults(aiAnim,handsMode[aiState]);

        yield return new WaitForSeconds(0.5f);

        CheckedWin(aiState + 1);


        yield return new WaitForSeconds(1.5f);

        ResetGame();

        StartCoroutine(Start());
    }

    private void ShowResults(Animation anim,Sprite sprite)
    {
        Image playerSprite = anim.GetComponent<Image>();
        playerSprite.sprite = sprite;

        anim.Play();
    }

    private void CheckedWin(int aiState)
    {
        for (int i = 1; i < handsMode.Length + 1; i++)
        {
            if (i == playerState && aiState == (i % 3 + 1))
            {
                print("Player Win");
                playerScore++;
                break;
            }
            
            if (i == aiState && playerState == (i % 3 + 1))
            {
                print("AI Win");
                aiScore++;
                break;
            }
        }

        player_Score_Text.text = playerScore.ToString();
        ai_Score_text.text = aiScore.ToString();
    }

    private void ResetGame()
    {
        ready.gameObject.SetActive(true);
        playerAnim.gameObject.SetActive(false);
        aiAnim.gameObject.SetActive(false);

        RectTransform PlayerRect = playerAnim.GetComponent<RectTransform>();
        PlayerRect.anchoredPosition = new Vector2 (250, 0);

        RectTransform aiRect = aiAnim.GetComponent<RectTransform>();
        aiRect.anchoredPosition = new Vector2(-20, 0);

        startGame = false;
        playerState = 0;
    }

    public void ModeSelection(int mode)
    {
        if(startGame)
        {
            playerState = mode + 1;
        }
    }
}
