using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MarketWatch.LiveData;

[ComImport, TypeLibType(0x1040), Guid("A43788C1-D91B-11D3-8F39-00C04F3651B8")]
interface IRTDUpdateEvent
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10), PreserveSig]
    void UpdateNotify();

    [DispId(11)]
    int HeartbeatInterval
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
        get;
        [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
        set;
    }

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)]
    void Disconnect();
}
