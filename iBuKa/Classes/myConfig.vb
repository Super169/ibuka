Imports Microsoft.Win32

Public Enum DisplayMode
    ActualSize = 0
    AutoFit = 1
    FitWidth = 2
    FitHeight = 3
    WinFit = 4
End Enum


Public Class myConfig

    Const REG_APP = "Super169\iBuka Reader"
    Const REG_FULL_APP = "SOFTWARE\" & REG_APP

    Const DEFAULT_WIN_X = 0
    Const DEFAULT_WIN_Y = 0
    Const DEFAULT_WIN_WIDTH = 800
    Const DEFAULT_WIN_HEIGHT = 560
    Const DEFAULT_VIEW_MODE = DisplayMode.AutoFit
    Const DEFAULT_LOCK_SIZE = False
    Const DEFAULT_LOCK_TOP = False

    Public Shared winX As Integer = DEFAULT_WIN_X
    Public Shared winY As Integer = DEFAULT_WIN_Y
    Public Shared winWidth As Integer = DEFAULT_WIN_WIDTH
    Public Shared winHeight As Integer = DEFAULT_WIN_HEIGHT
    Public Shared viewMode As DisplayMode = DEFAULT_VIEW_MODE
    Public Shared lockSize As Boolean = DEFAULT_LOCK_SIZE = False
    Public Shared lockTop As Boolean = DEFAULT_LOCK_TOP
    Public Shared lastFile As String = ""
    Public Shared lastPage As Integer = -1

    Public Shared Sub SaveKey()
        Dim regRoot, regApp As RegistryKey
        regRoot = Registry.CurrentUser.OpenSubKey("SOFTWARE", True)
        regApp = regRoot.CreateSubKey(REG_APP)
        regApp.SetValue("Version", My.Application.Info.Version.ToString())
        regApp.SetValue("winX", winX)
        regApp.SetValue("winY", winY)
        regApp.SetValue("winWidth", winWidth)
        regApp.SetValue("winHeight", winHeight)
        regApp.SetValue("viewMode", CType(viewMode, Integer))
        regApp.SetValue("lockSize", lockSize)
        regApp.SetValue("lockTop", lockTop)
        regApp.SetValue("lastFile", lastFile)
        regApp.SetValue("lastPage", lastPage)
        regApp.Close()
    End Sub

    Public Shared Sub ReadKey()
        Dim regKey As RegistryKey
        regKey = Registry.CurrentUser.OpenSubKey(REG_FULL_APP)
        If IsNothing(regKey) Then

        Else
            winX = CType(regKey.GetValue("winX", DEFAULT_WIN_X), Integer)
            winY = CType(regKey.GetValue("winY", DEFAULT_WIN_Y), Integer)
            winWidth = CType(regKey.GetValue("winWidth", DEFAULT_WIN_WIDTH), Integer)
            winHeight = CType(regKey.GetValue("winHeight", DEFAULT_WIN_HEIGHT), Integer)
            viewMode = CType(regKey.GetValue("viewMode", DEFAULT_VIEW_MODE), DisplayMode)
            lockSize = CType(regKey.GetValue("lockSize", False), Boolean)
            lockTop = CType(regKey.GetValue("lockTop", False), Boolean)
            lastFile = CType(regKey.GetValue("lastFile", ""), String)
            lastPage = CType(regKey.GetValue("lastPage", -1), Integer)
            regKey.Close()
        End If
    End Sub

End Class
