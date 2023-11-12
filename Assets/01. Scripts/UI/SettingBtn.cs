using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
public class SettingBtn : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject chickin;

    public void SettingBtnClick()
    {
        settingPanel.SetActive(true);
        chickin.SetActive(false);
    }

    public void XBtnClick()
    {
        settingPanel.SetActive(false);
        chickin.SetActive(true);
    }
}
