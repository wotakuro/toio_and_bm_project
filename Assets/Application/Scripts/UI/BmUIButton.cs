using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BMProject.UI
{
    public class BmUIButton : MonoBehaviour
    {
        [SerializeField] private UnityEvent onClickEvent;
    }
}