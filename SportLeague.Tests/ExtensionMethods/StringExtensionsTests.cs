using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportLigue.MainApp.ExtensionMethods;

namespace SportLeague.Tests.ExtensionMethods
{
	[TestClass]
	public class StringExtensionsTests
	{
		[TestMethod]
		public void TrimAndLower_AbCUntrimmed_abc()
		{
			var toFormat = "\t AbC \t";

			var formatted = toFormat.TrimAndToLower();

			Assert.AreEqual("abc", formatted);
		}

		[TestMethod]
		public void ToFirstCharUppercase_abc_Abc()
		{
			var toFormat = "abc";

			var formatted = toFormat.ToFirstCharUppercase();

			Assert.AreEqual("Abc", formatted);
		}

		[TestMethod]
		public void FormatName_WhitespaceString_Null()
		{
			var toFormat = "\t  ";

			var formatted = toFormat.FormatAsName();

			Assert.AreEqual(null, formatted);
		}

		[TestMethod]
		public void FormatName_Null_Null()
		{
			string toFormat = null;

			var formatted = toFormat.FormatAsName();

			Assert.AreEqual(null, formatted);
		}

		[TestMethod]
		public void FormatName_UnformatedName_FormatedName()
		{
			var toFormat = "\t Ivan iVanOv \t";

			var formatted = toFormat.FormatAsName();

			Assert.AreEqual("Ivan Ivanov", formatted);
		}
	}
}
