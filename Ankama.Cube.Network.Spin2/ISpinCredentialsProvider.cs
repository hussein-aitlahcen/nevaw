using System.Threading.Tasks;

namespace Ankama.Cube.Network.Spin2
{
	public interface ISpinCredentialsProvider
	{
		Task<ISpinCredentials> GetCredentials();
	}
}
