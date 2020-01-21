using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using SportLigue.MainApp.Annotations;

namespace SportLigue.MainApp.Models.ViewModels
{
	public class CreateMovieViewModel
	{
		[Required]
		[Display(Name = "Название")]
		public string Name { get; set; }
		[Display(Name = "Описание")]
		public string Description { get; set; }
		[Required]
		[Display(Name = "Режиссер")]
		public string DirectorName { get; set; }
		[Required]
		[Picture]
		public HttpPostedFileBase Poster { get; set; }
		public int ReleaseYear { get; set; }
	}

	public class ReadMovieViewModel
	{
		public long Id { get; set; }
		[Display(Name = "Название")]
		public string Name { get; set; }
		[Display(Name = "Описание")]
		public string Description { get; set; }
		[Display(Name = "Режиссер")]
		public string DirectorName { get; set; }
		[Display(Name = "Загрузил")]
		public string UploaderName { get; set; }
		public byte[] Poster { get; set; }

		[ReleaseYear]
		[Display(Name = "Год производства")]
		public int ReleaseYear { get; set; }
	}

	public class EditMovieGetViewModel
	{
		public long Id { get; set; }
		[Display(Name = "Название")]
		public string Name { get; set; }
		[Display(Name = "Описание")]
		public string Description { get; set; }
		[Display(Name = "Режиссер")]
		public string DirectorName { get; set; }
		public string UploaderName { get; set; }

		[ReleaseYear]
		public int ReleaseYear { get; set; }
	}

	public class EditMovieSetViewModel : EditMovieGetViewModel
	{
		public HttpPostedFileBase Poster { get; set; }
	}

	public class MovieListItemViewModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string DirectorName { get; set; }
		public string UploaderName { get; set; }

		[ReleaseYear]
		public int ReleaseYear { get; set; }
	}

	public class MovieListViewModel
	{
		public List<MovieListItemViewModel> MovieList { get; set; }
		public int PageNumber { get; set; }
		public int PageCount { get; set; }
		public string Filter { get; set; }
	}

	public class EditMovieButtomViewModel
	{
		public long Id { get; set; }
		public string UploaderName { get; set; }
	}
}