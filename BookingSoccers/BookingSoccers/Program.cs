using BookingSoccers.Authentication;
using BookingSoccers.DI;
using BookingSoccers.Repo.Context;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
//var connString = "Server=soccerfieldbookingmanagement.cunlmjsiwbxl.ap-northeast-1.rds.amazonaws.com;Port=3306;Database=soccer_fields_management;Uid=adminuser;Pwd=booking123456";
//Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnectionString") ??
    throw new InvalidOperationException("Connection string 'DatabaseConnectionString' not found.");

//var ServerVer = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>
    (JwtBearerDefaults.AuthenticationScheme, (o) => { });
    ;

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./FireBase-Config.json")
}
)); ;
builder.Services.AddCors(opt => opt.AddDefaultPolicy(builder => builder.AllowAnyOrigin()
                                                               .AllowAnyMethod()
                                                               .AllowAnyHeader()));
//builder.Services.AddAuthorization();
builder.Services.ConfigServiceDI();
builder.Services.AddDbContext<BookingSoccersContext>(options => 
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   // app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseRouting();

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapSwagger();

});

app.MapControllers();

app.Run();

