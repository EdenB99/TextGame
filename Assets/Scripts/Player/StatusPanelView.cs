using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusPanelView : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public Slider hpBar;
    public TextMeshProUGUI spText;
    public Slider spBar;
    public TextMeshProUGUI mpText;
    public Slider mpBar;

    public TextMeshProUGUI strText;
    public TextMeshProUGUI dexText;
    public TextMeshProUGUI intText;

    // 상태 갱신 메서드
    public void UpdateStatus(PlayerStatus status)
    {
        hpText.text = $"{status.hp} / {status.maxHp}";
        hpBar.value = (float)status.hp / status.maxHp;
        spText.text = $"{status.sp} / {status.maxSp}";
        spBar.value = (float)status.sp / status.maxSp;
        mpText.text = $"{status.mp} / status.maxMp";
        mpBar.value = (float)status.mp / status.maxMp;

        strText.text = status.str.ToString();
        dexText.text = status.dex.ToString();
        intText.text = status.intel.ToString();
    }
}