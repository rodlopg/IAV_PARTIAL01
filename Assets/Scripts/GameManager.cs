using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameOver = false;
    private string message = "";

    void Update()
    {
        if (gameOver) return;

        Peaton[] peatones = FindObjectsOfType<Peaton>();
        Ladron[] ladrones = FindObjectsOfType<Ladron>();

        if (peatones.Length == 0)
        {
            gameOver = true;
            message = "¡Perdiste!";
            Time.timeScale = 0f;
        }
        else if (ladrones.Length == 0)
        {
            gameOver = true;
            message = "¡Ganaste!";
            Time.timeScale = 0f;
        }
    }

    void OnGUI()
    {
        if (gameOver)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 50;
            style.normal.textColor = message == "¡Ganaste!" ? Color.green : Color.red;
            style.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), message, style);
        }
    }
}
