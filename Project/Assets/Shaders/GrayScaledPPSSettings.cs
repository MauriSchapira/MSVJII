// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( GrayScaledPPSRenderer ), PostProcessEvent.AfterStack, "GrayScaled", true )]
public sealed class GrayScaledPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "StepLevel" )]
	public FloatParameter _StepLevel = new FloatParameter { value = 0f };
	[Tooltip( "GrayBlackBalance" )]
	public FloatParameter _GrayBlackBalance = new FloatParameter { value = 0f };
}

public sealed class GrayScaledPPSRenderer : PostProcessEffectRenderer<GrayScaledPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "GrayScaled" ) );
		sheet.properties.SetFloat( "_StepLevel", settings._StepLevel );
		sheet.properties.SetFloat( "_GrayBlackBalance", settings._GrayBlackBalance );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
