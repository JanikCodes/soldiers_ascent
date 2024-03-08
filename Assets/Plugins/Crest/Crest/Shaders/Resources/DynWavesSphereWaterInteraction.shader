// Crest Ocean System

// Copyright 2022 Wave Harmonic Ltd

// This is a stub for a shader that got moved.

Shader "Hidden/Crest/Moved"
{
	Properties
	{
		[HideInInspector] _Message("This shader has moved. Use <i>Crest/Inputs/Dynamic Waves/Sphere-Water Interaction</i> instead.", Float) = 0
	}

	SubShader
	{
		Pass
		{
			ColorMask 0
		}
	}

	CustomEditor "Crest.ObsoleteShaderGUI"
}
