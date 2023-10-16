using Interview.Application.Mapper;
using Interview.Persistence;
using Interview.Persistence.Contexts.AuthDbContext.DbContext;
using Interview.Persistence.Contexts.AuthDbContext.IdentityAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using Interview.Persistence.ServiceExtensions;
using Microsoft.Extensions.Azure;
using Interview.API.Attribute;
using Interview.Domain.Entities.AuthModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddMvcCore().AddApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {

        Title = "Interview WebAPI",
        Version = "v1",
        Description = "Authentication & Authorization"
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter `Bearer` [space] and then your valid token in the text input below. \r\n\r\n Example: \"Bearer apikey \""
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            }, new string[]{}
        }
    });


});


builder.Services.AddCors();


builder.Services.AddPersistenceServices();






builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole(UserRoles.Admin));

    options.AddPolicy("HROnly", policy =>
    policy.RequireRole(UserRoles.HR));

    options.AddPolicy("AllRoles", policy =>
    {
        policy.RequireRole(UserRoles.Admin);
        policy.RequireRole(UserRoles.HR);
    });
});


builder.Services.AddControllers(x => x.Filters.Add<ApiExceptionFilterAttribute>())
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});



builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddControllers().AddJsonOptions(o =>
{

    o.JsonSerializerOptions.PropertyNamingPolicy = null;
    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
});

builder.Services.Configure<FormOptions>(x =>
{
    x.MultipartBodyLengthLimit = 2147483648;
});


builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);


builder.Services.AddDbContext<CustomDbContext>(option => option.UseSqlServer(ServiceExtension.CustomDbConnectionString));

builder.Services.AddIdentity<CustomUser, IdentityRole>(options => {
    options.User.RequireUniqueEmail = false;
}).AddEntityFrameworkStores<CustomDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidateAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidateIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))

    };
});


builder.WebHost.UseUrls("https://localhost:7077");

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
.Build();


builder.Services.AddRateLimiterServiceExtension();


builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["local-1:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["local-1:queue"], preferMsi: true);
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["AzureConnectionStrings:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["AzureConnectionStrings:queue"], preferMsi: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());

app.MapControllers();

app.MapControllerRoute(name: "default", pattern: "{Interview}/{action=Index}/{id?}");

app.Run();
