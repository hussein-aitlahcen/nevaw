using Ankama.Cube.Data;

namespace Ankama.Cube.Maps.Objects
{
	public interface IObjectWithElementaryState
	{
		ElementaryStates elementaryState
		{
			get;
		}

		void SetElementaryState(ElementaryStates value);
	}
}
