public class BCIDataListener : Singleton<BCIDataListener>
{
    public static EEGData CurrentData;


    // Start is called before the first frame update
    void Start()
    {
        CurrentData = new EEGData {
            xInput = 0.0f,
            yInput = 0.0f,
            kick = 0.0f
        };

    }

    // These are called by the web Applet
    // The signature has to correspond on the applet side
    // All of the parameters have to be filled in one by one because these calls do not handle custom structures well
    public void XInput(float xInput)
    {
        CurrentData.xInput = xInput;
    }

    public void YInput(float yInput)
    {
        CurrentData.yInput = yInput;
    }

    public void Kick(float kick)
    {
        CurrentData.kick = kick;
    }

    /*
    public void UpdateAlpha(float alpha)
    {
        CurrentData.alpha = alpha;
    }

    public void UpdateAlphaBeta(float alphaBeta)
    {
        CurrentData.alphaBeta = alphaBeta;
    }

    public void UpdateAlphaTheta(float alphaTheta)
    {
        CurrentData.alphaTheta = alphaTheta;
    }

    public void UpdateCoherence(float coherence)
    {
        CurrentData.coherence = coherence;
    }

    public void UpdateFocus(float focus)
    {
        CurrentData.focus = focus;
    }

    public void UpdateThetaBeta(float thetaBeta)
    {
        CurrentData.thetaBeta = thetaBeta;
    }

    public void UpdateBlink(float blink)
    {
        CurrentData.blink = blink;
    }

    public void UpdateO1(float o1)
    {
        CurrentData.o1 = o1;
    }*/
}
