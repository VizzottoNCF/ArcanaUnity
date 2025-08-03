using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public static SkillTree Instance;
    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); } else { Destroy(gameObject); }
    }

    [Header("Skill Tree Attributes")]
    [SerializeField] private int[] _skillLevel;
    [SerializeField] private int[] _skillCap;
    [SerializeField] private string[] _skillName;
    [SerializeField] private string[] _skillDescription;
    [SerializeField] private int _skillPoint;
    [SerializeField] private List<GameObject> _connectorList;
    [SerializeField] private List<Skill> _skillList;
    [SerializeField] private GameObject _connectorHolder;
    [SerializeField] private GameObject _skillHolder;

    private void Start()
    {
        _skillPoint = 20;

        _skillLevel = new int[6];
        _skillCap = new[] { 1, 5, 5, 2, 10, 10 };

        _skillName = new[] { "Upgrade 1", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Upgrade 5", "Upgrade 6", };
        _skillDescription = new[]
        {
            "Does a thing 1",
            "Does a thing 2",
            "Does a thing 3",
            "Does a thing 4",
            "Does a thing 5",
            "Does a thing 6",
        };
        // places all skills into skill list
        foreach (var skill in _skillHolder.GetComponentsInChildren<Skill>()) { _skillList.Add(skill); }

        // places all connectors into list
        foreach (var connector in _connectorHolder.GetComponentsInChildren<RectTransform>()) { if (connector.gameObject != _connectorHolder.gameObject) { _connectorList.Add(connector.gameObject); } }

        // assign IDs
        for (var i =  0; i < _skillList.Count; i++) { _skillList[i].rf_WriteID(i); }

        rf_UpdateAllSkillUI();
    }

    public void rf_UpdateAllSkillUI()
    {
        foreach (var skill in _skillList)
        {
            skill.rf_UpdateUI();
        }
    }

    #region functions calls for read/write values
    public string rf_ReadSkillName(int id) {  return _skillName[id]; }
    public string rf_ReadSkillDesc(int id) {  return _skillDescription[id]; }
    public int rf_ReadSkillLevel(int id) {  return _skillLevel[id]; }
    public int rf_ReadSkillCap(int id) {  return _skillCap[id]; }
    public int rf_ReadSkillPoint() {  return _skillPoint; }
    /// <summary>
    /// Spends 1 Skill Point.
    /// </summary>
    public void rf_SubtractSkillPoint() { _skillPoint -= 1; }
    public void rf_LevelUpSkill(int id) { _skillLevel[id]++; }
    public List<Skill> rf_ReadSkillList() { return _skillList; }
    public List<GameObject> rf_ReadConnectorList() { return _connectorList; }
    #endregion
}
