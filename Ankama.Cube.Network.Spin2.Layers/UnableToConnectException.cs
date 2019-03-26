using System;
using System.Threading.Tasks;

namespace Ankama.Cube.Network.Spin2.Layers
{
	public class UnableToConnectException : TcpConnectionException
	{
		public readonly TaskStatus taskStatus;

		public readonly Exception taskException;

		public UnableToConnectException(TaskStatus taskStatus, Exception taskException)
			: base($"TcpConnection disposed: unable to connect. Task status: {taskStatus} {taskException}")
		{
			this.taskStatus = taskStatus;
			this.taskException = taskException;
		}
	}
}
