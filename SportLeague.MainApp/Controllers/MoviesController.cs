using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SportLigue.MainApp.Models.ViewModels;
using SportLigue.MainApp.Services.Interfaces;

namespace SportLigue.MainApp.Controllers
{
	[Authorize]
	public class MoviesController : Controller
	{
		private IMovieService _movieService;

		public MoviesController(IMovieService movieService)
		{
			_movieService = movieService;
		}

		/// <summary>
		/// Переход на страницу сохранения фильма
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult AddMovie()
		{
			return View();
		}

		/// <summary>
		/// Сохранение фильма
		/// </summary>
		/// <param name="model">Данные фильма</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> AddMovie(CreateMovieViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = await _movieService.CreateAsync(model, User.Identity.Name);
					return RedirectToActionPermanent("GetMovie", new { id });
				}
				catch (Exception e)
				{
					ViewData["Error"] = e.Message;
					return View("MovieError");
				}
			}
			return View();
		}

		/// <summary>
		/// Переход на страницу редактирования данных фильма
		/// </summary>
		/// <param name="id">Идентификатор фильма</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> EditMovie(long id)
		{
			try
			{
				var model = await _movieService.ReadEditModelAsync(id);
				ViewData["CurrentMovieData"] = model;
				return View();
			}
			catch (Exception e)
			{
				ViewData["Error"] = e.Message;
				return View("MovieError");
			}
		}

		/// <summary>
		/// Изменение данных фильма
		/// </summary>
		/// <param name="model">Данные фильма</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditMovie(EditMovieSetViewModel model)
		{
			if (User.Identity.Name != model.UploaderName)
				return RedirectToAction("GetMovieList");

			if (ModelState.IsValid)
			{
				try
				{
					var id = await _movieService.UpdateAsync(model, User.Identity.Name);
					return RedirectToActionPermanent("GetMovie", new { id });
				}
				catch (Exception e)
				{
					ViewData["Error"] = e.Message;
					return View("MovieError");
				}
			}

			return View();
		}

		/// <summary>
		/// Переход на страницу фильма
		/// </summary>
		/// <param name="id">Идентификатор фильма</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> GetMovie(long id)
		{
			try
			{
				var result = await _movieService.ReadAsync(id);
				return View("MoviePage", result);
			}
			catch(Exception e)
			{
				ViewData["Error"] = e.Message;
				return View("MovieError");
			}
		}

		/// <summary>
		/// Переход на страницу со списком фильмов
		/// </summary>
		/// <param name="id">Номер страницы</param>
		/// <param name="itemsOnPage">Количество строк в таблице фильмов</param>
		/// <param name="filter">Фильтр фильмов по названию</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> GetMovieList(int id = 1, int itemsOnPage = 5, string filter = null)
		{
			try
			{
				var result = await _movieService.ReadListAsync(id, itemsOnPage, filter);
				return View("MovieList", result);
			}
			catch (Exception e)
			{
				ViewData["Error"] = e.Message;
				return View("MovieError");
			}
		}
	}
}