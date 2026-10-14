using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(menuName = "Combat/MapConfig", fileName = "MapConfig", order = 20)]
    public class MapConfig : ScriptableObject
    {
        [Range(1, 1.5f)]
        [LabelText("移动距离(1-1.5)")]
        public float MoveDistance = 1.5f;

        [LabelText("移动时间")]
        public float MoveTime = 2;

        [LabelText("击败boss移动时间")]
        public float VictoryMoveTime = 2;

        [LabelText("单位出生位置偏移")]
        public Vector3 SpawnOffset;

        [LabelText("玩家位置偏移")]
        public Vector3 PlayerOffset;

        [LabelText("宠物位置偏移")]
        public Vector3[] PetOffsets;

        [LabelText("怪物位置偏移")]
        public Vector3[] MonsterOffset;

        /// <summary>
        /// boss位置偏移
        /// </summary>
        public Vector3 BossOffset;

        [Header("公会战")]
        [LabelText("移动范围")]
        public Vector2 MoveRange;
        [LabelText("移动速度")]
        public float MoveSpeed;
    }
}