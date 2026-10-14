using System.Collections;
using System.Collections.Generic;
using Combat;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Prologue
{
    public class PlotConfig : MonoBehaviour
    {
        [ListDrawerSettings(NumberOfItemsPerPage = 10)]
        public List<PlotInfo> Lines;
    }

    [System.Serializable]
    public class PlotInfo
    {
        public EPlotType Type;
        public EButtonState ButtonState;

        [ShowIfGroup(nameof(IfShowWaitTime))]
        public float WaitTime;

        [ShowIfGroup(nameof(IfShowLearnSkillID))]
        public List<int> LearnSkillIDs;


        [ShowIfGroup(nameof(IfShowDialogue))]
        public EDialogueType DialogueType;
        [ShowIf(nameof(IfShowName))][ShowIfGroup(nameof(IfShowDialogue))] public int Name;
        [ShowIfGroup(nameof(IfShowDialogue))] public int Content;


        [ShowIfGroup(nameof(IfShowMove))]
        public float MoveDistance;
        [ShowIfGroup(nameof(IfShowMove))]
        public float MoveTime;


        [ShowIfGroup(nameof(IfSpawnEntity))]
        public EntityGenerator EntityGenerator;

        [ShowIfGroup(nameof(IfTimeLine))]
        public PlayableDirector PlayableDirector;

        [ShowIfGroup(nameof(IfCombat))]
        public float CombatTime;


        [ShowIfGroup(nameof(IfAI))]
        public EntityGenerator AIEntityGenerator;
        [ShowIfGroup(nameof(IfAI))]
        public bool OpenAI;


        [ShowIfGroup(nameof(IfRemoveEntity))]
        public EntityGenerator RemoveEntityGenerator;

        public bool IfShowWaitTime()
        {
            return (Type & EPlotType.Wait) == EPlotType.Wait;
        }

        public bool IfShowLearnSkillID()
        {
            return (Type & EPlotType.LearnSkill) == EPlotType.LearnSkill;
        }

        public bool IfShowDialogue()
        {
            return (Type & EPlotType.Dialogue) == EPlotType.Dialogue;
        }

        public bool IfShowName()
        {
            return DialogueType != EDialogueType.Narration;
        }

        public bool IfShowMove()
        {
            return (Type & EPlotType.Move) == EPlotType.Move;
        }

        public bool IfSpawnEntity()
        {
            return (Type & EPlotType.SpawnEntity) == EPlotType.SpawnEntity;
        }

        public bool IfTimeLine()
        {
            return (Type & EPlotType.TimeLine) == EPlotType.TimeLine;
        }

        public bool IfCombat()
        {
            return (Type & EPlotType.Combat) == EPlotType.Combat;
        }

        public bool IfAI()
        {
            return (Type & EPlotType.AIState) == EPlotType.AIState;
        }

        public bool IfRemoveEntity()
        {
            return (Type & EPlotType.RemoveEntity) == EPlotType.RemoveEntity;
        }
    }
}