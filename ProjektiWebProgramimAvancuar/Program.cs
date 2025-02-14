﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjektiWebProgramimAvancuar.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProjektiWebProgramimAvancuarContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjektiWebProgramimAvancuarContext") ?? throw new InvalidOperationException("Connection string 'ProjektiWebProgramimAvancuarContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();
// Enable CORS globally
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
