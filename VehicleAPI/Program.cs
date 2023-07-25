using DTO;
using Microsoft.EntityFrameworkCore;
using VehicleContext;
using VehicleInterface;
using VehicleInterface.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Mapper Config 

builder.Services.AddAutoMapper(typeof(Mapper));
#endregion

#region  Interface Config
builder.Services.AddScoped<IVehicleInterface, VehicleRepository>();


#endregion

#region DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("VehicleDbContext"),b => b.MigrationsAssembly("VehicleAPI")));
#endregion

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
