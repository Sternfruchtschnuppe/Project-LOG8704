#ifndef BEZIER_VERTEX_DISPLACEMENT_INCLUDED
#define BEZIER_VERTEX_DISPLACEMENT_INCLUDED

float3 BezierPos(float3 p0, float3 p1, float3 p2, float t)
{
    float omt = 1.0 - t;
    return omt*omt*p0 + 2.0*omt*t*p1 + t*t*p2;
}

float3 BezierTangent(float3 p0, float3 p1, float3 p2, float t)
{
    return normalize(2.0*(1.0 - t)*(p1 - p0) + 2.0*t*(p2 - p1));
}

void BezierVertexDisplacement_float(
    float3 p0, float3 p1, float3 p2,
    float t,
    float3 vertexLocalPos,
    float2 startScale,
    float2 endScale,
    out float3 OutPosition)
{
    float3 curvePos = BezierPos(p0, p1, p2, t);
    float3 tangent = BezierTangent(p0, p1, p2, t);

    float3 refSide = float3(1,0,0);// (abs(tangent.y) < 0.99) ? float3(0,1,0) : float3(1,0,0);
    float3 normal = normalize(cross(refSide, tangent));
    float3 binormal = normalize(cross(tangent, normal));

    float2 scale = lerp(startScale, endScale, t);
    float3 offset = binormal * vertexLocalPos.x * scale.x + normal * vertexLocalPos.y * scale.y;

    OutPosition = curvePos + offset;
}


#endif
