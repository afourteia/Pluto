
using Pluto;
using NITGEN.SDK.NBioBSP;
using SecuGen.FDxSDKPro.Windows;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Add this line
builder.Services.AddRequestTimeouts();


builder.Services.AddSingleton<NBioAPI>();
builder.Services.AddSingleton<NitgenService>();
builder.Services.AddSingleton<SGFingerPrintManager>();
builder.Services.AddSingleton<SecuGenService>();
var app = builder.Build();

app.UseRequestTimeouts();

app.MapGet("/", () => "Main page. YOLO!");
app.MapControllers(); // Add this line

app.Run();
