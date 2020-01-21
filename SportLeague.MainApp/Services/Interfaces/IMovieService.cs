using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportLigue.MainApp.Models.ViewModels;

namespace SportLigue.MainApp.Services.Interfaces
{
	public interface IMovieService
	{
		Task<long> CreateAsync(CreateMovieViewModel model, string userName);
		Task<ReadMovieViewModel> ReadAsync(long id);
		Task<EditMovieGetViewModel> ReadEditModelAsync(long id);
		Task<long> EditAsync(EditMovieSetViewModel model, string userName);
		Task<MovieListViewModel> ReadListAsync(int pageNumber = 1, int moviesOnPageNumber = 1, string filter = null);
	}
}
