using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Timeouts;
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

    [HttpGet("enroll")]
    public IActionResult Enrollment()
    {
        return Ok("Enrollment Started");
        // m_NBioAPI = new NBioAPI();
        // NBioAPI.Type.HFIR hNewFIR;
        // ret = m_NBioAPI.Enroll(out hNewFIR, null);
        // if (ret == NBioAPI.Error.NONE)
        // {
        //     // Enroll success ...
        //     // Get binary encoded FIR data
        //     NBioAPI.Type.FIR biFIR;
        //     m_NBioAPI.GetFIRFromHandle(hNewFIR, out biFIR);
        //     // Get text encoded FIR data
        //     NBioAPI.Type.FIR_TEXTENCODE textFIR;
        //     m_NBioAPI.GetTextFIRFromHandle(hNewFIR, out textFIR, true);
        //     // Write FIR data to file or DB
        // }
        // else
        // Enroll failed ...
    }

    [HttpPost("verify")]
    public IActionResult Verification([FromBody] FIRModel body)
    {
        return Ok($"Verification for ${body.Property1} Started");

        // m_NBioAPI = new NBioAPI();
        // //Read FIRText Data from File or DB.
        // uint ret;
        // bool result;
        // NBioAPI.Type.FIR_PAYLOAD myPayload = new NBioAPI.Type.FIR_PAYLOAD();
        // // Verify with binary FIR
        // ret = m_NBioAPI.Verify(biFIR, out result, myPayload);
        // if (ret != NBioAPI.Error.NONE)
        // {
        //     // Verify Success
        //     // Check payload
        //     if (myPayload.Data != null)
        //     {
        //         textPayload.Text = myPayload.Data;
        //     }
        // }
        // else
        //     // Verify failed
    }

    [HttpPost("convertToISO")]
    public IActionResult ConvertToISO([FromBody] FIRModel body)
    {
        return Ok($"Conversion to ISO for ${body.Property1} Started");




        /* Step 1: Convert TextEncoded to Image */
        //  public System.UInt32 NBioBSPToImage(NITGEN.SDK.NBioBSP.NBioAPI.Type.FIR_TEXTENCODE AuditFIR,
        //  out NITGEN.SDK.NBioBSP.NBioAPI.Export.EXPORT_AUDIT_DATA ExportAuditData);


        // Parameters
        // AuditFIR:
        // A object to a class specifying the FIR to be converted to image data.It can take a FIR handle, binary FIR or text encoded FIR.
        // ExportAuditData:
        // A object to a NBioAPI.Export.EXORT_DATA class that receives the image data converted from the FIR.This structure contains
        // information about the FIR and the raw image data.
        // Return Value
        // NBioAPI.Error.NONE: No error.
        // NBioAPI.Error.ALREADY_PROCESSED: FIR already processed.



        /* Step 2: Convert Image to ISO */
        // NBioAPI.Export.EXPORT_AUDIT_DATA exportAuditData;
        // NBioAPI.Export NBioExport = new NBioAPI.Export(m_NBioAPI);

        // String strResult;
        // NBioExport.NBioBSPToImage(m_FIREnrollAudit, out exportAuditData);
        // byte[] outBuf;
        // NBioAPI.NIMPORTRAWSET rawset;
        // outBuf = null;
        // NBioAPI.ExportRawToISOV1(exportAuditData, false, NBioAPI.COMPRESS_MOD.NONE, out outBuf);
        //     // Convert failed
    }

    [HttpGet("longRunningTask")]
    [RequestTimeout(milliseconds: 5 * 60 * 1000)]
    public async Task<IActionResult> LongRunningTask(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(8 * 60 * 1000, cancellationToken); // Wait for 5 seconds
            return Ok("Long Running Task Completed!");
        }
        catch (OperationCanceledException)
        {
            return StatusCode(408, "Your custom error message here");
        }

    }


}