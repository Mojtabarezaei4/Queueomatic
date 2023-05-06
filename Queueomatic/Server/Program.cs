using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Repositories;
using Queueomatic.DataAccess.Repositories.Interfaces;

using HashidsNet;

using Queueomatic.DataAccess.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddFastEndpoints();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseFastEndpoints();

app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();
