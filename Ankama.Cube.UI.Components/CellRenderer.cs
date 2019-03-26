using Ankama.Utilities;
using DG.Tweening;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[RequireComponent(typeof(RectTransform))]
	public abstract class CellRenderer<T, U> : CellRenderer where U : ICellRendererConfigurator
	{
		protected T m_value;

		protected U m_configurator;

		public override object value => m_value;

		protected abstract void SetValue(T value);

		protected abstract void Clear();

		public override void SetValue(object v)
		{
			if (v is T)
			{
				m_value = (T)v;
				SetValue(m_value);
			}
			else
			{
				m_value = default(T);
				Clear();
			}
			OnConfiguratorUpdate(instant: true);
		}

		public override Type GetValueType()
		{
			return typeof(T);
		}

		public override void SetConfigurator(ICellRendererConfigurator configurator, bool andUpdate = true)
		{
			SetConfigurator((U)configurator, andUpdate);
		}

		private void SetConfigurator(U configurator, bool andUpdate)
		{
			m_configurator = configurator;
			if (andUpdate)
			{
				OnConfiguratorUpdate(instant: true);
			}
		}

		public override CellRenderer Clone()
		{
			CellRenderer<T, U> cellRenderer = Object.Instantiate<CellRenderer<T, U>>(this, base.rectTransform.get_parent());
			cellRenderer.m_value = m_value;
			return cellRenderer;
		}
	}
	public abstract class CellRenderer : MonoBehaviour
	{
		private RectTransform m_rectTransform;

		private DragNDropClient m_dragNDropClient;

		public RectTransform rectTransform
		{
			get
			{
				if (m_rectTransform != null)
				{
					return m_rectTransform;
				}
				m_rectTransform = this.GetComponent<RectTransform>();
				if (m_rectTransform == null)
				{
					Log.Warning("CellRenderer without RectTransform !!! Name: " + this.get_name(), 76, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\List\\CellRenderer.cs");
				}
				return m_rectTransform;
			}
		}

		public abstract object value
		{
			get;
		}

		public DragNDropClient dragNDropClient
		{
			get
			{
				return m_dragNDropClient;
			}
			set
			{
				m_dragNDropClient = value;
			}
		}

		public abstract Type GetValueType();

		public abstract void SetValue(object value);

		public abstract void SetConfigurator(ICellRendererConfigurator configurator, bool andUpdate = true);

		public abstract void OnConfiguratorUpdate(bool instant);

		public virtual Sequence DestroySequence()
		{
			return null;
		}

		public abstract CellRenderer Clone();

		protected CellRenderer()
			: this()
		{
		}
	}
}
