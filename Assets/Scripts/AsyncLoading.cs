using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoading : MonoBehaviour
{
    public static string TargetScene = "Main Menu";

    [SerializeField] private Slider progressBar;

    public static void Load(string sceneName)
    {
        TargetScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private IEnumerator Start()
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(TargetScene);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            if (progressBar != null)
                progressBar.value = operation.progress / 0.9f;
            yield return null;
        }

        if (progressBar != null)
            progressBar.value = 1.0f;

        yield return new WaitForSeconds(0.25f);
        operation.allowSceneActivation = true;
    }
}