using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/DropItemConfig", fileName = "DropItemConfig", order = 15)]
public class DropItemConfig : ScriptableObject
{
    [Header("金币")]
    public AnimationCurve CoinDropCurve;
    public float CoinDropDuration;
    public AnimationCurve CoinFlyCurve;
    public float CoinFlyDuration;
    public int CoinDropNum;
    public float CoinInterval;
    public Vector2 HeightRange;
    public float Area;
    public int RotateNum;
    public float RotateDuration;
    public float CoinFlyMoveBackValue;
    public float CoinFlyMoveBackDuration;

    [Header("经验")]
    public AnimationCurve ExpDropCurve;
    public float ExpDropDuration;
    public AnimationCurve ExpFlyCurve;
    public float ExpFlyDuration;
    public int ExpDropNum;
    public float ExpInterval;
    public Vector2 ExpHeightRange;
    public float ExpArea;
    public int ExpRotateNum;
    public float ExpRotateDuration;
    public float ExpFlyMoveBackValue;
    public float ExpFlyMoveBackDuration;

    [Header("分解装备")]
    public Vector2 EquipMoveLength;
    public float EquipMoveDuration;

    // 掉落部分
    public AnimationCurve EquipDropCurve;
    public float EquipDropDuration;
    public Vector2 UpHeight;
    public Vector2 DownHeight;
    public Vector2 EquipDropArea;
    public int EquipCoinNum;
    public int EquipExpNum;

    // 旋转部分
    public int EquipRotateNum;
    public float EquipRotateDuration;

    // 弹跳部分
    public AnimationCurve EquipJumpCurve;
    public float EquipJumpDuration;
    public Vector2 EquipJumpRange;
    public float JumpPower;
    public int JumpNum;

    // Fade部分
    public AnimationCurve EquipFadeCurve;
    public float EquipFadeDelay;
    public float EquipFadeDuration;
    public float EquipFadeEndValue;
    public float EquipFadeHeight;
}
