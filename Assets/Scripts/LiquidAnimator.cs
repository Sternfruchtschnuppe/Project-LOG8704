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
    public float fillAmount = 0.5f;
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

    
    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        lastPos = transform.position;
        
        // float minY = yBounds.x - transform.position.y;
        // float maxY = yBounds.y - transform.position.y;
        // rend.GetPropertyBlock(block);
        // rend.SetPropertyBlock(block);
    }

    void Update()
    {
        rend.GetPropertyBlock(block);
        
        block.SetFloat(FillAmount, fillAmount);
        block.SetVector(FillAmountRemap, new Vector2(yBounds.x, yBounds.y));

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

        rend.SetPropertyBlock(block);
    }
    
    void OnDrawGizmosSelected()
    {
        var r = GetComponent<MeshRenderer>();
        if (r == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(r.bounds.center, r.bounds.size);
    }

}