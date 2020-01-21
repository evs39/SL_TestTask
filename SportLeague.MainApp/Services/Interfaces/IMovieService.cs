using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportLigue.MainApp.Models.ViewModels;

namespace SportLigue.MainApp.Services.Interfaces
{
	/// <summary>
	/// Интерфейс сервиса фильмов
	/// </summary>
	public interface IMovieService
	{
		/// <summary>
		/// Сохранение фильма
		/// </summary>
		/// <param name="model">Данные фильма</param>
		/// <param name="userName">Имя пользователя</param>
		/// <returns></returns>
		Task<long> CreateAsync(CreateMovieViewModel model, string userName);
		
		/// <summary>
		/// Получение данных фильма
		/// </summary>
		/// <param name="id">Идентификатор фильма</param>
		/// <returns></returns>
		Task<ReadMovieViewModel> ReadAsync(long id);

		/// <summary>
		/// Получение данных фильма перед редактированием
		/// </summary>
		/// <param name="id">Идентификатор фильма</param>
		/// <returns></returns>
		Task<EditMovieGetViewModel> ReadEditModelAsync(long id);

		/// <summary>
		/// Редактирование данных фильма
		/// </summary>
		/// <param name="model"></param>
		/// <param name="userName"></param>
		/// <returns></returns>
		Task<long> UpdateAsync(EditMovieSetViewModel model, string userName);
		
		/// <summary>
		/// Считывание списка фильмов для представления
		/// </summary>
		/// <param name="pageNumber">Номер страницы</param>
		/// <param name="moviesOnPageNumber">Число фильмов на странице</param>
		/// <param name="filter">Фильтр фильмов по названию</param>
		/// <returns></returns>
		Task<MovieListViewModel> ReadListAsync(int pageNumber = 1, int moviesOnPageNumber = 1, string filter = null);
	}
}
