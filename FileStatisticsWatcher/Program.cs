namespace FileStatisticsWatcher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.AddFilteringServices();
            builder.AddBaseServices();
            builder.AddRepositores();
            builder.AddOptions();
            builder.AddDatabase();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/FileSettings/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=FileSettings}/{action=Index}");

            app.Run();
        }
    }
}