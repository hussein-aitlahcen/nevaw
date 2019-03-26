using Ankama.Utilities;
using JetBrains.Annotations;
using RestSharp;
using System.Collections.Generic;

namespace Ankama.Cube.Extensions
{
	public static class RestSharpExtensions
	{
		public static string GetFirstHeaderValue(this IList<Parameter> parameters, [NotNull] string name)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Invalid comparison between Unknown and I4
			int i = 0;
			for (int count = parameters.Count; i < count; i++)
			{
				Parameter val = parameters[i];
				Log.Info($"Get first header name : {val.get_Name()} value : {val.get_Value()} type : {val.get_Type()}", 16, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Extensions\\RestSharpExtensions.cs");
				if ((int)val.get_Type() == 3 && string.Equals(val.get_Name(), name))
				{
					return val.get_Value() as string;
				}
			}
			return null;
		}
	}
}
