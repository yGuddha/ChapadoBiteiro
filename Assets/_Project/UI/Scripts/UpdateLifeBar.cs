using UnityEngine;
using UnityEngine.UI;

public class UpdateLifeBar : MonoBehaviour
{
    public Image FillImage;

    void OnEnable()
    {
        GetComponentInParent<Stats>().OnLifeChange += MimetizePlayerLife;
    }
    void OnDisable()
    {
        GetComponentInParent<Stats>().OnLifeChange -= MimetizePlayerLife;
    }

    private void MimetizePlayerLife(float playerLife)
    {
        FillImage.fillAmount = playerLife / 10;
    }
}
