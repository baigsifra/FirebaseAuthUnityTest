using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsScreen : MonoBehaviour
{
    public Image ExpBar;

    public Text storeNameTextField, levelTextField, balanceTextField, ordersCompletedTextField, totalTipsTextField, totalProfitTextField, totalExpTextField;

    void Awake() {
        float currentExp = FirebaseHelper.userData.CurrentExp;
        float maxExp = FirebaseHelper.userData.Level * 200;
        ExpBar.fillAmount = currentExp / maxExp;

        storeNameTextField.text = FirebaseHelper.userData.StoreName;
        levelTextField.text = "Level " + FirebaseHelper.userData.Level;
        balanceTextField.text = "Balance: " + FirebaseHelper.userData.Currency;
        ordersCompletedTextField.text = "Orders Completed: " + FirebaseHelper.userData.CompletedOrders;
        totalTipsTextField.text = "Total Tips: " + FirebaseHelper.userData.TotalTips;
        totalProfitTextField.text = "Total Profit: " + FirebaseHelper.userData.TotalProfit;
        totalExpTextField.text = "Total Experience Earned: " + FirebaseHelper.userData.TotalExp;
    }
}
