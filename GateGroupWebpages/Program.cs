

using gategourmetLibrary.Repo;
using gategourmetLibrary.Service;

namespace GateGroupWebpages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            

            // Add services to the container.
            builder.Services.AddRazorPages();
            //til den gemme når man er logget ind 
            builder.Services.AddSession();
            builder.Services.AddSingleton<IOrderRepo, OrderRepo>();
            builder.Services.AddSingleton<OrderService>(); 
            
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //til logud knappen 
            app.UseSession();

            app.MapRazorPages();

            app.Run();
        }
    }
}
