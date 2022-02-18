using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI fpsText;

    [SerializeField]
    private float refresh;

    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer = timer <= 0 ? refresh : timer -= Time.smoothDeltaTime;
        if (timer <= 0)
        {
            float fps = 1f / Time.smoothDeltaTime;
            fpsText.text = "FPS: " + fps.ToString("0.00");
        }            
    }
}
