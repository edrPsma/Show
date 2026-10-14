using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatUtility
    {

        /// <summary>
        /// 获取距离最近的单位
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IEntity GetNearestEntity(Vector3 pos, ICollection<IEntity> entities)
        {
            IEntity nearest = null;
            float dis = float.MaxValue;
            foreach (var entity in entities)
            {
                if (entity.AIAgent.IsDeath) continue;

                float curDis = Mathf.Abs(pos.x - entity.Position.x);
                if (curDis < dis)
                {
                    nearest = entity;
                    dis = curDis;
                }
            }

            return nearest;
        }

        /// <summary>
        /// 获取范围内的单位
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="area"></param>
        /// <param name="entities"></param>
        /// <param name="source"></param>
        public static void GetInAreaEntity(Vector3 pos, float area, List<IEntity> entities, IEntity[] source)
        {
            foreach (var entity in source)
            {
                if (entity.AIAgent.IsDeath) continue;

                if (Mathf.Abs(pos.x - entity.Position.x) <= area)
                {
                    entities.Add(entity);
                }
            }
        }

        /// <summary>
        /// 设置怪物的属性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="data"></param>
        public static void SetMonsterAttribute(IEntity entity, DTLevel_base data)
        {
            for (int i = 0; i < data.Monster_attr.Count; i++)
            {
                entity.AttributeSet.SetValue(data.Monster_attr[i][0], data.Monster_attr[i][1]);
            }

            entity.AttributeSet.ResetHP(DamageFormula.CalMaxHP(entity.AttributeSet), 0, DamageFormula.CalMaxHP(entity.AttributeSet));
        }

        public static void SetMonsterAttribute(IEntity entity, List<List<int>> attr)
        {
            for (int i = 0; i < attr.Count; i++)
            {
                entity.AttributeSet.SetValue(attr[i][0], attr[i][1]);
            }

            long hp = DamageFormula.CalMaxHP(entity.AttributeSet);
            entity.AttributeSet.ResetHP(hp, 0, hp);
        }

        /// <summary>
        /// 设置副本怪物的属性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="data"></param>
        public static void SetMonsterAttribute(IEntity entity, DTCopy_level data)
        {
            for (int i = 0; i < data.Monster_attr.Count; i++)
            {
                entity.AttributeSet.SetValue(data.Monster_attr[i][0], data.Monster_attr[i][1]);
            }

            entity.AttributeSet.ResetHP(DamageFormula.CalMaxHP(entity.AttributeSet), 0, DamageFormula.CalMaxHP(entity.AttributeSet));
        }

        /// <summary>
        /// long 钳制
        /// </summary>
        /// <param name="value">当前值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static long Clamp(long value, long min, long max)
        {
            if (value < min)
            {
                value = min;
            }
            if (value > max)
            {
                value = max;
            }

            return value;
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long CeilToLong(double value)
        {
            return (long)Math.Ceiling(value);
        }
    }
}