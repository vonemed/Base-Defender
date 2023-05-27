using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    public TMP_Text moneyUI;

    private void Awake()
    {
        //Subscribe to money changes
        PlayerInventory.Instance.RegisterUIHandler(DisplayMoney);
        //Display start money
        DisplayMoney(PlayerInventory.Instance.GetPlayerMoney());

    }

    public void DisplayMoney(int amount)
    {
        moneyUI.text = amount.ToString();
        
        if (amount != 0) //To get rid of animation at the beginning
        {
            moneyUI.transform.localScale *= 2f;

            moneyUI.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        }
    }
}
