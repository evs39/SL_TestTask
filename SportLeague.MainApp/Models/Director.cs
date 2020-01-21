using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportLigue.MainApp.Models
{
	public class Director
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public virtual ICollection<Movie> Movies { get; set; }
	}
}