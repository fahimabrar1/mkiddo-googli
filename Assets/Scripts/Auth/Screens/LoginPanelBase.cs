using UnityEngine;

public class LoginPanelBase : MonoBehaviour
{
    public int panelID;
    public LoginScreenController loginScreenController;

    public Color failedColor = new(255, 0, 0, 255);
    public Color successColor = new(4, 230, 0, 255);


    public virtual void OnClickPlay()
    {
        loginScreenController.OnClickPlay();
    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            loginScreenController.OnClickBack();
        }
    }

}