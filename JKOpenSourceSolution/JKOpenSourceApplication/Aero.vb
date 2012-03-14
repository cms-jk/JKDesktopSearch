'*****背景をAero化するためのクラス
Imports System
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Text

Namespace MsdnMag

    ' Desktop Windows Manager APIsの利用
    Friend Class DwmAPI
        <DllImport("dwmapi.dll", PreserveSig:=False)> _
        Public Shared Sub DwmEnableBlurBehindWindow(ByVal hWnd As IntPtr, ByVal pBlurBehind As DWM_BLURBEHIND)
        End Sub

        <DllImport("dwmapi.dll", PreserveSig:=False)> _
        Public Shared Sub DwmExtendFrameIntoClientArea(ByVal hWnd As IntPtr, ByVal pMargins As MARGINS)
        End Sub

        <DllImport("dwmapi.dll", PreserveSig:=False)> _
        Public Shared Function DwmIsCompositionEnabled() As Boolean
        End Function

        <DllImport("dwmapi.dll", PreserveSig:=False)> _
        Public Shared Sub DwmGetColorizationColor(ByRef pcrColorization As Integer, <MarshalAs(UnmanagedType.Bool)> ByRef pfOpaqueBlend As Boolean)
        End Sub

        <DllImport("dwmapi.dll", PreserveSig:=False)> _
        Public Shared Sub DwmEnableComposition(ByVal bEnable As Boolean)
        End Sub

        <DllImport("dwmapi.dll", PreserveSig:=False)> _
        Public Shared Function DwmRegisterThumbnail(ByVal dest As IntPtr, ByVal source As IntPtr) As IntPtr
        End Function

        <DllImport("dwmapi.dll", PreserveSig:=False)> _
        Public Shared Sub DwmUnregisterThumbnail(ByVal hThumbnail As IntPtr)
        End Sub

        <DllImport("dwmapi.dll", PreserveSig:=False)> _
        Public Shared Sub DwmUpdateThumbnailProperties(ByVal hThumbnail As IntPtr, ByVal props As DWM_THUMBNAIL_PROPERTIES)
        End Sub

        <DllImport("dwmapi.dll", PreserveSig:=False)> _
        Public Shared Sub DwmQueryThumbnailSourceSize(ByVal hThumbnail As IntPtr, ByRef size As Size)
        End Sub

        <StructLayout(LayoutKind.Sequential)> _
        Public Class DWM_THUMBNAIL_PROPERTIES
            Public dwFlags As UInteger
            Public rcDestination As RECT
            Public rcSource As RECT
            Public opacity As Byte
            <MarshalAs(UnmanagedType.Bool)> _
            Public fVisible As Boolean
            <MarshalAs(UnmanagedType.Bool)> _
            Public fSourceClientAreaOnly As Boolean

            Public Const DWM_TNP_RECTDESTINATION As UInteger = &H1
            Public Const DWM_TNP_RECTSOURCE As UInteger = &H2
            Public Const DWM_TNP_OPACITY As UInteger = &H4
            Public Const DWM_TNP_VISIBLE As UInteger = &H8
            Public Const DWM_TNP_SOURCECLIENTAREAONLY As UInteger = &H10
        End Class

        <StructLayout(LayoutKind.Sequential)> _
        Public Class MARGINS
            Public cxLeftWidth As Integer, cxRightWidth As Integer, cyTopHeight As Integer, cyBottomHeight As Integer

            Public Sub New(ByVal left As Integer, ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer)
                cxLeftWidth = left
                cyTopHeight = top
                cxRightWidth = right
                cyBottomHeight = bottom
            End Sub
        End Class

        <StructLayout(LayoutKind.Sequential)> _
        Public Class DWM_BLURBEHIND
            Public dwFlags As UInteger
            <MarshalAs(UnmanagedType.Bool)> _
            Public fEnable As Boolean
            Public hRegionBlur As IntPtr
            <MarshalAs(UnmanagedType.Bool)> _
            Public fTransitionOnMaximized As Boolean

            Public Const DWM_BB_ENABLE As UInteger = &H1
            Public Const DWM_BB_BLURREGION As UInteger = &H2
            Public Const DWM_BB_TRANSITIONONMAXIMIZED As UInteger = &H4
        End Class

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure RECT
            Public left As Integer, top As Integer, right As Integer, bottom As Integer

            Public Sub New(ByVal left As Integer, ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer)
                Me.left = left
                Me.top = top
                Me.right = right
                Me.bottom = bottom
            End Sub
        End Structure
    End Class
End Namespace
