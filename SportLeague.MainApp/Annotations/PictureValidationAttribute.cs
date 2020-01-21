using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportLigue.MainApp.Annotations
{
	public class PictureValidationAttribute : ValidationAttribute
	{
		private static string[] AllowedTypes;

		static PictureValidationAttribute()
		{
			AllowedTypes = new string[]
			{
				"image/jpeg",
				"image/pjpeg",
				"image/png"
			};
		}

		public PictureValidationAttribute()
		{
			ErrorMessage = "Недопустимый формат файла";
		}

		public override bool IsValid(object value)
		{
			var file = value as HttpPostedFileBase;
			if (file == null)
				return false;

			return AllowedTypes.Contains(file.ContentType);
		}
	}
}