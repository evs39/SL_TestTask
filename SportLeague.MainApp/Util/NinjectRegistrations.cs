using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using SportLigue.MainApp.Models;
using SportLigue.MainApp.Services;
using SportLigue.MainApp.Services.Interfaces;

namespace SportLigue.MainApp.Util
{
	public class NinjectRegistrations : NinjectModule
	{
		public override void Load()
		{
			Bind<IMovieService>().To<MovieService>();
		}
	}
}