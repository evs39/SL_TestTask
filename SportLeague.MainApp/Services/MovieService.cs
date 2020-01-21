using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SportLigue.MainApp.ExtensionMethods;
using SportLigue.MainApp.Models;
using SportLigue.MainApp.Models.ViewModels;
using SportLigue.MainApp.Services.Interfaces;

namespace SportLigue.MainApp.Services
{
	public class MovieService : IMovieService
	{
		private ApplicationDbContext _context;

		public MovieService(ApplicationDbContext context)
		{
			_context = context;
		}
		
		public async Task<long> CreateAsync(CreateMovieViewModel model, string	userName)
		{
			var director = await _context.Directors
				.FirstOrDefaultAsync(d => d.Name.ToLower().Trim() == model.DirectorName.ToLower().Trim());
			if (director == null)
			{
				director = new Director()
				{
					Name = model.DirectorName.FormatAsName()
				};
				_context.Directors.Add(director);
				await _context.SaveChangesAsync();
			}

			var movie = await _context.Movies
				.FirstOrDefaultAsync(m => m.Name.Trim().ToLower() == model.Name.ToLower().Trim());
			if (movie != null)
				throw new Exception("The movie already exists in database");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
			if (user == null)
				throw new Exception("User not found");

			byte[] poster = new byte[model.Poster.ContentLength];
			using (var stream = model.Poster.InputStream)
			{
				stream.Read(poster, 0, (int)stream.Length);
			}

			movie = new Movie()
			{
				Name = model.Name,
				Description = model.Description,
				ReleaseDate = model.ReleaseYear,
				Director = director,
				Uploader = user,
				Poster = poster
			};

			_context.Movies.Add(movie);
			await _context.SaveChangesAsync();

			return movie.Id;
		}

		public async Task<long> EditAsync(EditMovieSetViewModel model, string userName)
		{
			var director = await _context.Directors
				.FirstOrDefaultAsync(d => d.Name.ToLower().Trim() == model.DirectorName.ToLower().Trim());
			if (director == null)
			{
				director = new Director()
				{
					Name = model.DirectorName.FormatAsName()
				};
				_context.Directors.Add(director);
				await _context.SaveChangesAsync();
			}

			var movie = await _context.Movies
				.FirstOrDefaultAsync(m => m.Id != model.Id
				&& m.Name.ToLower().Trim() == model.Name.ToLower().Trim());
			if (movie != null)
				throw new Exception("Фильм с таким названием уже существует");

			movie = await _context.Movies
				.FirstOrDefaultAsync(m => m.Id == model.Id);
			if (movie == null)
				throw new Exception("Фильм отсутствует в библиотеке");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
			if (user == null)
				throw new Exception("Пользователь не найден");

			byte[] poster = new byte[model.Poster.ContentLength];
			using (var stream = model.Poster.InputStream)
			{
				stream.Read(poster, 0, (int)stream.Length);
			}

			movie.Name = model.Name;
			movie.Description = model.Description;
			movie.ReleaseDate = model.ReleaseYear;
			movie.Director = director;
			movie.Uploader = user;
			movie.Poster = poster;

			await _context.SaveChangesAsync();

			return movie.Id;
		}

		public async Task<ReadMovieViewModel> ReadAsync(long id)
		{
			var viewModel = await _context.Movies
				.Select(m => new ReadMovieViewModel()
				{
					Id = m.Id,
					Name = m.Name,
					Description = m.Description,
					UploaderName = m.Uploader.UserName,
					DirectorName = m.Director.Name,
					ReleaseYear = m.ReleaseDate,
					Poster = m.Poster
				})
				.FirstOrDefaultAsync(m => m.Id == id);
			if (viewModel == null)
				throw new Exception("Movie not found");

			return viewModel;
		}

		public async Task<EditMovieGetViewModel> ReadEditModelAsync(long id)
		{
			var model = await _context.Movies
				.Select(m => new EditMovieGetViewModel()
				{
					Id = m.Id,
					Name = m.Name,
					DirectorName = m.Director.Name,
					Description = m.Description,
					UploaderName = m.Uploader.UserName,
					ReleaseYear = m.ReleaseDate
				})
				.FirstOrDefaultAsync(m => m.Id == id);
			if (model == null)
				throw new Exception("Фильм не найден");

			return model;
		}

		public async Task<MovieListViewModel> ReadListAsync(int pageNumber, int moviesOnPageNumber, string filter)
		{
			var movieListViewModel = new MovieListViewModel()
			{
				PageNumber = pageNumber,
				Filter = filter
			};
			
			var query = string.IsNullOrEmpty(filter)
				? _context.Movies
				: _context.Movies
					.Where(m => m.Name.ToLower().Trim().Contains(filter.ToLower().ToString()));

			var modelCount = await query.CountAsync();
			movieListViewModel.PageCount = (int)Math.Ceiling((double)modelCount / (double)moviesOnPageNumber);


			if (movieListViewModel.PageNumber <= movieListViewModel.PageCount)
				movieListViewModel.MovieList = await query
					.OrderBy(m => m.Id)
					.Skip((pageNumber - 1) * moviesOnPageNumber)
					.Take(moviesOnPageNumber)
					.Select(m => new MovieListItemViewModel()
					{
						Id = m.Id,
						Name = m.Name,
						UploaderName = m.Uploader.UserName,
						DirectorName = m.Director.Name,
						ReleaseYear = m.ReleaseDate
					})
					.ToListAsync();
			else
				movieListViewModel.MovieList = new List<MovieListItemViewModel>();

			return movieListViewModel;
		}
	}
}