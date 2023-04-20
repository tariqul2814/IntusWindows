using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using IntusWindowsInterview.Common.Configuration;
using IntusWindowsInterview.Model.Data;
using IntusWindowsInterview.Repository;
using IntusWindowsInterview.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

string AllowOrigin = "AllowAllOrigin";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRazorPages();

var dbConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(
                    option => option.UseSqlServer(dbConnection));
//var mySettings = new GlobalConfiguration();
//builder.Services.Configure<ConnectionStringConfig>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddSingleton(builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringConfig>());


builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IOrderServices, OrderServices>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowOrigin,
        builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(AllowOrigin);
app.UseAuthorization();

app.MapControllers();

app.Run();
