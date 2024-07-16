using ApiForCrypto.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

namespace ApiForCrypto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                                      policy =>
                                      {
                                          policy.WithOrigins("https://guleb23-myap-183d.twc1.net")
                                                              .AllowAnyHeader()
                                                              .AllowAnyMethod()
                                                              .AllowCredentials();

                                      });
            });
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("default"));
            });
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
            });

            var app = builder.Build();
            
            app.UseCors(MyAllowSpecificOrigins);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images")),
                RequestPath = "/Images"
            });
            app.UseHttpsRedirection();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
                });
            }





            app.MapGet("/api/Users", (ApplicationDBContext db) =>
            {
                return db.Users.ToList();
            });
            app.MapGet("/api/login", (ApplicationDBContext db, string? login, string? password) =>
            {
                UserModel logInUser = db.Users.FirstOrDefault(x => x.Username == login && x.Password == password);
                if (logInUser != null)
                {
                    return Results.Json(logInUser);
                }
                else
                {
                    return Results.NotFound();
                }
                
            });
            app.MapPost("/api/CreateUser", async (ApplicationDBContext db, UserModel user) =>
            {
                UserModel anyUser = db.Users.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
                if (anyUser != null)
                {
                    return Results.Conflict();
                }
                else
                {
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return Results.Ok();
                }

            });

            app.Run();
        }
    }
}
