using System.Reflection;
using System.Runtime.Loader;
using PirozhkiService_DataAccess;
using PirozhkiService_DataAccess.Repository;
using PirozhkiService_DataAccess.Repository.IRepository;
using InstrumentService_Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace InstrumentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            var currentAssembly = Assembly.GetExecutingAssembly();
            Console.WriteLine($"Сборка текущего приложения (Assembly): {currentAssembly.FullName}");

            // Получаем сборку текущего приложения с помощью AssemblyLoadContext.Default.Assemblies
            var contextAssemblies = AssemblyLoadContext.Default.Assemblies;
            foreach (var assembly in contextAssemblies)
            {
                Console.WriteLine($"Сборка в контексте загрузки сборок (AssemblyLoadContext.Default.Assemblies): {assembly.FullName}");
            }
            
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));
            /
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
            }).AddDefaultTokenProviders().AddDefaultUI().AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddTransient<IEmailSender, EmailSender>();
            
            builder.Services.AddHttpContextAccessor();
            
            builder.Services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
               Options.Cookie.IsEssential = true;
            });
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IInquiryDetailRepository, InquiryDetailRepository>();
            builder.Services.AddScoped<IInquiryHeaderRepository, InquiryHeaderRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            builder.Services.AddControllersWithViews();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            //добавление middleware сессий. активируем сессии, позволяя приложению использовать механизм сессий для хранения данных,
            //связанных с определенным пользователем, на протяжении нескольких HTTP-запросов.
            app.UseSession();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}