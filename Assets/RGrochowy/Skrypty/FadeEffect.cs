using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeEffect : MonoBehaviour
{
    public Image fadeImage;
    public TextMeshProUGUI fadeText;
    public float fadeDuration = 1f;
    public AudioSource audioSource; // Dodaj zmienn¹ dla AudioSource
    public AudioClip deathSound; // Dodaj zmienn¹ dla klipu dŸwiêkowego

    private void Start()
    {
        // Ustaw pocz¹tkow¹ przezroczystoœæ na 0 (ca³kowicie przezroczysty)
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
    }

    public IEnumerator FadeToBlackAndBack(System.Action callback)
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        // Fade to black
        yield return StartCoroutine(Fade(1f));

        // Wait for a few seconds
        yield return new WaitForSeconds(2f);

        // Call the callback function (e.g., script.Return())
        callback();

        // Fade back to clear
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        // Ustaw przezroczystoœæ obrazu
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
        // Ustaw przezroczystoœæ tekstu
        fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, alpha);
    }
}
