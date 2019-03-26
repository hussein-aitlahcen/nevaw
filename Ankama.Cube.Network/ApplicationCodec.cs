namespace Ankama.Cube.Network
{
	public interface ApplicationCodec<T>
	{
		bool TrySerialize(T t, out byte[] result);

		bool TryDeserialize(byte[] data, out T result);
	}
}
