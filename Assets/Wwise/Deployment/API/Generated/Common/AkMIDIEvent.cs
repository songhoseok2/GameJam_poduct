#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.11
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

public class AkMIDIEvent : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal AkMIDIEvent(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(AkMIDIEvent obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~AkMIDIEvent() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent(swigCPtr);
        }
        swigCPtr = IntPtr.Zero;
      }
      GC.SuppressFinalize(this);
    }
  }

  public byte byType {
    set {
      AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byType_set(swigCPtr, value);

    } 
    get {
      byte ret = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byType_get(swigCPtr);

      return ret;
    } 
  }

  public byte byChan {
    set {
      AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChan_set(swigCPtr, value);

    } 
    get {
      byte ret = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChan_get(swigCPtr);

      return ret;
    } 
  }

  public AkMIDIEvent() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent(), true) {

  }

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.