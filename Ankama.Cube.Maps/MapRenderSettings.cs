using Ankama.Cube.SRP;
using Ankama.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	[Serializable]
	public struct MapRenderSettings
	{
		[Serializable]
		public struct LightSettings
		{
			[SerializeField]
			public Color ambientColor;

			[SerializeField]
			public Color lightColor;

			[SerializeField]
			public float lightIntensity;

			[SerializeField]
			[EulerAngles]
			public Quaternion lightRotation;

			[SerializeField]
			[EulerAngles]
			public Quaternion overrideLightRotation;

			public static LightSettings defaultSettings
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					//IL_001b: Unknown result type (might be due to invalid IL or missing references)
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0033: Unknown result type (might be due to invalid IL or missing references)
					//IL_003a: Unknown result type (might be due to invalid IL or missing references)
					//IL_003f: Unknown result type (might be due to invalid IL or missing references)
					LightSettings result = default(LightSettings);
					result.ambientColor = Color.get_grey();
					result.lightColor = Color.get_white();
					result.lightIntensity = 1f;
					result.lightRotation = Quaternion.get_identity();
					result.overrideLightRotation = Quaternion.get_identity();
					return result;
				}
			}
		}

		[SerializeField]
		public LightSettings lightSettings;

		public static MapRenderSettings Create()
		{
			MapRenderSettings result = default(MapRenderSettings);
			result.lightSettings = LightSettings.defaultSettings;
			return result;
		}

		public static MapRenderSettings CreateFromScene()
		{
			MapRenderSettings ambience = Create();
			FillWithScene(ref ambience);
			return ambience;
		}

		public static void FillWithScene(ref MapRenderSettings ambience, bool duplicateScriptables = false)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			ambience.lightSettings.ambientColor = RenderSettings.get_ambientLight();
			Dictionary<Light, SRPLight> s_lights = SRPLight.s_lights;
			if (s_lights.Count > 1)
			{
				Log.Warning("Multiple light in scene, select first one", 64, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\MapRenderProfile.cs");
			}
			if (s_lights.Count > 0)
			{
				Dictionary<Light, SRPLight>.Enumerator enumerator = s_lights.GetEnumerator();
				enumerator.MoveNext();
				KeyValuePair<Light, SRPLight> current = enumerator.Current;
				Light key = current.Key;
				SRPLight value = current.Value;
				ambience.lightSettings.lightRotation = key.get_transform().get_rotation();
				ambience.lightSettings.lightColor = key.get_color();
				ambience.lightSettings.lightIntensity = key.get_intensity();
				ambience.lightSettings.overrideLightRotation = value.get_overrideDirRotation();
			}
		}

		public static void ApplyToScene(MapRenderSettings ambience)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			RenderSettings.set_ambientLight(ambience.lightSettings.ambientColor);
			foreach (KeyValuePair<Light, SRPLight> s_light in SRPLight.s_lights)
			{
				Light key = s_light.Key;
				SRPLight value = s_light.Value;
				key.set_color(ambience.lightSettings.lightColor);
				key.set_intensity(ambience.lightSettings.lightIntensity);
				key.get_transform().set_rotation(ambience.lightSettings.lightRotation);
				value.set_overrideDirRotation(ambience.lightSettings.overrideLightRotation);
			}
		}

		public unsafe static void TransitionTo(MapRenderSettings ambience, float duration)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			DOTween.To(_003C_003Ec._003C_003E9__6_0 ?? (_003C_003Ec._003C_003E9__6_0 = new DOGetter<Color>((object)_003C_003Ec._003C_003E9, (IntPtr)(void*)/*OpCode not supported: LdFtn*/)), _003C_003Ec._003C_003E9__6_1 ?? (_003C_003Ec._003C_003E9__6_1 = new DOSetter<Color>((object)_003C_003Ec._003C_003E9, (IntPtr)(void*)/*OpCode not supported: LdFtn*/)), ambience.lightSettings.ambientColor, duration);
			foreach (KeyValuePair<Light, SRPLight> s_light in SRPLight.s_lights)
			{
				Light light = s_light.Key;
				SRPLight srpLight = s_light.Value;
				_003C_003Ec__DisplayClass6_0 _003C_003Ec__DisplayClass6_;
				DOTween.To(new DOGetter<Color>((object)_003C_003Ec__DisplayClass6_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Color>((object)_003C_003Ec__DisplayClass6_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), ambience.lightSettings.lightColor, duration);
				DOTween.To(new DOGetter<float>((object)_003C_003Ec__DisplayClass6_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)_003C_003Ec__DisplayClass6_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), ambience.lightSettings.lightIntensity, duration);
				DOTween.To(new DOGetter<Quaternion>((object)_003C_003Ec__DisplayClass6_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Quaternion>((object)_003C_003Ec__DisplayClass6_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), ambience.lightSettings.lightRotation.get_eulerAngles(), duration);
				DOTween.To(new DOGetter<Quaternion>((object)_003C_003Ec__DisplayClass6_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Quaternion>((object)_003C_003Ec__DisplayClass6_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), ambience.lightSettings.overrideLightRotation.get_eulerAngles(), duration);
			}
		}
	}
}
