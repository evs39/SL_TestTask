using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportLigue.MainApp.Annotations
{
	/// <summary>
	/// Атрибут валидации типа загружаемого на сервер файла
	/// </summary>
	public class PictureAttribute : ValidationAttribute
	{
		private static string[] AllowedTypes;

		/// <summary>
		/// Инициализация разрешенных типов
		/// </summary>
		static PictureAttribute()
		{
			AllowedTypes = new string[]
			{
				"image/jpeg",
				"image/pjpeg",
				"image/png"
			};
		}

		public PictureAttribute()
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