using Newtonsoft.Json;

namespace Ankama.Cube.Network.Spin2
{
	public sealed class AnkamaNoHaapiCredentials : AnkamaSpinCredentials
	{
		private readonly string m_login;

		public AnkamaNoHaapiCredentials(string login)
		{
			m_login = login;
		}

		protected override void WriteCredentials(JsonTextWriter jsonWriter)
		{
			jsonWriter.WritePropertyName("token");
			jsonWriter.WriteValue(m_login);
		}
	}
}
