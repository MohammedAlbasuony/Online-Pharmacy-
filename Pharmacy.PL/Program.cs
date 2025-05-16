using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Pharmacy.BLL.Implementation;
using Pharmacy.BLL.Mapper;
using Pharmacy.BLL.Service.Abstraction;
using Pharmacy.BLL.Service.Implementation;
using Pharmacy.DAL.DB;
using Pharmacy.DAL.Entity;
using Pharmacy.DAL.Repo.Abstraction;
using Pharmacy.DAL.Repo.Implementation;

namespace Pharmacy.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options=>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IMedicineRepo, MedicineRepo>();
            builder.Services.AddScoped<IMedicineService, MedicineService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IPatientsRepo, PatientsRepo>();
            builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
            builder.Services.AddAutoMapper(typeof(MyProfile));

            builder.Services.AddSession(); // Move this line before builder.Build()  
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            var app = builder.Build();

             SeedService.SeedDatabase(app.Services);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            { 
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
