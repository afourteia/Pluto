var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Add this line
builder.Services.AddRequestTimeouts();
var app = builder.Build();


app.UseRequestTimeouts();

app.MapGet("/", () => "Main page. YOLO!");
app.MapControllers(); // Add this line

app.Run();
