using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve;

    private string main = "MainScene";


    void Start()
    {
        StartCoroutine(FadeIn());
    }
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(main));
    }
    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(255f, 255f, 255f, a);
            yield return 0;
        }

    }
    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(255f, 255f, 255f, a);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }
    public void Retry()
    {

        FadeTo(SceneManager.GetActiveScene().name);

    }
    public void Next()
    {
        FadeTo(SceneManager.GetActiveScene().name);

    }


}
