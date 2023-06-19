using Web_API_ING.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MongoDB.Driver;
using Web_API_ING.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.
//Enable CORS

builder.Services.AddCors(c => {
	c.AddPolicy("AllowOrigin", options =>
	options.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());
});

SqlHelper.conStrEXDX = builder.Configuration["ConnectionStrings:oracle_conn"];
SqlHelper.conStrDX26 = builder.Configuration["ConnectionStrings:oracle_connDX26"];

var configuration = builder.Configuration;

builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>
{
	var mongoSettings = configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
	return new MongoClient(mongoSettings.ConnectionString);
});
//JSON Serialzer
builder.Services.AddControllersWithViews().AddNewtonsoftJson(optione =>
   optione.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
	.AddNewtonsoftJson(optione => optione.SerializerSettings.ContractResolver = new DefaultContractResolver());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//Enable CORS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
