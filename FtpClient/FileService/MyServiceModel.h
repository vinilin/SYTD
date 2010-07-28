#pragma once
#define ENDL std::endl;

class MyServiceModel  
{
public:
    MyServiceModel();
    virtual ~MyServiceModel();

private:     
    LPCTSTR SVCSTARTNAME; 
    LPCTSTR SVCDISPLNAME; 
    SERVICE_STATUS serviceStatus;
    SERVICE_STATUS_HANDLE serviceStatusHandle;

    HWND hCMain_seivice_data;

    // Event used to hold ServiceMain from completing
    HANDLE terminateEvent;
    // Thread for the actual work
    HANDLE threadHandle;
    // Flags holding current state of service
    BOOL pauseService ;
    BOOL runningService ;

public:

    int  main(int argc, TCHAR* argv[]);

    void InstallService();
    void UninstallService();           
    void WINAPI ServiceMain(DWORD argc, TCHAR* argv[]);
    void WINAPI ServiceControlHandler(DWORD contorlCode );
    LRESULT CALLBACK WndProc(HWND hDlg, UINT Msg, WPARAM wParam ,LPARAM lParam);

    static void WINAPI _ServiceControlHandler(DWORD contorlCode );
    static void WINAPI _ServiceMain( DWORD argc, TCHAR * argv[] );
    void ErrorHandler(char *s, DWORD err);
    VOID terminate(DWORD error);

    BOOL SendStatusToSCM (
        DWORD dwCurrentState, 
        DWORD dwWin32ExitCode, 
        DWORD dwServiceSpecificExitCode,
        DWORD dwCheckPoint,
        DWORD dwWaitHint
        );

    VOID StopService();
    BOOL InitService();
    VOID ResumeService();
    VOID PauseService();
    BOOL RunService(LPCTSTR sSvcName);
};
DWORD WINAPI ServiceThread(LPDWORD param);
extern MyServiceModel objApp;
