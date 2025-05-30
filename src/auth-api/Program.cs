using IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfraestructureServicesDependencies(builder.Configuration)
    .AddServiceDependencies()
    .AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
