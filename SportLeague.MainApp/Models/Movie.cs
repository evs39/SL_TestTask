using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportLigue.MainApp.Models
{
	public class Movie
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public long DirectorId { get; set; }
		public string UploaderId { get; set; }
		public int ReleaseDate { get; set; }
		public byte[] Poster { get; set; }

		public virtual ApplicationUser Uploader { get; set; }
		public virtual Director Director { get; set; }
		/*
		 название,
		 описание,
		 год выпуска,
		 режиссёр,
		 пользователь, который выложил информацию,
		 постер
		 */
	}
}