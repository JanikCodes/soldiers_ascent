// Crest Ocean System

// Copyright 2020 Wave Harmonic Ltd

#include "OceanGraphConstants.hlsl"
#include "../OceanGlobals.hlsl"
#include "../OceanInputsDriven.hlsl"
#include "../OceanVertHelpers.hlsl"

void GeoMorph_half
(
	in const float3 i_positionWS,
	in const float3 i_oceanPosScale0,
	in const float i_meshScaleAlpha,
	in const float i_geometryGridSize,
	in const float i_sliceIndex0,
	in const float4x4 i_matrix,
	in const bool i_previous,
	out float3 o_positionMorphedWS,
	out float o_lodAlpha
)
{
	o_positionMorphedWS = i_positionWS;

	if (i_previous)
	{
		// Vertex snapping and lod transition
		SnapAndTransitionVertLayout(i_matrix, i_meshScaleAlpha, _CrestCascadeDataSource[i_sliceIndex0], i_geometryGridSize, o_positionMorphedWS, o_lodAlpha);
	}
	else
	{
		// Vertex snapping and lod transition
		SnapAndTransitionVertLayout(i_matrix, i_meshScaleAlpha, _CrestCascadeData[i_sliceIndex0], i_geometryGridSize, o_positionMorphedWS, o_lodAlpha);
	}
}
