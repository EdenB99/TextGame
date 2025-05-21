using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    public int maxHp = 100;
    public int hp = 100;
    public int maxSp = 50;
    public int sp = 50;
    public int maxMp = 30;
    public int mp = 30;

    public int str = 10;
    public int dex = 10;
    public int intel = 10; // int는 예약어라 intel로 표기

    // 추가: 골드, 레벨, 상태이상 등 필요시 확장
}