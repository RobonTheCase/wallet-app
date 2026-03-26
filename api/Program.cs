using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using Microsoft.AspNetCore.Mvc;
using WalletApi.Dtos;
using WalletApi.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=wallet.db"));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
        .Where(e => e.Value?.Errors.Count > 0)
        .Select(e => e.Value!.Errors.First().ErrorMessage)
        .ToList();

        var response = new ApiResponse<List<string>>
        {
            Success = false,
            Message = "Validation failed",
            Data = errors
        };

        return new BadRequestObjectResult(response);
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.MapControllers();

app.UseHttpsRedirection();


app.Run();
