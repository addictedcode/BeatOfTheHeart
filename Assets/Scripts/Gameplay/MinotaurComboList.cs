using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinotaurComboList", menuName = "Minotaur/MinotaurComboList", order = 1)]
public class MinotaurComboList : ScriptableObject
{
    public List<MinotaurComboSO> comboList;
}
