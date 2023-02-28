using Microsoft.EntityFrameworkCore;
using React.Handler;
using React.Data;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ReactJSDomain",
        builder => { builder.WithOrigins("https://nicholas-wu.com", "http://localhost:3000", "https://nicholas-wu.com/?", "https://saif3n.github.io" ,"https://saif3n.github.io/", "https://saif3n.github.io/stockapp/", "https://saif3n.github.io/stockapp").AllowAnyHeader().AllowAnyMethod(); });
});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IReactRepo, ReactRepo>();
builder.Services
    .AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, AuthHandler>("MyAuthentication", null)
        .AddScheme<AuthenticationSchemeOptions, AuthHandler>("AdminAuthentication", null);


builder.Services.AddDbContext<ReactDBContext>(options => options.UseSqlite(builder.Configuration["WebAPIConnection"]));
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnly", policy => policy.RequireClaim("normalUser"));
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("admin"));
    options.AddPolicy("AuthOnly", policy => policy.RequireAssertion(context => context.User.HasClaim(c => (c.Type == "normalUser" || c.Type == "admin"))));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors("ReactJSDomain");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
