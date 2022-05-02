using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSimbol : MonoBehaviour
{
    public static LoadingSimbol instance;
    private Image _fillImage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _fillImage = GetComponent<Image>();
        }
        else if (instance != this) Destroy(this);
    }
    public void FillImage(float value)
    {
        _fillImage.fillAmount = value;
        if (_fillImage.fillAmount > 0 && _fillImage.enabled == false) _fillImage.enabled = true;
    }
}
