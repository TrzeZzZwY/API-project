using AppCore.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Infrastructure.EF;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Infrastructure.EF.services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ContextConnection") 
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
    );
//Identity
builder.Services.AddIdentity<UserEntity, UserRoleEntity>()
    .AddEntityFrameworkStores<AppDbContext>();

//Services
builder.Services.AddScoped<ITagService, EfTagService>();
builder.Services.AddScoped<ICommentService, EfCommentService>();
builder.Services.AddScoped<IPublishService, EfPublishService>();
builder.Services.AddScoped<IAlbumService, EfAlbumService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
