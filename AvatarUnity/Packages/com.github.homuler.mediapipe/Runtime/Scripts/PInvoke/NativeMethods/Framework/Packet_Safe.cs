using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool mp_Packet__IsEmpty(IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_SidePacket__clear(IntPtr sidePacket);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_SidePacket__size(IntPtr sidePacket);
  }
}
