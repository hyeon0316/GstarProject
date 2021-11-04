using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillToolTipDatabase : MonoBehaviour
{
    [SerializeField]
    private SkillToolTip theSkillToolTip;

    public void ShowToolTip(Vector3 _pos)
    {
        theSkillToolTip.SkillShowToolTip(_pos);
    }

    // 📜SlotToolTip 👉 📜Slot 징검다리
    public void HideToolTip()
    {
        theSkillToolTip.SkillHideToolTip();
    }
}
