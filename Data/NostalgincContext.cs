using Microsoft.EntityFrameworkCore;
using Nostalginc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nostalginc.Data
{
    public class NostalgincContext : DbContext
    {
        public DbSet<TopLevelCategories> TopLevelCategories { get; set; }
        public DbSet<WeatherForecast> WeatherForecast { get; set; }

        public string DbPath { get; }

        public NostalgincContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "NostalgincContext.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
