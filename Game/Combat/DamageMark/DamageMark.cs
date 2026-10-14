using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using System.Text;
using Task;

namespace Combat
{
    public class DamageMark : MonoController
    {
        List<Sequence> _tweenSeq = new List<Sequence>();

        public void Show(Vector3 pos, float dir, ref DamageInfo damageInfo)
        {
            string configName = EDamageMarkType.Defult.ToString();
            if (damageInfo.IsDodge)
            {
                configName = EDamageMarkType.Dodge.ToString();
            }
            else if (damageInfo.IsCritical)
            {
                configName = EDamageMarkType.Critical.ToString();
            }
            else if (damageInfo.IsDot)
            {
                configName = EDamageMarkType.Dot.ToString();
            }

            Show(pos, dir, GetAsset<DamageMarkConfig>(configName), ref damageInfo);
        }

        void Show(Vector3 pos, float dir, DamageMarkConfig config, ref DamageInfo damageInfo)
        {
            transform.SetParent(GameEntry.Prefab.SceneNode);
            transform.position = pos;

            SetText(config, ref damageInfo);
            MoveX(config, dir);
            MoveY(config);
            Fade(config);

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCurCombatManager();
            GameEntry.Task.AddTask(DelayToRecycle)
                .SetName("DamageMark DelayToRecycle")
                .Delay(TimeSpan.FromSeconds(config.Duration))
                .SetSpeed(() => combatManager.GameSpeed.Value)
                .Run();
        }

        private void DelayToRecycle(TaskInfo taskInfo)
        {
            foreach (var item in _tweenSeq)
            {
                item.Kill();
            }
            _tweenSeq.Clear();
            IDamageMarkFactory damageMarkFactory = this.GetSystem<IDamageMarkFactory>();
            damageMarkFactory.Release(this);
        }

        void MoveX(DamageMarkConfig config, float dir)
        {
            if (!config.UseMoveX) return;

            Vector3 startPos = transform.position;

            transform.position = startPos + new Vector3(config.StartPosOffsetX, 0, 0) * dir;

            Sequence sequence = DOTween.Sequence();
            sequence
                .Insert(config.MoveXDelay, transform.DOMoveX(startPos.x + config.EndPosOffsetX * dir, config.MoveXDuration))
                .SetEase(config.MoveXCurve);

            _tweenSeq.Add(sequence);

            sequence.Play();
        }

        void MoveY(DamageMarkConfig config)
        {
            if (!config.UseMoveY) return;

            Vector3 startPos = transform.position;

            transform.position = startPos + new Vector3(0, config.StartPosOffsetY, 0);

            Sequence sequence = DOTween.Sequence();
            sequence
                .Insert(config.MoveXDelay, transform.DOMoveY(startPos.y + config.EndPosOffsetY, config.MoveYDuration))
                .SetEase(config.MoveYCurve);

            _tweenSeq.Add(sequence);

            sequence.Play();
        }

        void Fade(DamageMarkConfig config)
        {
            if (!config.UseFade) return;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(Get<TMP_Text>("Content").DOFade(config.StartFade, 0));
            sequence
                .Insert(config.FadeDelay, Get<TMP_Text>("Content").DOFade(config.EndFade, config.FadeDuration))
                .SetEase(config.FadeCurve);

            _tweenSeq.Add(sequence);

            sequence.Play();
        }

        private void SetText(DamageMarkConfig config, ref DamageInfo info)
        {
            var dmgStr = "";
            var color16 = ColorUtility.ToHtmlStringRGBA(config.Color);

            string content = info.FinalDamageValue.FormatNumber();

            if (info.IsDodge)
            {
                dmgStr = GetFontText("MISS", color16);
            }
            else if (info.IsCritical)
            {
                dmgStr = $"<sprite=10 color=#{color16}>{GetFontText(content, color16)}";
            }
            else
            {
                dmgStr = GetFontText(content, color16);
            }

            Get<TMP_Text>("Content").text = dmgStr;
            Get<TMP_Text>("Content").fontSize = config.FontSize;

            string GetFontText(string text, string color)
            {
                char[] arr = text.ToCharArray();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < arr.Length; i++)
                {
                    sb.Append($"<sprite={GetFontIndex(arr[i])} color=#{color}>");
                }
                return sb.ToString();
            }

            int GetFontIndex(char c)
            {
                if (c >= '0' && c <= '9')
                {
                    return c - 48;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    return c - 65 + 11;
                }
                else
                {
                    return 37;
                }
            }
        }
    }
}