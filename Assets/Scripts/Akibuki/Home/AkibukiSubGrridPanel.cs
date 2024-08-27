using UnityEngine;

public class AkibukiSubGrridPanel : MonoBehaviour
{
    public int ID;

    public AkibukiHomeController akibukiHomeController;

    public void OnClickPanel()
    {
        akibukiHomeController.OnClickSubPanel(ID);
    }
}