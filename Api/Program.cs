using Infrastructure.Extensions;
using Application.Extensions;
using Rabbitmq.Extensions;
using Rabbitmq.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(RabbitMqSettings.ConfigName));


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();

builder.Services.AddRabbitMqPublisher(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();