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

		/// <summary>
		/// Сохранение фильма в БД
		/// </summary>
		/// <param name="model">Данные фильма</param>
		/// <param name="userName">Имя пользователя</param>
		/// <returns></returns>
		public async Task<long> CreateAsync(CreateMovieViewModel model, string	userName)
		{
			// Проверка, имеется ли данный режиссер в БД, если нет - сохраняется
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

			// Поиск фильма по названию
			var movie = await _context.Movies
				.FirstOrDefaultAsync(m => m.Name.Trim().ToLower() == model.Name.ToLower().Trim());
			if (movie != null)
				throw new Exception("Фильм уже существует в БД");

			// Проверка наличия пользователя в БД
			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
			if (user == null)
				throw new Exception("Пользователь не найден");

			// Перевод файла постера в набор байт
			byte[] poster = new byte[model.Poster.ContentLength];
			using (var stream = model.Poster.InputStream)
			{
				stream.Read(poster, 0, (int)stream.Length);
			}

			// Добавление фильма в БД
			movie = new Movie()
			{
				Name = model.Name.FormatAsName(),
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

		/// <summary>
		/// Редактирование данных фильма в БД
		/// </summary>
		/// <param name="model"></param>
		/// <param name="userName"></param>
		/// <returns></returns>
		public async Task<long> UpdateAsync(EditMovieSetViewModel model, string userName)
		{
			// Проверка, имеется ли данный режиссер в БД, если нет - сохраняется
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

			// Проверка есть ли в БД фильм с таким названием, но другим идентификатором
			var movie = await _context.Movies
				.FirstOrDefaultAsync(m => m.Id != model.Id
				&& m.Name.ToLower().Trim() == model.Name.ToLower().Trim());
			if (movie != null)
				throw new Exception("Фильм с таким названием уже существует");

			// Поиск фильма с таким идентификатором в БД
			movie = await _context.Movies
				.FirstOrDefaultAsync(m => m.Id == model.Id);
			if (movie == null)
				throw new Exception("Фильм отсутствует в библиотеке");

			// Проверка наличия пользователя в БД
			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
			if (user == null)
				throw new Exception("Пользователь не найден");

			// Перевод файла постера в набор байт
			byte[] poster = new byte[model.Poster.ContentLength];
			using (var stream = model.Poster.InputStream)
			{
				stream.Read(poster, 0, (int)stream.Length);
			}

			// Сохранение изменений в БД
			movie.Name = model.Name.FormatAsName();
			movie.Description = model.Description;
			movie.ReleaseDate = model.ReleaseYear;
			movie.Director = director;
			movie.Uploader = user;
			movie.Poster = poster;

			await _context.SaveChangesAsync();

			return movie.Id;
		}

		/// <summary>
		/// Получение данных фильма из БД
		/// </summary>
		/// <param name="id">Идентификатор фильма</param>
		/// <returns></returns>
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
				throw new Exception("Фильм не найден");

			return viewModel;
		}

		/// <summary>
		/// Получение данных фильма перед редактированием из БД
		/// </summary>
		/// <param name="id">Идентификатор фильма</param>
		/// <returns></returns>
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

		/// <summary>
		/// Считывание списка фильмов для представления из БД
		/// </summary>
		/// <param name="pageNumber">Номер страницы</param>
		/// <param name="moviesOnPageNumber">Число фильмов на странице</param>
		/// <param name="filter">Фильтр фильмов по названию</param>
		/// <returns></returns>
		public async Task<MovieListViewModel> ReadListAsync(int pageNumber, int moviesOnPageNumber, string filter)
		{
			// Создание объекта для хранения данных для страницы
			var movieListViewModel = new MovieListViewModel()
			{
				PageNumber = pageNumber,
				Filter = filter
			};
			
			// Создание запроса в соотвествии с требованиями фильтрации
			var query = string.IsNullOrEmpty(filter)
				? _context.Movies
				: _context.Movies
					.Where(m => m.Name.ToLower().Trim().Contains(filter.ToLower().ToString()));

			// Расчет числа удовлетворяющих запросу записей в БД
			var modelCount = await query.CountAsync();
			movieListViewModel.PageCount = (int)Math.Ceiling((double)modelCount / (double)moviesOnPageNumber);

			// Получение списка удовлетворящих запросу объектов в зависимости
			// от числа фильмов на странице и номера текущей страницы
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