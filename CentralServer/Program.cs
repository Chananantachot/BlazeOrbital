using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlazeOrbital.CentralServer.Data;
using BlazeOrbital.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.AddServiceDefaults();
builder.Services.AddDataProtection()
    .SetApplicationName("CentralServer");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString))
    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddIdentityServer()
    .AddApiAuthorization<IdentityUser, ApplicationDbContext>();

builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServer:Authority"];
        options.RequireHttpsMetadata = false;
        options.Audience = builder.Configuration["IdentityServer:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["IdentityServer:IssuerUri"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["IdentityServer:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false
        };
    })
    .AddIdentityServerJwt();

builder.Services.AddGrpc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddQuickGridEntityFrameworkAdapter();
var app = builder.Build();
SeedData.EnsureSeeded(app.Services);

// Allow requests from the external ManufacturingHub and  MissionControl applications
app.UseCors(cors =>
{
    var manufacturingHubOrigin = builder.Configuration["Apps:ManufacturingHub:Origin"];
    var missionControlOrigin = builder.Configuration["Apps:MissionControl:Origin"];

    if (!string.IsNullOrEmpty(manufacturingHubOrigin) && !string.IsNullOrEmpty(missionControlOrigin))
    {
        cors.WithOrigins(manufacturingHubOrigin, missionControlOrigin)
            .AllowAnyMethod()
            .AllowAnyHeader();
    }
});

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
app.UseGrpcWeb();
app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<ManufacturingDataService>().EnableGrpcWeb();
app.MapRazorPages();
app.MapControllers();

app.Run();
