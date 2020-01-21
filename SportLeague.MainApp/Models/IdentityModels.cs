using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SportLigue.MainApp.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		public virtual ICollection<Movie> Movies { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Movie> Movies { get; set; }
		public DbSet<Director> Directors { get; set; }

		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
			
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Movie>()
				.Property(m => m.Name)
				.HasMaxLength(50)
				.IsRequired();

			modelBuilder.Entity<Movie>()
				.Property(m => m.ReleaseDate)
				.HasColumnType(DataType.Date.ToString());

			modelBuilder.Entity<Movie>()
				.HasIndex(m => m.Id)
				.IsUnique();

			modelBuilder.Entity<Movie>()
				.HasRequired(m => m.Director)
				.WithMany(d => d.Movies)
				.HasForeignKey(m => m.DirectorId);

			modelBuilder.Entity<Movie>()
				.HasRequired(m => m.Uploader)
				.WithMany(u => u.Movies)
				.HasForeignKey(m => m.UploaderId);

			modelBuilder.Entity<Director>()
				.Property(d => d.Name)
				.HasMaxLength(50)
				.IsRequired();

			modelBuilder.Entity<Director>()
				.HasIndex(d => d.Name)
				.IsUnique();

			modelBuilder.Entity<Director>()
				.HasKey(d => d.Id)
				.HasMany(d => d.Movies)
				.WithRequired(m => m.Director)
				.HasForeignKey(m => m.DirectorId);
		}

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}