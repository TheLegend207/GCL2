using UnityEngine;

public class DKAnimationRelay : MonoBehaviour
{
    public DK dk;

    public void ShowBarrel()
    {
        if (dk != null)
            dk.ShowBarrel();
    }

    public void ThrowBarrel()
    {
        if (dk != null)
            dk.ThrowBarrel();
    }
}