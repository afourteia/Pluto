var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Add this line
var app = builder.Build();

app.MapGet("/", () => "Main page. YOLO!");
app.MapControllers(); // Add this line

app.Run();
