    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public GameObject resolutionDropdownObj;
    private Dropdown resolutionDropdown;

    void Start()
    {
        resolutionDropdown = resolutionDropdownObj.GetComponent<Dropdown>();
    }
}