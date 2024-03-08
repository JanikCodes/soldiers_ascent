// Crest Ocean System

// Copyright 2020 Wave Harmonic Ltd

#if _PROCEDURALSKY_ON
half3 SkyProceduralDP(in const half3 i_refl, in const half3 i_lightDir)
{
	half dp = dot(i_refl, i_lightDir);

	if (dp > _SkyDirectionality)
	{
		dp = (dp - _SkyDirectionality) / (1. - _SkyDirectionality);
		return lerp(_SkyBase, _SkyTowardsSun, dp);
	}

	dp = (dp - -1.0) / (_SkyDirectionality - -1.0);
	return lerp(_SkyAwayFromSun, _SkyBase, dp);
}
#endif

#if _PLANARREFLECTIONS_ON
void PlanarReflection(in const half4 i_screenPos, in const half3 i_n_pixel, in const float i_pixelZ, inout half3 io_colour)
{
	half4 screenPos = i_screenPos;
	// This should probably convert normal from world space to view space or something like that.
	screenPos.xy += max(1.0, i_pixelZ * _PlanarReflectionDistanceFactor) * _PlanarReflectionNormalsStrength * i_n_pixel.xz;
	half4 refl = SAMPLE_TEXTURE2D(_ReflectionTex, sampler_ReflectionTex, screenPos.xy / screenPos.w);
	// If more than four layers are used on terrain, they will appear black if HDR is enabled on the planar reflection
	// camera. Reflection alpha is probably a negative value.
	io_colour = lerp(io_colour, refl.rgb, _PlanarReflectionIntensity * saturate(refl.a));
}
#endif // _PLANARREFLECTIONS_ON

float CalculateFresnelReflectionCoefficient(float cosTheta)
{
	// Fresnel calculated using Schlick's approximation
	// See: http://www.cs.virginia.edu/~jdl/bib/appearance/analytic%20models/schlick94b.pdf
	// reflectance at facing angle
	float R_0 = (_RefractiveIndexOfAir - _RefractiveIndexOfWater) / (_RefractiveIndexOfAir + _RefractiveIndexOfWater); R_0 *= R_0;
	const float R_theta = R_0 + (1.0 - R_0) * pow(max(0.,1.0 - cosTheta), _FresnelPower);
	return R_theta;
}

void ApplyReflectionSky
(
	in const half3 i_view,
	in const half3 i_n_pixel,
	in const half3 i_lightDir,
	in const half i_shadow,
	in const half4 i_screenPos,
	in const float i_pixelZ,
	in const half i_weight,
	in const float3 i_positionWS,
	inout half3 io_col
)
{
	half3 skyColour;

	// Reflection
	half3 refl = reflect(-i_view, i_n_pixel);
	// Don't reflect below horizon
	refl.y = max(refl.y, 0.0);

	// Sharp reflection
	const real mip = _ReflectionBlur;

#if _PROCEDURALSKY_ON
	skyColour = SkyProceduralDP(refl, i_lightDir);
//#elif _OVERRIDEREFLECTIONCUBEMAP_ON
//	// User-provided cubemap - TODO figure out how to unpack HDR values from this
//	half4 encodedIrradiance = SAMPLE_TEXTURECUBE_LOD(_ReflectionCubemapOverride, samplerunity_SpecCube0, refl, mip);
//#if !defined(UNITY_USE_NATIVE_HDR)
//	skyColour = DecodeHDREnvironment(encodedIrradiance, unity_SpecCube0_HDR);
//#else
//	skyColour = encodedIrradiance.rbg;
//#endif
#else

#if USE_FORWARD_PLUS
	skyColour = SAMPLE_TEXTURECUBE_LOD(_GlossyEnvironmentCubeMap, sampler_GlossyEnvironmentCubeMap, refl, mip).rgb;
#else
	// Unity sky
	half4 encodedIrradiance = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, refl, mip);
#if !defined(UNITY_USE_NATIVE_HDR)
	skyColour = DecodeHDREnvironment(encodedIrradiance, unity_SpecCube0_HDR);
#else
	skyColour = encodedIrradiance.rbg;
#endif
#endif

#endif

#if _PLANARREFLECTIONS_ON
	PlanarReflection(i_screenPos, i_n_pixel, i_pixelZ, skyColour);
#endif

	// Specular from sun light

	// Surface smoothness
	float smoothness = _Smoothness;
#if _VARYSMOOTHNESSOVERDISTANCE_ON
	smoothness = lerp(smoothness, _SmoothnessFar, pow(saturate(i_pixelZ / _SmoothnessFarDistance), _SmoothnessPower));
#endif

	half alpha = 1.0;
	BRDFData brdfData;
	// TODO pull in code for specular highlight
	InitializeBRDFData(0.0, 0.0, 1.0, smoothness, alpha, brdfData);
	const Light mainLight = GetMainLight();

	half3 lightColor = LightingPhysicallyBased(brdfData, mainLight, i_n_pixel, i_view) * i_shadow * _LightIntensityMultiplier;

#if !USE_FORWARD_PLUS
#ifdef _ADDITIONAL_LIGHTS
	int pixelLightCount = GetAdditionalLightsCount();
	for (int i = 0; i < pixelLightCount; ++i)
	{
		// Last parameter is shadow mask. Since we do not support lightmaps just pass probes. See:
		// Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl
		Light light = GetAdditionalLight(i, i_positionWS, unity_ProbesOcclusion);
		lightColor += LightingPhysicallyBased(brdfData, light, i_n_pixel, i_view);
	}
#endif
#endif

	// Multiply Specular here because it the BRDF doesn't seem to use it..
	skyColour += lightColor;


	// Fresnel
	float R_theta = CalculateFresnelReflectionCoefficient(max(dot(i_n_pixel, i_view), 0.0));
	io_col = lerp(io_col, skyColour, R_theta * _Specular * i_weight);
}

#if _UNDERWATER_ON
void ApplyReflectionUnderwater
(
	in const half3 i_view,
	in const half3 i_n_pixel,
	in const half3 i_lightDir,
	in const half i_shadow,
	in const half4 i_screenPos,
	half3 scatterCol,
	in const half i_weight,
	inout half3 io_col
)
{
	const half3 underwaterColor = scatterCol;
	// The the angle of outgoing light from water's surface
	// (whether refracted form outside or internally reflected)
	const float cosOutgoingAngle = max(dot(i_n_pixel, i_view), 0.);

	// calculate the amount of incident light from the outside world (io_col)
	{
		// have to calculate the incident angle of incoming light to water
		// surface based on how it would be refracted so as to hit the camera
		const float cosIncomingAngle = cos(asin(clamp( (_RefractiveIndexOfWater * sin(acos(cosOutgoingAngle))) / _RefractiveIndexOfAir, -1.0, 1.0) ));
		const float reflectionCoefficient = CalculateFresnelReflectionCoefficient(cosIncomingAngle) * i_weight;
		io_col *= (1.0 - reflectionCoefficient);
		io_col = max(io_col, 0.0);
	}

	// calculate the amount of light reflected from below the water
	{
		// angle of incident is angle of reflection
		const float cosIncomingAngle = cosOutgoingAngle;
		const float reflectionCoefficient = CalculateFresnelReflectionCoefficient(cosIncomingAngle) * i_weight;
		io_col += (underwaterColor * reflectionCoefficient);
	}
}
#endif
