/*
 **********************************
 * Author: Željko Kalajžić
 * Project Task: Homework 3 - Cars factory
 **********************************
 * Description:
 *  
    To keep (persist) the data between multiple requests,
         *  the data must be extracted to a class (CarRepository)
         *  which will be shared by multiple controllers i.e.
         *  it will be registered not as Transient but Singleton in `Program.cs`.
 *
 **********************************
 */

using FactoryApplication.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<CarRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
