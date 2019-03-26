using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.UI.Fight.Info
{
	public class FightInfoMessageRoot : MonoBehaviour
	{
		private const int MaxRibbonCount = 5;

		[Header("TextFeedback")]
		[SerializeField]
		private FightInfoMessageRibbon m_ribbonReference;

		[SerializeField]
		private Color[] m_colors;

		private List<FightInfoMessageRibbon> m_ribbons;

		private List<FightInfoMessageRibbon> m_activeRibbons;

		private string[] m_colorsString;

		public void Start()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			m_activeRibbons = new List<FightInfoMessageRibbon>();
			Color[] colors = m_colors;
			int num = m_colors.Length;
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = "#" + ColorUtility.ToHtmlStringRGBA(colors[i]);
			}
			m_colorsString = array;
			List<FightInfoMessageRibbon> list = new List<FightInfoMessageRibbon>();
			for (int j = 0; j < 5; j++)
			{
				FightInfoMessageRibbon fightInfoMessageRibbon = Object.Instantiate<FightInfoMessageRibbon>(m_ribbonReference, m_ribbonReference.get_transform().get_parent());
				fightInfoMessageRibbon.get_gameObject().SetActive(false);
				list.Add(fightInfoMessageRibbon);
			}
			m_ribbons = list;
			m_ribbonReference.get_gameObject().SetActive(false);
		}

		public void BuildAndDrawScoreMessage(FightInfoMessage message, string playerOrigin)
		{
			FightInfoMessageRibbon ribbon = GetRibbon();
			if (ribbon != null)
			{
				ribbon.AddParameter(playerOrigin);
				ribbon.AddParameter(GetHTMLStringColor(message.ribbonGroup));
				DrawInfoMessage(ribbon, message, MessageInfoType.Score);
			}
		}

		public void BuildAndDrawInfoMessage(FightInfoMessage message, params string[] parameters)
		{
			FightInfoMessageRibbon ribbon = GetRibbon();
			if (ribbon != null)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					ribbon.AddParameter(parameters[i]);
				}
				ribbon.AddParameter(GetHTMLStringColor(message.ribbonGroup));
				DrawInfoMessage(ribbon, message, MessageInfoType.Default);
			}
		}

		private void DrawInfoMessage(FightInfoMessageRibbon ribbon, FightInfoMessage message, MessageInfoType messageType)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			ribbon.Initialise(messageType, (int)message.iconType, m_colors[(int)message.ribbonGroup], message.countValue);
			ribbon.PlayAnimation(message.id, ReleaseRibbon);
		}

		private FightInfoMessageRibbon GetRibbon()
		{
			List<FightInfoMessageRibbon> ribbons = m_ribbons;
			int count = ribbons.Count;
			if (count > 0)
			{
				FightInfoMessageRibbon fightInfoMessageRibbon = ribbons[count - 1];
				ribbons.RemoveAt(count - 1);
				fightInfoMessageRibbon.ClearParameters();
				fightInfoMessageRibbon.SetExpectedIndex(m_activeRibbons.Count, tween: false);
				m_activeRibbons.Add(fightInfoMessageRibbon);
			}
			return null;
		}

		private void ReleaseRibbon(FightInfoMessageRibbon ribbon)
		{
			ribbon.get_gameObject().SetActive(false);
			List<FightInfoMessageRibbon> activeRibbons = m_activeRibbons;
			activeRibbons.Remove(ribbon);
			int count = activeRibbons.Count;
			for (int i = 0; i < count; i++)
			{
				activeRibbons[i].SetExpectedIndex(i, tween: true);
			}
			m_ribbons.Add(ribbon);
		}

		public Color GetColor(MessageInfoRibbonGroup group)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return m_colors[(int)group];
		}

		private string GetHTMLStringColor(MessageInfoRibbonGroup group)
		{
			return m_colorsString[(int)group];
		}

		public FightInfoMessageRoot()
			: this()
		{
		}
	}
}
