using Newtonsoft.Json;

namespace Ankama.Cube.Network.Spin2
{
	public sealed class AnkamaTokenCredentials : AnkamaSpinCredentials
	{
		private readonly string m_token;

		public AnkamaTokenCredentials(string token)
		{
			m_token = token;
		}

		protected override void WriteCredentials(JsonTextWriter jsonWriter)
		{
			jsonWriter.WritePropertyName("token");
			jsonWriter.WriteValue(m_token);
		}
	}
}
