using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
namespace Pluto;

[ApiController]
[Route("[controller]")]
public class FingerPrintDeviceController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Welcome to the FingerPrintDevice page!");

    }

    [HttpGet("initialize")]
    public IActionResult Initialize()
    {
        return Ok("Device Initialized");
        // m_NBioAPI = new NBioAPI();

        // Get DeviceID
        // EnumerateDevice function can be used to retrieve the device ID

        // ret = m_NBioAPI.OpenDevice(DeviceID);
        // if (ret == NBioAPI.Error.NONE)
        //     return Ok("Device Opened");
        // else
        //     return Ok("Device Open Failed");
    }

    [HttpGet("closeDevice/{deviceId}")]
    public IActionResult CloseDevice(string deviceId)
    {
        return Ok($"Device Closed: {deviceId}");
        // ret = m_NBioAPI.CloseDevice(DeviceID);
        // if (ret == NBioAPI.Error.NONE)
        //     return Ok("Device Closed");
        // else
        //     return Ok("Device Close Failed");
    }


}