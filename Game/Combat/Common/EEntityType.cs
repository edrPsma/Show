using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public enum EEntityType
    {
        /// <summary>
        /// 英雄
        /// </summary>
        Hero,

        /// <summary>
        /// 随从
        /// </summary>
        Entourage,

        /// <summary>
        /// 杂鱼
        /// </summary>
        TrashFish,

        /// <summary>
        /// Boss
        /// </summary>
        Boss,

        /// <summary>
        /// 公会Boss
        /// </summary>
        GuildBoss,

        /// <summary>
        /// 玩家召唤物
        /// </summary>
        Summon,

        /// <summary>
        /// Pvp
        /// </summary>
        Pvp,

        /// <summary>
        /// 显示用
        /// </summary>
        Viewer,

        /// <summary>
        /// 分身
        /// </summary>
        Doppelganger,
    }
}

