var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Hello world 2
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Hello pitek
var app = builder.Build();
//Testujemy
//Hello world 3

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//pitek to przyjaciel
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
