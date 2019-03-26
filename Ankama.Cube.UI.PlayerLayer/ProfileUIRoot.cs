using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.PlayerLayer
{
	public class ProfileUIRoot : AbstractUI
	{
		[SerializeField]
		private Transform SafeArea;

		[SerializeField]
		private Image m_greyBG;

		public unsafe void Initialise()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			SafeArea.set_localPosition(new Vector3(((IntPtr)(void*)SafeArea.get_localPosition()).x, 1080f, 0f));
		}

		public IEnumerator PlayEnterAnimation()
		{
			SafeArea.set_localPosition(new Vector3(0f, 1080f, 0f));
			Sequence val = DOTween.Sequence();
			TweenSettingsExtensions.Append(val, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(SafeArea, 0f, 0.25f, false), 30));
			TweenExtensions.Play<Sequence>(val);
			yield return TweenExtensions.WaitForKill(val);
		}

		public unsafe IEnumerator Unload()
		{
			DOTweenModuleUI.DOFade(m_greyBG, 0f, 0.5f);
			DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, 0.25f);
			Sequence val = DOTween.Sequence();
			TweenSettingsExtensions.Append(val, ShortcutExtensions.DOLocalMove(SafeArea, new Vector3(((IntPtr)(void*)SafeArea.get_localPosition()).x, 1080f, 0f), 0.25f, false));
			TweenExtensions.Play<Sequence>(val);
			yield return TweenExtensions.WaitForKill(val);
		}

		public void Close()
		{
			PlayerIconRoot.instance.ReducePanel();
		}
	}
}
