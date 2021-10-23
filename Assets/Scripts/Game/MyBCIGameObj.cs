using UnityEngine;

// This is an adittional component that we add to whatever needs to listen to biofeedback data
// Currently it is a specific impleentation for the player character as it gets the CharacterSkinController and caches parameters specific to this game.
// Generally how you would want to go about it, is create your own BciObj class, attach an interface to it, then give it parameters that it wants to cache and run logic on.
// You can of course also just attach the BCIInteractable interface straight onto your game object class, depending on your implementation and intended scope.
public class MyBCIGameObj : MonoBehaviour, IBCIInteractable
{
    private CharacterSkinController control;


    void Start()
    {
        control = GetComponent<CharacterSkinController>();
    }

    void Update()
    {
        ReactToBCI();
    }

    // send
    private void OnMouseDown()
    {
        SendEventToWeb();
    }

    public void SendEventToWeb()
    {
        DataSender.Instance.SendToJS();
    }

    public void ReactToBCI()
    {
        print("bcigameobj ReactToBCI coherence data: " + BCIDataListener.CurrentData.coherence
            + " and blink data: " + BCIDataListener.CurrentData.blink
            + " and focus data: " + BCIDataListener.CurrentData.focus);
        control.ChangeFacialExpression(BCIDataListener.CurrentData.focus);
        if (BCIDataListener.CurrentData.blink > 0.1f)
            control.Blink();
    }
}
