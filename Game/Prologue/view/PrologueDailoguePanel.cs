using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Prologue;
using Sirenix.OdinInspector;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

public class PrologueDailoguePanel : UIFormLogic
{
    public override UIGroup DefultGroup => UIGroup.Middle;
    public override string Location => "ui/Prologue/PrologueDailoguePanel";

    List<DialogueInfo> _list;
    Action _clickAction;
    Tween _tween;
    int _tweenTaskID;
    protected override void OnStart()
    {
        _list = new List<DialogueInfo>();
        Get<Button>("btnContinue").Subscribe(BtnContinueOnClick);
        Get<LoopListView2>("Scroll View").InitListView(0, OnListRefresh);
    }

    private LoopListViewItem2 OnListRefresh(LoopListView2 view, int index)
    {
        if (index < 0)
        {
            return null;
        }

        LoopListViewItem2 item;

        if (_list[index].Type == EDialogueType.Player)
        {
            item = view.NewListViewItem("player");
            DialogueItem dialogueItem = item.GetOrAddComponent<DialogueItem>();

            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
                dialogueItem.Init();
            }

            dialogueItem.Refresh(_list[index]);
        }
        else if (_list[index].Type == EDialogueType.Monster)
        {
            item = view.NewListViewItem("monster");
            DialogueItem dialogueItem = item.GetOrAddComponent<DialogueItem>();

            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
                dialogueItem.Init();
            }

            dialogueItem.Refresh(_list[index]);
        }
        else if (_list[index].Type == EDialogueType.Narration)
        {
            item = view.NewListViewItem("narration");
            NarrationItem narrationItem = item.GetOrAddComponent<NarrationItem>();

            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
                narrationItem.Init();
            }

            narrationItem.Refresh(_list[index]);
        }
        else
        {
            return null;
        }

        if (index == _list.Count - 1)
        {
            _tweenTaskID = GameEntry.Task.AddTask(_ =>
            {
                float size = Get<LoopListView2>("Scroll View").ViewPortSize;
                float contentSize = Get<LoopListView2>("Scroll View").ContainerTrans.sizeDelta.y;
                float targetY = contentSize - size < 0 ? 0 : contentSize - size;
                _tween = Get<LoopListView2>("Scroll View").ContainerTrans.DOAnchorPosY(targetY, 0.1f);
            }).Run();
        }

        return item;
    }

    private void BtnContinueOnClick()
    {
        _clickAction?.Invoke();
    }

    public void SetButtonAction(Action clickAction)
    {
        _clickAction = clickAction;
    }

    public void SetButtonState(EButtonState buttonState)
    {
        if (buttonState == EButtonState.GoAheading || buttonState == EButtonState.Combating || buttonState == EButtonState.Tweening)
        {
            Get<Button>("btnContinue").interactable = false;
            Get<Image>("btnContinue").sprite = GetAsset<Sprite>("off");
        }
        else
        {
            Get<Button>("btnContinue").interactable = true;
            Get<Image>("btnContinue").sprite = GetAsset<Sprite>("on");
        }
        Get<Text>("btnTxt").text = GameEntry.Localization.GetString((int)buttonState);
    }

    [Button]
    public void ShowDialogue(EDialogueType type, int name, int content)
    {
        GameEntry.Task.CancelTask(ref _tweenTaskID);
        _tween?.Kill();
        _tween = null;
        _list.Add(new DialogueInfo(type, name, content));
        Get<LoopListView2>("Scroll View").SetCountAndRefresh(_list.Count, false);
    }

    public readonly struct DialogueInfo
    {
        public readonly EDialogueType Type;
        public readonly int Name;
        public readonly int Content;

        public DialogueInfo(EDialogueType type, int name, int content)
        {
            Type = type;
            Name = name;
            Content = content;
        }
    }

    public class DialogueItem : MonoController
    {
        public void Init()
        {
            Get<DOTweenAnimation>("tween").CreateTween(false, false);
            Get<DOTweenAnimation>("tween").tween.SetAutoKill(false);
        }

        public void Refresh(DialogueInfo dialogueInfo)
        {
            Get<Text>("name").text = GameEntry.Localization.GetString(dialogueInfo.Name);
            Get<Text>("content").text = GameEntry.Localization.GetString(dialogueInfo.Content);
            Get<DOTweenAnimation>("tween").DORestart();
        }
    }

    public class NarrationItem : MonoController
    {
        public void Init()
        {
            Get<DOTweenAnimation>("tween").CreateTween(false, false);
            Get<DOTweenAnimation>("tween").tween.SetAutoKill(false);
        }

        public void Refresh(DialogueInfo dialogueInfo)
        {
            Get<Text>("content").text = GameEntry.Localization.GetString(dialogueInfo.Content);
            Get<DOTweenAnimation>("tween").DORestart();
        }
    }
}
