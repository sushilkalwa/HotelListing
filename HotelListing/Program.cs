using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration().WriteTo.File(path: "C:\\Users\\sushi\\Desktop\\Logs\\logs-.txt",
	outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:1j}{NewLine}{Exception}",
	rollingInterval: RollingInterval.Day,
	restrictedToMinimumLevel: LogEventLevel.Information).CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(o =>
{
	o.AddPolicy("AllowAll", builder =>
	 builder.AllowAnyOrigin()
	 .AllowAnyMethod()
	 .AllowAnyHeader());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseCors("AllowAll");

app.Run();
