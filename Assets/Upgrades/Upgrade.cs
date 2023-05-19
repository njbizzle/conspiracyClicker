using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Upgrade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] UpgradeData upgradeData;
    [SerializeField] GameManager_ gameManager;
    [SerializeField] UpgradeManager upgradeManager;

    [Header("")]
    [SerializeField] CustomButton upgradeButton;
    [SerializeField] TMP_Text upgradeNameDisplay;
    [SerializeField] Image upgradePriceProgressFill;
    [SerializeField] Image upgradeImage;

    [Header("Border")]
    [SerializeField] Image upgradeImageBorder;

    [Space(10)]
    [Header("Border Color")]
    [SerializeField] Color upgradeBorderColor;
    [SerializeField] Color upgradeBorderHoverColor;
    [Space(5)]
    [Header("Purchasable Border Color")]
    [SerializeField] Color upgradePurchasableBorderColor;
    [SerializeField] Color upgradePurchasableBorderHoverColor;
    [Space(10)]
    [Header("Border Fill Color")]
    [SerializeField] Color upgradeBorderFillColor;
    [SerializeField] Color upgradeBorderFillHoverColor;
    [Space(5)]
    [Header("Purchasable Border Fill Color")]
    [SerializeField] Color upgradePurchasableBorderFillColor;
    [SerializeField] Color upgradePurchasableBorderFillHoverColor;
    [Space(10)]
    [Header("Purchased Border Colors")]
    [SerializeField] Color upgradeBorderPurchasedColor;
    [SerializeField] Color upgradeBorderPurchasedHoverColor;

    [Space(10)]
    [SerializeField] Image upgradeBorderFill;
    [SerializeField] float upgradeBorderProgressSpeed;

    [Header("Tooltip")]
    [SerializeField] RectTransform upgradeTooltipTransform;
    [SerializeField] TMP_Text upgradeTooltipText;

    [SerializeField] bool isHovered;
    [SerializeField] bool shown = false;

    public UpgradeData GetUpgradeData(){
        return upgradeData;
    }

    public void SetUpgradeData(UpgradeData newUpgradeData){

        // forces content size fitters to act before the rest of the start method is run
        LayoutRebuilder.ForceRebuildLayoutImmediate(upgradeTooltipText.rectTransform);
        upgradeData = newUpgradeData;

        upgradeNameDisplay.text = "Locked";
        upgradeTooltipText.text = "Locked";

        upgradeImage.sprite = null;
    }

    void ShowUpgrade(){
        upgradeNameDisplay.text = upgradeData.upgradeName;
        upgradeTooltipText.text = upgradeData.tooltipText + "\n\nCost: " + gameManager.Number(upgradeData.upgradeCost);

        upgradeImage.sprite = upgradeData.upgradeImage;
    }

    void HideToolTip(){
        isHovered = false;
        upgradeTooltipTransform.gameObject.SetActive(isHovered);
    }
    void ShowToolTip(){
        isHovered = true;
        upgradeTooltipTransform.gameObject.SetActive(isHovered);
    }

    void OnClick(){
        if (!upgradeData.canPurchase || !shown || upgradeData.purchased) {return;} // price check

        upgradeData.purchased = true;

        gameManager.subtractClout(upgradeData.upgradeCost, upgradeData.conspiracyLevel); // pay
        upgradeManager.MoveToPurchase(GetComponent<RectTransform>()); // move upgrade
        upgradeData.conspiracistData.Upgrade(upgradeData.conspiracistUpgrade, upgradeData.conspiracistUpgradeMultiplier);
    }

    void Start(){
        gameManager = FindObjectOfType<GameManager_>();
        upgradeManager = FindObjectOfType<UpgradeManager>();

        // setup hover on buttons
        upgradeButton.AddOnHoverEnter(ShowToolTip);
        upgradeButton.AddOnHoverExit(HideToolTip);
        upgradeButton.AddOnClick(OnClick);
    }

    void Update(){
        ResizeToolTip();

        // update canPurchase
        upgradeData.canPurchase = gameManager.getClout() >= upgradeData.upgradeCost;

        if (upgradeData.conspiracistData.conspiracistQuantity > 0 && !shown){
            ShowUpgrade();
            shown = true;
        }

        // progress bar
        if (upgradeData.purchased || !shown){
            upgradeBorderFill.fillAmount = 0;
        } else{
            gameManager.progressBarFillCalculate((gameManager.getClout() / upgradeData.upgradeCost), upgradeBorderProgressSpeed, upgradeBorderFill);
        }

        // locking and color changes
        if (isHovered){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            upgradeTooltipTransform.position = new Vector3(mousePos.x, mousePos.y, 0);

            if (upgradeData.purchased || !shown){
                upgradeImageBorder.color = upgradeBorderPurchasedHoverColor;

            } else if (upgradeData.canPurchase){
                upgradeImageBorder.color = upgradePurchasableBorderHoverColor;
                upgradeBorderFill.color = upgradePurchasableBorderFillHoverColor;

            } else{
                upgradeImageBorder.color = upgradeBorderHoverColor;
                upgradeBorderFill.color = upgradeBorderFillHoverColor;
            }
        } else{
            if (upgradeData.purchased || !shown){
                upgradeImageBorder.color = upgradeBorderPurchasedColor;

            } else if (upgradeData.canPurchase){
                upgradeImageBorder.color = upgradePurchasableBorderColor;
                upgradeBorderFill.color = upgradePurchasableBorderFillColor;

            } else{
                upgradeImageBorder.color = upgradeBorderColor;
                upgradeBorderFill.color = upgradeBorderFillColor;
            }
        }
    }

    void ResizeToolTip(){
        upgradeTooltipTransform.sizeDelta = new Vector2(
            upgradeTooltipTransform.sizeDelta.x, 
            upgradeTooltipText.rectTransform.rect.height
        );
    }
}