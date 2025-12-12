using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private void Start()
    {
        StartCoroutine(ScoreRefreshRoutine());
    }

    private IEnumerator ScoreRefreshRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        while (true)
        {
            int score = GameplayManager.Instance.GetScore();
            scoreText.text = "Score: " + score.ToString();

            yield return delay;
        }
    }
}
