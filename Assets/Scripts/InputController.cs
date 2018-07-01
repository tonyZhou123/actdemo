using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController {

    public static readonly InputController Instance = new InputController();

    public delegate void onDpadDraggingHandler(int quadrant, float angle, float ratio);
    public event onDpadDraggingHandler onDpadDraggingEvent;

    public delegate void onDpadReleasedHandler();
    public event onDpadReleasedHandler onDpadReleasedEvent;

    public delegate void onBtnAttackClickedHandler();
    public event onBtnAttackClickedHandler onBtnAttackClickedEvent;

    public void notifyDpadDragging(int quadrant, float angle, float ratio)
    {
        onDpadDraggingEvent(quadrant, angle, ratio);
    }

    public void notifyDpadReleased()
    {
        onDpadReleasedEvent();
    }

    public void notifyBtnAttackClicked()
    {
        onBtnAttackClickedEvent();
    }
}
