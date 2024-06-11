using comtrade.Model;
using comtrade.RewardedCustomer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddHttpClient();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer("Server=DESKTOP-NQ2UHDB;Database=comtradeDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }); //provider za server baze

        builder.Services.AddDbContext<RewardedCustomerContext>(options =>
        {
            options.UseSqlServer("Server=DESKTOP-NQ2UHDB;Database=comtradeDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }); //provider za server baz
        

        var app = builder.Build();

        

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();


Console.WriteLine("Zdravo");
   