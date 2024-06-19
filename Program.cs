
using Pluto;
using NITGEN.SDK.NBioBSP;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Add this line
builder.Services.AddRequestTimeouts();


builder.Services.AddSingleton<NBioAPI>();
builder.Services.AddSingleton<NitgenService>();

var app = builder.Build();

app.UseRequestTimeouts();

app.MapGet("/", () => "Main page. YOLO!");
app.MapControllers(); // Add this line

app.Run();
