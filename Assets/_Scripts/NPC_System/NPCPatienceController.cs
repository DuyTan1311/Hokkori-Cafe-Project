using UnityEngine;
using System.Collections;
using System;

public class NPCPatienceController : MonoBehaviour
{
    [SerializeField] NPCProfile profile;

    private Coroutine waitCoroutine;

    public event Action OnPatienceExpired;

    IEnumerator WaitForDrink()
    {
        float timer = 0;

        while(timer < profile.waitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        // if coroutine has completed without stopping, invoke event
        OnPatienceExpired?.Invoke();
        // reset coroutine reference afterwards
        waitCoroutine = null;
    }

    public void StartWaiting()
    {
        if(waitCoroutine == null)
        {
            waitCoroutine = StartCoroutine(WaitForDrink());
        }
    }

    public void StopWaiting()
    {
        if( waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
    }
}
