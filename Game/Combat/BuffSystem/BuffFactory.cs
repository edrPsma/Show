using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

namespace Combat
{
    [System(typeof(IBuffFactory))]
    public class BuffFactory : AbstractSystem, IBuffFactory
    {
        ObjectPool<Buff> _buffPool;
        protected override void OnInit()
        {
            base.OnInit();
            _buffPool = GameEntry.Pool.GetOrCreatePool<Buff>(EPoolType.Scalable, 30) as ObjectPool<Buff>;
            _buffPool.OnReleaseEvent += buff => buff.OnReset();
        }

        public Buff Create(int buffID, EntityInfo caster, IEntity owner)
        {
            Buff buff = _buffPool.SpawnByType();
            buff.Init(DataTable.GetItem<DTSkill_buff>(buffID), caster, owner);

            return buff;
        }

        public void ReleaseBuff(Buff buff)
        {
            _buffPool.Release(buff);
        }

        public IBuffExcutor GetBuffExcutor(int type)
        {
            return GetBuffExcutor((EBuffExcutorType)type);
        }

        public IBuffExcutor GetBuffExcutor(EBuffExcutorType type)
        {
            return GameEntry.Reflection.Get<IBuffExcutor>(type);
        }
    }
}