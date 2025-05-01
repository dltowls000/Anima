using UnityEngine;

public class StatusButton : MonoBehaviour
{
    public GameObject image;

    public void onClicked() 
    {
        if (!image.activeSelf)
        {
            image.SetActive(true);
        }
        else
        {
            image.SetActive(false);
        }
    }

}
