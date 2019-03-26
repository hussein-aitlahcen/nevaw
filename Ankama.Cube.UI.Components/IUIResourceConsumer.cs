namespace Ankama.Cube.UI.Components
{
	public interface IUIResourceConsumer
	{
		UIResourceDisplayMode Register(IUIResourceProvider provider);

		void UnRegister(IUIResourceProvider provider);
	}
}
