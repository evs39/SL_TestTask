using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace SportLigue.MainApp.ExtensionMethods
{
	public static class StringExtensions
	{
		public static string FormatAsName(this string name)
		{
			if (name.IsNullOrWhiteSpace())
				return null;

			var nameParts = name.TrimAndToLower().Split('\x20', '\t');
			var formattedName = nameParts[0].ToFirstCharUppercase();

			for (int i = 1; i < nameParts.Length; i++)
			{
				formattedName += "\x20" + nameParts[i].ToFirstCharUppercase();
			}

			return formattedName;
		}

		public static string ToFirstCharUppercase(this string toConvert)
		{
			// First char to Uppercase
			return toConvert.Substring(0, 1).ToUpper() + toConvert.Substring(1);
		}

		public static string TrimAndToLower(this string toConvert)
		{
			// Remove start/end string spaces and convert to lowercase
			return toConvert.Trim().ToLower();
		}
	}
}