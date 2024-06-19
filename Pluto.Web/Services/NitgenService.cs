namespace Pluto;
using NITGEN.SDK.NBioBSP;
public class NitgenService
{
    private readonly ILogger<NitgenService> _logger;
    private readonly NBioAPI m_NBioAPI;
    private readonly NBioAPI.Export m_NBioExport;
    public uint NumDevice { get; set; }
    public short[]? DeviceIDs { get; set; }

    private NBioAPI.Type.FIR? FIR;
    private NBioAPI.Type.HFIR? hCapturedFIR;
    private NBioAPI.Type.FIR_TEXTENCODE? textFIR;

    public NitgenService(NBioAPI nBioAPI, ILogger<NitgenService> logger)
    {
        m_NBioAPI = nBioAPI;
        m_NBioExport = new NBioAPI.Export(m_NBioAPI);

        _logger = logger;
        _logger.LogInformation("NitgenService is being initialized.");
    }

    private short deviceID = NBioAPI.Type.DEVICE_ID.AUTO;

    public string GetVersion()
    {
        m_NBioAPI.GetVersion(out NBioAPI.Type.VERSION Version);
        string strVersion = string.Format("{0}.{1}", Version.Major, Version.Minor);
        return strVersion;
    }

    public NBioAPI.Type.INIT_INFO_0 GetInfo()
    {
        m_NBioAPI.GetInitInfo(out NBioAPI.Type.INIT_INFO_0 InitInfo);
        return InitInfo;
    }

    // Return Value Type: NBioAPI.Error
    public uint SetInitInfo(NBioAPI.Type.INIT_INFO_0 InitInfo)
    {
        return m_NBioAPI.SetInitInfo(InitInfo);
    }

    public uint GetDevices()
    {
        m_NBioAPI.EnumerateDevice(out uint numDevice, out short[] deviceIDs);
        NumDevice = numDevice;
        DeviceIDs = deviceIDs;
        return numDevice;
    }

    public uint GetDeviceInfo(short deviceID, out NBioAPI.Type.DEVICE_INFO_0 deviceInfo)
    {
        return m_NBioAPI.GetDeviceInfo(deviceID, out deviceInfo);
    }

    public uint SetDeviceInfo()
    {
        // return m_NBioAPI.SetDeviceInfo(short DeviceID, NBioAPI.Type.DEVICE_INFO_0 DeviceInfo);
        return 0;
    }

    public bool OpenDevice()
    {
        // Get DeviceID
        // EnumerateDevice function can be used to retrieve the device ID
        _logger.LogInformation("Opening device...");
        var ret = m_NBioAPI.OpenDevice(deviceID);
        if (ret == NBioAPI.Error.NONE)
            return true;
        else
            return false;
    }

    public bool CloseDevice()
    {
        _logger.LogInformation("Closing device...");
        var ret = m_NBioAPI.CloseDevice(deviceID);
        if (ret == NBioAPI.Error.NONE)
            return true;
        else
            return false;
    }

    // Adjust device brightness
    public uint AdjustDevice() { return 0; }

    public short GetOpenedDeviceID()
    {
        return m_NBioAPI.GetOpenedDeviceID();
    }

    public uint CheckFinger()
    {
        var ret = m_NBioAPI.CheckFinger(out bool bFingerExist);
        if (ret == NBioAPI.Error.NONE)
            return bFingerExist ? 1u : 0u;
        else
            return ret;
    }

    public string Capture()
    {
        // var ret = m_NBioAPI.Capture(
        //     NBioAPI.Type.FIR_PURPOSE.ENROLL,
        //     out NBioAPI.Type.HFIR hCapturedFIR,
        //     15 * 1000,
        //     null,// AuditData   . Not sure what it does
        //     null);
        // m_NBioAPI.OpenDevice(deviceID);
        // var temp = m_NBioAPI.CloseDevice(deviceID);
        // _logger.LogInformation($"Closing device: {temp}");
        // var ret = m_NBioAPI.Capture(
        //     out hCapturedFIR,
        //     15 * 1000,
        //     null);

        // var ret = m_NBioAPI.Capture(
        //     NBioAPI.Type.FIR_PURPOSE.ENROLL,
        //     out hCapturedFIR,
        //     15 * 1000,
        //     null,
        //     null);

        NBioAPI.Type.HFIR SubscriberFIR;
        NBioAPI.Type.HFIR? InputFIR = new NBioAPI.Type.HFIR();
        NBioAPI.Type.HFIR? AuditFIR = new NBioAPI.Type.HFIR();
        NBioAPI.Type.WINDOW_OPTION winop = new NBioAPI.Type.WINDOW_OPTION();
        uint ret = 111;

        try
        {
            // var ret = m_NBioAPI.Capture(
            //     NBioAPI.Type.FIR_PURPOSE.ENROLL,
            //     out hCapturedFIR,
            //     15 * 1000,
            //     null,
            //     null);
            // ret = m_NBioAPI.Enroll(
            //     ref InputFIR,
            //     out SubscriberFIR,
            //     null,
            //     15 * 1000,
            //     AuditFIR,
            //     winop);

            NBioAPI.Type.HFIR hFIR, hAuditFIR;

            hAuditFIR = new NBioAPI.Type.HFIR();

            m_NBioAPI.Enroll(null, out hFIR, null, NBioAPI.Type.TIMEOUT.DEFAULT, hAuditFIR, null);
            _logger.LogInformation($"Enroll Capture ret: ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while capturing fingerprint.");
            return "Error occurred while capturing fingerprint.";
        }

        if (ret == NBioAPI.Error.NONE)
        {
            var temp = m_NBioAPI.GetFIRFromHandle(
                        hCapturedFIR,
                        out FIR,
                        NBioAPI.Type.FIR_FORMAT.STANDARD);

            var temp1 = m_NBioAPI.GetTextFIRFromHandle(
                hCapturedFIR,
                out textFIR,
                false,
                NBioAPI.Type.FIR_FORMAT.STANDARD
                );

            var temp2 = m_NBioExport.NBioBSPToImage(
                hCapturedFIR,
                out NBioAPI.Export.EXPORT_AUDIT_DATA ExportAuditData);

            var temp3 = m_NBioExport.NBioBSPToImage(
                FIR,
                out NBioAPI.Export.EXPORT_AUDIT_DATA ExportAuditData1);

            var temp4 = NBioAPI.ExportRawToISOV1(
                ExportAuditData,
                false,
                NBioAPI.COMPRESS_MOD.NONE,
                out byte[] outBuf);

            return "Success";
        }
        else if (ret == NBioAPI.Error.DEVICE_NOT_OPENED)
            return "Device not opened";
        else if (ret == NBioAPI.Error.CAPTURE_FAKE_SUSPICIOUS)
            return "Timeout or not valid capture";
        else if (ret == NBioAPI.Error.USER_CANCEL)
            return "User Cancelled";
        else
            return "Failed for other reasons";

    }
    public uint Process() { return 0; }
    public uint CreateTemplate() { return 0; }

    public uint VerifyMatch() { return 0; }

    public uint Enroll(ref NBioAPI.Type.HFIR? storedFIR)
    {
        var ret = m_NBioAPI.Enroll(
            ref storedFIR,
            out NBioAPI.Type.HFIR hCapturedFIR,
            null, // Payload
            15 * 1000,
            null, // AuditData
            null); // WindowOption

        if (ret == NBioAPI.Error.NONE)
            // TODO: Need to return an object instead
            return 1;
        // m_NBioAPI.GetFIRFromHandle()
        // NBioAPI.Type.FIR FIR = new NBioAPI.Type.FIR();
        // m_NBioAPI.GetTextFIRFromHandle()
        // NBioAPI.Type.FIR_TEXTENCODE textFIR = new NBioAPI.Type.FIR_TEXTENCODE();
        else
            return 0;
    }

    public uint Verify(ref NBioAPI.Type.FIR FIR)
    {
        return VerifyInternal(FIR);
    }

    public uint Verify(ref NBioAPI.Type.INPUT_FIR FIR)
    {
        return VerifyInternal(FIR);
    }

    public uint Verify(ref NBioAPI.Type.FIR_TEXTENCODE FIR)
    {
        return VerifyInternal(FIR);
    }

    private uint VerifyInternal(dynamic inputFIR)
    {
        var ret = m_NBioAPI.Verify(
            inputFIR,
            out bool bVerify,
            null,
            15 * 1000,
            null,
            null);

        if (ret == NBioAPI.Error.NONE)
            // TODO: Need to return an object instead
            return 1;
        else
            return 0;
    }

    ~NitgenService()
    {
        _logger.LogInformation("NitgenService is being finalized.");
    }
}
