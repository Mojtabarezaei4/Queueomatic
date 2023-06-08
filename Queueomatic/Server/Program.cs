using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Repositories;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Endpoints.Hubs.Room;
using Queueomatic.Server.Services.AuthenticationService;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.ParticipantService;
using Queueomatic.Server.Services.RoomService;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSignalR();
builder.Services.AddFastEndpoints();


var jwtSecret = builder.Configuration.GetSection("JWTSigningKeys").GetSection("DefaultKey").Value;
builder.Services.AddJWTBearerAuth(jwtSecret);


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IHashIdService, HashIdService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddTransient(typeof(Random));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SignedInUser", x => x.RequireRole("User").RequireClaim("UserId"));
    options.AddPolicy("ValidParticipant", x => x.RequireRole("Participant").RequireClaim("ParticipantId"));
});

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
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
});

app.MapHub<RoomHub>("/rooms/{id}");
app.MapFallbackToFile("index.html");

app.Run();