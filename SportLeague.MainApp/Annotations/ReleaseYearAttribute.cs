using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportLigue.MainApp.Annotations
{
	public class ReleaseYearAttribute : ValidationAttribute
	{
		public ReleaseYearAttribute()
		{
			ErrorMessage = "Недопустимое значение года выпуска";
		}

		public override bool IsValid(object value)
		{
			if (Int32.TryParse(value.ToString(), out int year))
				if (year < 1895 || year > 3000)
					return false;

			return true;
		}
	}
}