using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform ThrusterFillAmount;
    private PlayerController controller;
    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }
    private void Update()
    {
        SetFuelAmount(controller.GetDashFuleAmount());
    }
    void SetFuelAmount(float Amt)
    {
        ThrusterFillAmount.localScale = new Vector3(1f, Amt, 1f);//setting ui = amount
    }
}
