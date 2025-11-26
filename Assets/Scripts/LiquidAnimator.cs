using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class LiquidAnimator : MonoBehaviour
{
    private static readonly int FillAmountRemap = Shader.PropertyToID("_FillAmountRemap");
    private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
    private static readonly int Wobble = Shader.PropertyToID("_Wobble");
    private static readonly int WaveHeight = Shader.PropertyToID("_WaveHeight");
    private static readonly int WaveScale = Shader.PropertyToID("_WaveScale");
    private static readonly int WaveSpeed = Shader.PropertyToID("_WaveSpeed");
    private static readonly int TopColor = Shader.PropertyToID("_TopColor");
    private static readonly int SideColor = Shader.PropertyToID("_SideColor");

    public Vector2 yBounds = new Vector2(-1f, 1f);
    
    Renderer rend;
    MaterialPropertyBlock block;
    Vector3 lastPos, velocity, smoothVelocity;
    Vector3 wobble, wobbleTarget;
    
    public float waveBaseHeight = 0.02f;
    public float waveIntensity = 0.2f;
    public float waveScale = 1.0f;

    
    public float wobbleSpeed = 2f;
    public float wobbleDamping = 3f;
    public float wobbleStrength = 0.02f;

    private Vector2 wobbleOffset;
    private Vector2 wobbleVelocity;

    private Bottle _bottle;
    
    void OnEnable()
    {
        _bottle = GetComponentInParent<Bottle>();
        rend = GetComponent<Renderer>();
        rend.material = new Material(rend.sharedMaterial);
        block = new MaterialPropertyBlock();
        lastPos = transform.position;
        
        _bottle.color = _bottle.GetTargetColor();
    }

    void Update()
    {
        if(rend.sharedMaterial == null) return;
        rend.GetPropertyBlock(block);
        if (block == null) return;
        
        block.SetFloat(FillAmount, _bottle.fillAmount);
        var bollteEmptyAddValue = _bottle.fillAmount == 0f ? -100f : 0f;
        block.SetVector(FillAmountRemap, new Vector2(yBounds.x + bollteEmptyAddValue, yBounds.y + bollteEmptyAddValue));

        Vector3 pos = transform.position;
        velocity = (pos - lastPos) / Mathf.Max(Time.deltaTime, 1e-5f);
        lastPos = pos;

        Vector2 move = new Vector2(velocity.x, -velocity.z);
        wobbleVelocity += move * wobbleStrength;
        wobbleVelocity += -wobbleOffset * (wobbleSpeed * wobbleSpeed * Time.deltaTime);
        wobbleVelocity *= Mathf.Exp(-wobbleDamping * Time.deltaTime);
        wobbleOffset += wobbleVelocity * Time.deltaTime;
        block.SetVector(Wobble, wobbleOffset);
        
        float waveHeight = waveBaseHeight + wobbleVelocity.magnitude * waveIntensity;
        
        block.SetFloat(WaveHeight, waveHeight);
        block.SetFloat(WaveScale, waveScale);
        block.SetFloat(WaveSpeed, 1.0f);
        
        block.SetColor(TopColor, _bottle.color + Color.white * 0.2f);
        block.SetColor(SideColor, _bottle.color);
        
        rend.SetPropertyBlock(block);
    }
}