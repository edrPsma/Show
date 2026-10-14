using System;
using System.Collections;
using System.Collections.Generic;
using Ev;
using UnityEngine;

namespace Combat
{
    public class Buff : Controller
    {
        public IEntity Owner { get; private set; }
        public EntityInfo Caster { get; private set; }
        public DTSkill_buff Data { get; private set; }
        public BindProperty<float> Duration { get; private set; }
        public bool IsActive { get; private set; }
        public Blackboard Blackboard { get; private set; }
        public List<IBuffExcutor> _excutors;
        public float Multiple;// 技能倍率
        public int SkillID;// 技能等级
        public int SkillLevel;// 技能等级

        public Buff()
        {
            _excutors = new List<IBuffExcutor>();
            Duration = new BindProperty<float>();
            Blackboard = new Blackboard();
        }

        public void Init(DTSkill_buff data, EntityInfo caster, IEntity owner)
        {
            Data = data;
            Caster = caster;
            Owner = owner;
        }

        public void Excute()
        {
            IsActive = true;
            Duration.Value = Data.Time;

            IBuffFactory buffFactory = this.GetSystem<IBuffFactory>();
            for (int i = 0; i < Data.BuffEffect.Count; i++)
            {
                IBuffExcutor buffExcutor = buffFactory.GetBuffExcutor(Data.BuffEffect[i]);
                if (buffExcutor != null)
                {
                    _excutors.Add(buffExcutor);
                    buffExcutor.Start(this, i, Data.Parameter[i]);
                }
                else
                {
                    Debug.LogError($"Combat Buff执行器为空,Type: {Data.BuffEffect[i]}");
                    Owner.BuffOwner.RemoveBuff(this);
                }
            }
        }

        public void Update(float deltaTime)
        {
            if (!IsActive) return;

            if ((Data.TimeType == DTSkill_buff.ETimeType.Xian_shi && Duration.Value >= 0) || Data.TimeType != DTSkill_buff.ETimeType.Xian_shi)
            {
                try
                {
                    for (int i = 0; i < _excutors.Count; i++)
                    {
                        _excutors[i]?.Update(this, i, Data.Parameter[i]);
                    }
                    if (Data.TimeType == DTSkill_buff.ETimeType.Xian_shi)
                    {
                        Duration.Value -= deltaTime;
                        if (Duration.Value <= 0)
                        {
                            Over();
                        }
                    }
                    else if (Data.TimeType == DTSkill_buff.ETimeType.B_u_f_f_ti_gong_zhe_reng_zai_ti_gong_b_u_f_f_ji_cun_zai)
                    {
                        if (!Caster.IsValid)
                        {
                            Over();
                        }
                    }
                }
                catch (Exception e) { Debug.LogError(e); }
            }
            else
            {
                Owner.BuffOwner.RemoveBuff(this);
            }
        }

        public void Over()
        {
            if (!IsActive) return;

            IsActive = false;
            for (int i = 0; i < _excutors.Count; i++)
            {
                _excutors[i].Over(this, i, Data.Parameter[i]);
            }
            Owner.BuffOwner.RemoveBuff(this);
        }

        public void Disperse()
        {
            if (!IsActive) return;
            if (Data.CanPurify == DTSkill_buff.ECanPurify.Bu_neng) return;

            IsActive = false;
            for (int i = 0; i < _excutors.Count; i++)
            {
                _excutors[i].Interrupt(this, i, Data.Parameter[i]);
                _excutors[i].Over(this, i, Data.Parameter[i]);
            }
            Owner.BuffOwner.RemoveBuff(this);
        }

        public void OnReset()
        {
            Owner = default;
            Caster = default;
            Duration.UnsubscribeAll();
            Duration.Value = 0;
            IsActive = false;
            Blackboard.Reset();
            _excutors.Clear();
        }
    }
}