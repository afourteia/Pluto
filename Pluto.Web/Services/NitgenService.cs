namespace Pluto;
using NITGEN.SDK.NBioBSP;

public class NitgenService
{


    private NBioAPI m_NBioAPI;
    public uint NumDevice { get; set; }
    public short[]? DeviceIDs { get; set; }
    public NitgenService()
    {
        m_NBioAPI = new NBioAPI();
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

    public void GetDevices()
    {
        m_NBioAPI.EnumerateDevice(out uint numDevice, out short[] deviceIDs);
        NumDevice = numDevice;
        DeviceIDs = deviceIDs;
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
        var ret = m_NBioAPI.OpenDevice(deviceID);
        if (ret == NBioAPI.Error.NONE)
            return true;
        else
            return false;
    }

    public bool CloseDevice()
    {
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

    public uint Capture()
    {
        return m_NBioAPI.Capture(
            NBioAPI.Type.FIR_PURPOSE.VERIFY,
            out NBioAPI.Type.HFIR hCapturedFIR,
            15 * 1000,
            null,// AtuidData. Not sure what it does
            null);
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
}
