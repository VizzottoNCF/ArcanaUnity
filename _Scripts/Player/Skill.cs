using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    private int _id;

    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;

    [SerializeField] private int[] _connectedSkills;

    public void rf_UpdateUI()
    {
        string skill_name = SkillTree.Instance.rf_ReadSkillName(_id);
        string skill_desc = SkillTree.Instance.rf_ReadSkillDesc(_id);
        int skill_level = SkillTree.Instance.rf_ReadSkillLevel(_id);
        int skill_cap = SkillTree.Instance.rf_ReadSkillCap(_id);
        int skill_point = SkillTree.Instance.rf_ReadSkillPoint();
        List<Skill> skill_list = SkillTree.Instance.rf_ReadSkillList();
        List<GameObject> connec_list = SkillTree.Instance.rf_ReadConnectorList(); 

        _titleText.text = $"{skill_name}  {skill_level}/{skill_cap}";
        _descriptionText.text = $"{skill_desc}\n Cost: {skill_point}/1 SP";

        // color.yellow if skill is maxed, color.green if able to buy, else color.white
        GetComponent<Image>().color = skill_level >= skill_cap ? Color.yellow : skill_point > 0 ? Color.green : Color.white;

        foreach (var connectedSkill in _connectedSkills) 
        {
            skill_list[connectedSkill].GetComponent<Button>().interactable = (skill_level > 0);
            connec_list[connectedSkill].SetActive(skill_level > 0);
        }
    }

    public void rf_BuySkill()
    {
        int skill_level = SkillTree.Instance.rf_ReadSkillLevel(_id);
        int skill_cap = SkillTree.Instance.rf_ReadSkillCap(_id);
        int skill_point = SkillTree.Instance.rf_ReadSkillPoint();

        // if you're not elligible to buy the skill, returns
        if (skill_point < 1 || skill_level >= skill_cap) { return; }

        // spends 1 skill point and levels up the skill
        SkillTree.Instance.rf_SubtractSkillPoint();
        SkillTree.Instance.rf_LevelUpSkill(_id);

        // updates skill ui
        SkillTree.Instance.rf_UpdateAllSkillUI(); 
    }

    public void rf_WriteID(int id) { _id = id; }
}
