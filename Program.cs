using Microsoft.EntityFrameworkCore;
using Products_API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//builder.Services.AddDbContext<AddDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("contr")));




//builder.Services.AddControllersWithViews()
//    .AddMvcOptions(options =>
//    {
//        // Optionally, configure additional options for antiforgery here
//        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
//    });












builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AddDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ProfilingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
