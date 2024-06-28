using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using NITGEN.SDK.NBioBSP;


var listener = new TcpListener(IPAddress.Any, 12345);
listener.Start();
Console.WriteLine("TCP Server is running on port 12345...");


var comThread = new Thread(() => EnrollFingerprint());
comThread.SetApartmentState(ApartmentState.STA);
comThread.Start();
comThread.Join();


// while (true)
// {
//     var client = await listener.AcceptTcpClientAsync();
//     Console.WriteLine("Connected to client.");

//     _ = Task.Run(async () =>
//     {
//         var stream = client.GetStream();
//         var buffer = new byte[1024];
//         int bytesRead;

//         while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
//         {
//             var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//             Console.WriteLine($"Received: {request}");

//             if (request == "Capture" || request == "Enroll")
//             {
//                 // Use a separate thread with STAThread for COM operations
//                 var comThread = new Thread(() =>
//                 {
//                     if (request == "Capture")
//                     {
//                         CaptureFingerprint();
//                     }
//                     else if (request == "Enroll")
//                     {
//                         EnrollFingerprint();
//                     }
//                 });
//                 comThread.SetApartmentState(ApartmentState.STA);
//                 comThread.Start();
//                 comThread.Join(); // Wait for the COM operation to complete
//             }

//             await stream.WriteAsync(buffer, 0, bytesRead);
//             Console.WriteLine($"Echoed..");
//         }

//         Console.WriteLine("Client disconnected.");
//         client.Close();
//     });
// }

[STAThread]
static void CaptureFingerprint()
{
    NBioAPI m_NBioAPI = new NBioAPI();
    NBioAPI.Export m_NBioExport = new NBioAPI.Export(m_NBioAPI);
    NBioAPI.Type.HFIR hCapturedFIR = new NBioAPI.Type.HFIR();
    NBioAPI.Type.FIR_PAYLOAD payload = new NBioAPI.Type.FIR_PAYLOAD();
    Console.WriteLine($"Capture..");
    m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
    m_NBioAPI.Capture(out hCapturedFIR); // Assuming Capture is a synchronous method
    m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
}

[STAThread]
static void EnrollFingerprint()
{
    NBioAPI m_NBioAPI = new NBioAPI();
    NBioAPI.Export m_NBioExport = new NBioAPI.Export(m_NBioAPI);
    NBioAPI.Type.HFIR hCapturedFIR = new NBioAPI.Type.HFIR();
    NBioAPI.Type.FIR_PAYLOAD payload = new NBioAPI.Type.FIR_PAYLOAD();
    Console.WriteLine($"Enroll..");
    m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
    m_NBioAPI.Enroll(out hCapturedFIR, payload); // Assuming Enroll is a synchronous method
    m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
}