namespace Ankama.Cube.Utility
{
	public class Singleton<T> where T : class, new()
	{
		private static T s_instance;

		public static T instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new T();
				}
				return s_instance;
			}
		}

		public void Destroy()
		{
			if (this == s_instance)
			{
				s_instance = null;
			}
		}
	}
}
