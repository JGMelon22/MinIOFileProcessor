using FileUploaderPartA.API.Extensions;
using FileUploaderPartA.Application.Imports.Commands.Handlers;
using FileUploaderPartA.Infrastructure.Configurations;
using FileUploaderPartA.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateImportCommandHandler).Assembly));

builder.Services.Configure<AmazonS3Configuration>(builder.Configuration.GetSection("AWS"));
builder.Services.Configure<FileUploadConfiguration>(builder.Configuration.GetSection("UploadConfiguration"));

builder.Services.AddServices();

builder.Services.AddScoped<DapperDbContext>();
builder.Services.AddRepositories();

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
