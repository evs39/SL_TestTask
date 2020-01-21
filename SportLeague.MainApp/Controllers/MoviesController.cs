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

		[HttpGet]
		public ActionResult AddMovie()
		{
			return View();
		}

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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditMovie(EditMovieSetViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = await _movieService.EditAsync(model, User.Identity.Name);
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
				return View("MovieError"); // TODO error page
			}
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> GetMovieList(int id = 1, string filter = null)
		{
			try
			{
				var result = await _movieService.ReadListAsync(id, 5, filter);
				return View("MovieList", result);
			}
			catch (Exception e)
			{
				ViewData["Error"] = e.Message;
				return View("MovieError"); // TODO error page
			}
		}
	}
}