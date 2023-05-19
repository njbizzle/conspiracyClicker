using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomButton : MonoBehaviour
{
    public event Action onHoverEnterDelegateFuntion;
    public event Action onHoverExitDelegateFuntion;
    public event Action onClickDelegateFuntion;

    public void AddOnHoverEnter(Action methodAdded){
        onHoverEnterDelegateFuntion += methodAdded;
    }
    public void AddOnHoverExit(Action methodAdded){
        onHoverExitDelegateFuntion += methodAdded;
    }
    public void AddOnClick(Action methodAdded){
        onClickDelegateFuntion += methodAdded;
    }
    public void ResetButton(){
        onClickDelegateFuntion = null;
    }

    void OnMouseEnter(){
        if (onHoverEnterDelegateFuntion != null){
            onHoverEnterDelegateFuntion.Invoke();
        }
    }
    void OnMouseExit(){
        if (onHoverExitDelegateFuntion != null){
            onHoverExitDelegateFuntion.Invoke();
        }
    }
    void OnMouseDown(){
        if (onClickDelegateFuntion != null){
            onClickDelegateFuntion.Invoke();
        }
    }

}
