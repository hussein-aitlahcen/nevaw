using System;

namespace Ankama.Cube.TEMPFastEnterMatch.Network
{
	public class MessageHandler<T> : IMessageHandler where T : class
	{
		private readonly Type m_messageType;

		private readonly Action<T> m_action;

		public Type messageType => m_messageType;

		public MessageHandler(Action<T> action)
		{
			m_messageType = typeof(T);
			m_action = action;
		}

		public void Execute(object data)
		{
			m_action(data as T);
		}
	}
}
