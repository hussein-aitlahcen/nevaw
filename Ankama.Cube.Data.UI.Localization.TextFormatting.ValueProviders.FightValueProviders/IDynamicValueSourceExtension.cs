using Ankama.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders
{
	public static class IDynamicValueSourceExtension
	{
		public static int GetValueInt(this IReadOnlyList<ILevelOnlyDependant> dynamicValues, string name, int level)
		{
			if (dynamicValues != null)
			{
				int count = dynamicValues.Count;
				for (int i = 0; i < count; i++)
				{
					ILevelOnlyDependant levelOnlyDependant = dynamicValues[i];
					if (levelOnlyDependant.get_referenceId() == name)
					{
						return levelOnlyDependant.GetValueWithLevel(level);
					}
				}
			}
			if (Enumerable.Contains(name, '.'))
			{
				return GetValueInOtherData(name, level);
			}
			Log.Error("dynamic value " + name + " not found", 47, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\Localization\\TextFormatting\\ValueProviders\\FightValueProviders\\IFightValueProvider.cs");
			return 0;
		}

		private static int GetValueInOtherData(string name, int level)
		{
			string[] array = name.Split(new char[1]
			{
				'.'
			});
			if (array.Length != 0)
			{
				string a = array[0];
				if (!(a == "God"))
				{
					if (a == "Spell" && array.Length == 3)
					{
						SpellDefinition spellDefinition = null;
						if (int.TryParse(array[1], out int result))
						{
							spellDefinition = ObjectReference.GetSpell(result);
						}
						if (spellDefinition != null)
						{
							string name2 = array[2];
							return (spellDefinition.precomputedData?.dynamicValueReferences).GetValueInt(name2, level);
						}
						Log.Error("No spell for id=" + array[1] + ": dynamic value " + name + " not found", 86, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\Localization\\TextFormatting\\ValueProviders\\FightValueProviders\\IFightValueProvider.cs");
					}
				}
				else if (array.Length == 3)
				{
					GodDefinition god = ObjectReference.GetGod(array[1]);
					if (god != null)
					{
						string name3 = array[2];
						return (god.precomputedData?.dynamicValueReferences).GetValueInt(name3, level);
					}
					Log.Error("No god for '" + array[1] + "': dynamic value " + name + " not found", 68, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\Localization\\TextFormatting\\ValueProviders\\FightValueProviders\\IFightValueProvider.cs");
				}
			}
			Log.Error("dynamic value " + name + " not found", 92, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\Localization\\TextFormatting\\ValueProviders\\FightValueProviders\\IFightValueProvider.cs");
			return 0;
		}
	}
}
