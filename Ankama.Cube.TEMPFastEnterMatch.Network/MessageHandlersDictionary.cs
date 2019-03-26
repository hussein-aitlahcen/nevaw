using System;
using System.Collections.Generic;

namespace Ankama.Cube.TEMPFastEnterMatch.Network
{
	internal class MessageHandlersDictionary
	{
		private readonly Dictionary<Type, List<IMessageHandler>> m_dict = new Dictionary<Type, List<IMessageHandler>>();

		private static readonly IReadOnlyList<IMessageHandler> NoHandlers = new List<IMessageHandler>(0);

		public void Add(IMessageHandler handler)
		{
			if (m_dict.TryGetValue(handler.messageType, out List<IMessageHandler> value))
			{
				value.Add(handler);
				m_dict.Add(handler.messageType, value);
			}
			else
			{
				m_dict.Add(handler.messageType, new List<IMessageHandler>
				{
					handler
				});
			}
		}

		public IReadOnlyList<IMessageHandler> HandlersFor(Type messageType)
		{
			if (m_dict.TryGetValue(messageType, out List<IMessageHandler> value))
			{
				return value;
			}
			return NoHandlers;
		}

		public void Clear()
		{
			m_dict.Clear();
		}
	}
}
