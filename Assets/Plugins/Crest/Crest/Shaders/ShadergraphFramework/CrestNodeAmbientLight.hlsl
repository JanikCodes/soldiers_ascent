// Crest Ocean System

// Copyright 2020 Wave Harmonic Ltd

#include "OceanGraphConstants.hlsl"

#if CREST_HDRP
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

void ApplyIndirectLightingMultiplier
(
	inout half3 io_ambientLight
)
{
	// Allows control of baked lighting through volume framework.
#if !defined(SHADERGRAPH_PREVIEW)
	// We could create a BuiltinData struct which would have rendering layers on it, but it seems more complicated.
#if VERSION_GREATER_EQUAL(13, 0)
	io_ambientLight *= GetIndirectDiffuseMultiplier(GetMeshRenderingLayerMask());
#else
	io_ambientLight *= GetIndirectDiffuseMultiplier(GetMeshRenderingLightLayer());
#endif
#endif
}
#endif // CREST_HDRP

void CrestNodeAmbientLight_half
(
	out half3 o_ambientLight
)
{
	// Use the constant term (0th order) of SH stuff - this is the average
	o_ambientLight = half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
#if CREST_HDRP
	ApplyIndirectLightingMultiplier(o_ambientLight);
#endif
}
