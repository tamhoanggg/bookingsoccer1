using BookingSoccers.Authentication;
using BookingSoccers.Repo.Context;
using FirebaseAdmin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("CapstoneGradingBEContextConnection") ??
    throw new InvalidOperationException("Connection string 'CapstoneGradingBEContextConnection' not found.");

var ServerVer = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>
    (JwtBearerDefaults.AuthenticationScheme, (o) => { });
//builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddDbContext<BookingSoccersContext>(options =>
{

    options.UseMySql(ServerVer);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseRouting();

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

});

app.Run();

