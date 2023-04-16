using LoansAppWebApi.Core;
using LoansAppWebApi.Core.Filters;
using LoansAppWebApi.Core.Services;
using LoansAppWebApi.Models.Configuration;
using LoansAppWebApi.Models.DbModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers((options) =>
{
    // model validation
    options.Filters.Add(new ValidateModelFilter());

    // badrequest and unauthorized formatter
    options.Filters.Add(new HttpResponseFilter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<User, Role, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<Role, ApplicationDbContext, Guid>>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddCors(opt =>
{
    var frontUrl = builder.Configuration.GetValue<string>("front-url");

    opt.AddDefaultPolicy(builderCors =>
    {
        builderCors.WithOrigins(frontUrl!).AllowAnyMethod().AllowAnyHeader().
        WithExposedHeaders(new string[] { "totalAmountOfRecords" });
    });
});

builder.Services.AddTransient<JwtGenerator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(opts =>
               {
                   JWTConfiguration jwtConfiguration = new JWTConfiguration();
                   builder.Configuration.Bind("JWT", jwtConfiguration);

                   opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = jwtConfiguration.Issuer,
                       ValidAudience = jwtConfiguration.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.AccessTokenSecret)),
                       ClockSkew = TimeSpan.Zero,
                   };
               });

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("JWT"));


var app = builder.Build();

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
};


app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();