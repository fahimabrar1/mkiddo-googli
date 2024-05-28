using UnityEngine;

public class LoginPanelBase : MonoBehaviour
{
    public int panelID;
    public LoginScreenController loginScreenController;


    public virtual void OnClickNext()
    {
        loginScreenController.OnClickNext();
    }

    public virtual void OnClickBack()
    {
        loginScreenController.OnClickBack();
    }

    public virtual void OnClickPlay()
    {
        loginScreenController.OnClickPlay();
    }
}