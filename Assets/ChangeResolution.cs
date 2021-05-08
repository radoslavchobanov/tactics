using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeResolution : MonoBehaviour
{
    private DropdownMenu resolutionDropdown;

    void Start()
    {
        resolutionDropdown = this.gameObject.GetComponent<DropdownMenu>();

        print(resolutionDropdown.MenuItems());
    }
}
