using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using DG.Tweening;
using Pool;
using UnityEngine;
using UnityEngine.UI;

public class DropItemPanel : UIFormLogic
{
    public override UIGroup DefultGroup => UIGroup.RearMost;
    public override string Location => "ui/Combat/DropItemPanel";
    GameObjectPool<Image> _dropItemPool;
    GameObjectPool<Image> _dropExpPool;

    protected override void OnStart()
    {
        CreatePool();
    }

    protected override void OnShow()
    {
        Get<RectTransform>("root").SetActive(true);
    }

    protected override void OnHide()
    {
        Get<RectTransform>("root").SetActive(false);
    }

    protected override void OnDestroy()
    {
        _dropExpPool.Dispose();
        _dropItemPool.Dispose();
    }

    void CreatePool()
    {
        Image clone = Get<Image>("dropItem");
        _dropItemPool = new GameObjectPool<Image>(EPoolType.Scalable, 20, clone, target =>
        {
            target.SetParent(Get<Transform>("recycleNode"));
        }, null);
        _dropItemPool.OnReleaseEvent += target => target.SetParent(Get<Transform>("recycleNode"));
        clone.SetActive(false);

        Image clone1 = Get<Image>("dropExp");
        _dropExpPool = new GameObjectPool<Image>(EPoolType.Scalable, 20, clone1, target =>
        {
            target.SetParent(Get<Transform>("recycleNode"));
        }, null);
        _dropExpPool.OnReleaseEvent += target => target.SetParent(Get<Transform>("recycleNode"));
        clone1.SetActive(false);
    }

    public void SpawnCoin(Vector3 pos)
    {
        DropItemConfig config = GetAsset<DropItemConfig>("config");
        PlayerPanel playerPanel = GameEntry.UI.SearchForm<PlayerPanel>();
        if (playerPanel == null) return;
        if (!Panel.IsShow()) return;

        Panel.StartCoroutine(CoinTween(config.CoinInterval, config.CoinDropNum, pos, config, playerPanel));
    }

    IEnumerator CoinTween(float interval, int num, Vector3 pos, DropItemConfig config, PlayerPanel playerPanel)
    {
        CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(ECombatType.Mainline);

        WaitForSeconds wait = new WaitForSeconds(interval);
        for (int i = 0; i < num; i++)
        {
            MainScene mainScene = GameEntry.Scene.GetScene<MainScene>();
            if (mainScene == null || mainScene.SceneCamera == null) yield break;

            Image image = _dropItemPool.SpawnByType();
            image.SetParent(Get<RectTransform>("root"));

            Vector3 randomPos = Utility.Random.RandomCircleSurface(new Vector2(pos.x, pos.z), config.Area);
            randomPos = new Vector3(randomPos.x, 0, randomPos.y);

            float randomLen = (float)Utility.Random.GetRandom01();
            Vector3 topPos = randomPos + Vector3.up * (randomLen * config.HeightRange.x + config.HeightRange.y);

            Vector3 startPos = CameraUtility.ScenePos2UIPos(mainScene.SceneCamera, image.rectTransform, randomPos);
            topPos = CameraUtility.ScenePos2UIPos(mainScene.SceneCamera, image.rectTransform, topPos);

            image.transform.position = startPos;

            Sequence sequence = DOTween.Sequence();
            sequence.timeScale = combatManager.GameSpeed.Value;
            sequence.Append(image.transform.DOMoveY(topPos.y, config.CoinDropDuration).SetEase(config.CoinDropCurve));
            sequence.Insert(0, image.transform.DORotate(Vector3.back * 360, config.RotateDuration, RotateMode.FastBeyond360).SetLoops(config.RotateNum).SetEase(Ease.Linear));
            // sequence.AppendInterval(0.5f);

            Vector3 dir = playerPanel.GetCoinTransform().position - image.transform.position;

            sequence.Append(image.transform.DOMove(image.transform.position - dir * config.CoinFlyMoveBackValue, config.CoinFlyMoveBackDuration).SetEase(config.CoinFlyCurve));
            sequence.Append(image.transform.DOMove(playerPanel.GetCoinTransform().position, config.CoinFlyDuration).SetEase(config.CoinFlyCurve));

            int v_i = i;
            sequence.Play().OnComplete(() =>
            {
                Transform coinTrans = playerPanel.GetCoinTransform();
                if (coinTrans != null)
                {
                    coinTrans.DOKill();
                    coinTrans.localScale = Vector3.one;
                    coinTrans.DOScale(Vector3.one * 1.3f, 0.1f)
                    .OnComplete(() =>
                    {
                        if (coinTrans != null)
                        {
                            coinTrans.localScale = Vector3.one;
                        }
                    });

                    //TODO 临时音效
                    if (v_i == 0)
                    {
                        GameEntry.Audio.PlayEffect("audio/ui/sound_gold", 0.7f);
                    }
                }

                _dropItemPool.Release(image);
            });

            yield return wait;
        }
    }

    public void SpawnExp(Vector3 pos)
    {
        DropItemConfig config = GetAsset<DropItemConfig>("config");
        PlayerPanel playerPanel = GameEntry.UI.SearchForm<PlayerPanel>();
        if (playerPanel == null) return;
        if (!Panel.IsShow()) return;

        Panel.StartCoroutine(ExpTween(config.ExpInterval, config.ExpDropNum, pos, config, playerPanel));
    }

    IEnumerator ExpTween(float interval, int num, Vector3 pos, DropItemConfig config, PlayerPanel playerPanel)
    {
        CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(ECombatType.Mainline);
        WaitForSeconds wait = new WaitForSeconds(interval);

        for (int i = 0; i < num; i++)
        {

            MainScene mainScene = GameEntry.Scene.GetScene<MainScene>();
            if (mainScene == null || mainScene.SceneCamera == null) yield break;

            Image image = _dropExpPool.SpawnByType();
            image.SetParent(Get<RectTransform>("root"));

            Vector3 randomPos = Utility.Random.RandomCircleSurface(new Vector2(pos.x, pos.z), config.ExpArea);
            randomPos = new Vector3(randomPos.x, 0, randomPos.y);

            float randomLen = (float)Utility.Random.GetRandom01();
            Vector3 topPos = randomPos + Vector3.up * (randomLen * config.ExpHeightRange.x + config.ExpHeightRange.y);

            Vector3 startPos = CameraUtility.ScenePos2UIPos(mainScene.SceneCamera, image.rectTransform, randomPos);
            topPos = CameraUtility.ScenePos2UIPos(mainScene.SceneCamera, image.rectTransform, topPos);

            image.transform.position = startPos;

            Sequence sequence = DOTween.Sequence();
            sequence.timeScale = combatManager.GameSpeed.Value;
            sequence.Append(image.transform.DOMoveY(topPos.y, config.ExpDropDuration).SetEase(config.ExpDropCurve));
            sequence.Insert(0, image.transform.DORotate(Vector3.back * 360, config.ExpRotateDuration, RotateMode.FastBeyond360).SetLoops(config.ExpRotateNum).SetEase(Ease.Linear));
            // sequence.AppendInterval(0.5f);

            Vector3 dir = playerPanel.GetExpTransform().position - image.transform.position;

            sequence.Append(image.transform.DOMove(image.transform.position - dir * config.ExpFlyMoveBackValue, config.ExpFlyMoveBackDuration).SetEase(config.ExpFlyCurve));
            sequence.Append(image.transform.DOMove(playerPanel.GetExpTransform().position, config.ExpFlyDuration).SetEase(config.ExpFlyCurve));

            sequence.Play().OnComplete(() =>
            {
                playerPanel.PlayExpParticle();
                _dropExpPool.Release(image);
            });

            yield return wait;
        }
    }
}
