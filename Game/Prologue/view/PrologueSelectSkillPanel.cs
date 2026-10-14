using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrologueSelectSkillPanel : UIFormLogic<PrologueSelectSkillPanel.OpenInfo>
{
    public class OpenInfo
    {
        public List<int> SkillList;
        public Action<int> ClickAction;
    }
    public override UIGroup DefultGroup => UIGroup.Front;
    public override string Location => "ui/Prologue/PrologueSelectSkillPanel";

    OpenInfo _openInfo;
    int _selectSkill;
    protected override void OnStart()
    {
        Get<Button>("btnSure").Subscribe(BtnSureOnClick);
    }

    protected override void OnOpen(OpenInfo data)
    {
        _openInfo = data;
        InitSkillItem();
    }

    void InitSkillItem()
    {
        RectTransform[] list = GetArray<RectTransform>("skillItems");
        for (int i = 0; i < list.Length; i++)
        {
            list[i].GetOrAddComponent<SkillItem>().Refresh(_openInfo.SkillList[i], id => _selectSkill = id, i == 0);
        }
    }

    private void BtnSureOnClick()
    {
        _openInfo.ClickAction?.Invoke(_selectSkill);
        GameEntry.UI.Pop<PrologueSelectSkillPanel>();
    }

    public class SkillItem : MonoController
    {
        Action<int> _onSelect;
        int _skillId;
        public void Refresh(int skillId, Action<int> onSelect, bool select)
        {
            _skillId = skillId;
            _onSelect = onSelect;
            GetComponent<Toggle>().Subscribe(BtnSelectOnClick);
            SetSkillInfo();
            BtnSelectOnClick(select);
        }

        void SetSkillInfo()
        {
            var data = DataTable.GetItem<DTSkill_base>(_skillId);
            UITools.LoadCommonSkillIcon(this, data.Icon, sprite => Get<Image>("icon").sprite = sprite);
            Get<Text>("name").text = GameEntry.Localization.GetString(data.Name);
            Get<Text>("desc").text = GameEntry.Localization.GetString(data.Describe);
        }

        private void BtnSelectOnClick(bool isOn)
        {
            if (!isOn) return;

            _onSelect?.Invoke(_skillId);
        }
    }
}
