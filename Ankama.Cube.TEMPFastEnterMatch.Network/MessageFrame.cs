using Ankama.Cube.Network;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.TEMPFastEnterMatch.Network
{
	public abstract class MessageFrame<T> : IDisposable where T : class
	{
		protected readonly IConnection<T> m_connection;

		private readonly MessageHandlersDictionary m_handlersDict = new MessageHandlersDictionary();

		protected MessageFrame(IConnection<T> connection)
		{
			m_connection = connection;
			m_connection.OnApplicationMessage += ExecuteHandlers;
		}

		public void WhenReceiveEnqueue<U>(Action<U> action) where U : class, T
		{
			MessageHandler<U> handler = new MessageHandler<U>(action);
			m_handlersDict.Add(handler);
		}

		public virtual void Dispose()
		{
			m_connection.OnApplicationMessage -= ExecuteHandlers;
			m_handlersDict.Clear();
		}

		private void ExecuteHandlers(T message)
		{
			IReadOnlyList<IMessageHandler> readOnlyList = m_handlersDict.HandlersFor(message.GetType());
			int count = readOnlyList.Count;
			for (int i = 0; i < count; i++)
			{
				readOnlyList[i].Execute(message);
			}
		}
	}
}
