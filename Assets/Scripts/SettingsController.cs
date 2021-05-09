using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
#region Resolution
    public GameObject resolutionDropdownObj;
    private Dropdown resolutionDropdown;
    public struct ResolutionInfo
    {
        int width {get {return this.width;} set {this.width = width;}}
        int height {get {return this.height;} set {this.height = height;}}
        string name {get {return this.name;} set {this.name = name;}}
    }
    public static ResolutionInfo Resolution;

    public void SetResolution()
    {
        Screen.SetResolution(1920, 1080, true);
    }
#endregion

    void Start()
    {
        resolutionDropdown = resolutionDropdownObj.GetComponent<Dropdown>();

        // print(resolutionDropdown.options[0].text);

        SetResolution();
    }
}