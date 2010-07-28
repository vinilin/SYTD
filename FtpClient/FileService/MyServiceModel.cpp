#include "stdafx.h"
#include "MyServiceModel.h"
#include "FtpTransService.h"
#include "Trans.nsmap"
#include "FtpController.h"
#include "FileSet.h"

using namespace std;
MyServiceModel objApp;
FtpController *ftpController = NULL;

MyServiceModel::MyServiceModel()
{ 
    // 服务名
    SVCSTARTNAME = "MyServiceName";   
    // 显示名
    SVCDISPLNAME = "MyServiceName";
    serviceStatusHandle = NULL; 
}
MyServiceModel::~MyServiceModel()
{
}
int MyServiceModel::main(int argc, TCHAR * argv[])
{
    int iRet = 0; 
    TCHAR TServceName[256];
    strcpy(TServceName,SVCSTARTNAME);
    SERVICE_TABLE_ENTRY dispatchTable[]= 
    {
     {TEXT(TServceName),(LPSERVICE_MAIN_FUNCTION)_ServiceMain}, 
     { NULL,NULL}
     
    };

    if(argc<=1)
    {
        cout<<"usage :"<<ENDL;
        cout<<"\"install\" "; 
        cout<<"or ";
        cout<<"\"uninstall\" ";
        cout<<"\"exec \""; 
        cout<<" parameter required in the command line"<<ENDL;
    }
    if(argc>1)
    {
        if(_stricmp("install",argv[1])==0)
        { 
            InstallService(); 
        }
        else if(_stricmp("uninstall",argv[1])==0)
        {
            UninstallService(); 
        }
        else if(_stricmp("exec",argv[1])==0)
        {
            RunService(SVCSTARTNAME);
        }
        else
        {
            cout<<"usage :"<<ENDL;
            cout<<"\"install\" "; 
            cout<<"or";
            cout<<"\"uninstall\" ";
            cout<<"\"exec \" "; 
            cout<<"or";
            cout<<" parameter required in the command line"<<ENDL;
        }
    }
    else 
    {
        BOOL success;
        success = StartServiceCtrlDispatcher(dispatchTable);
        if(success)
        {
            ErrorHandler("In StartServiceCtrlDispatcher",GetLastError()); 
        }
    } 
    iRet = 0;
    return iRet;
}
void MyServiceModel::InstallService()
{ 
    SC_HANDLE schService; 
    SC_HANDLE schSCManager; 
    TCHAR szPath[512];
    cout<<"Install Starting..."<<ENDL;

    if(!GetModuleFileName(NULL,szPath,512)) 
    {
        ErrorHandler("In GetModuleFileName",GetLastError()); 
        return;
    }
    schSCManager = OpenSCManager( 
        0, // pointer to machine name string
        0, // pointer to database name string
        SC_MANAGER_CREATE_SERVICE // type of access
        );
    if (!schSCManager)
    {
        ErrorHandler("In OpenScManager",GetLastError());
        return;
    }
// Install 
    schService = CreateService(
        schSCManager, // handle to service control manager database
        SVCSTARTNAME, // pointer to name of service to start
        SVCDISPLNAME, // pointer to display name 
        SERVICE_ALL_ACCESS,// type of access to service
        SERVICE_WIN32_OWN_PROCESS|SERVICE_INTERACTIVE_PROCESS ,// type of service
        SERVICE_DEMAND_START,// when to start service
        SERVICE_ERROR_NORMAL,// severity if service fails to start
        szPath, // pointer to name of binary file
        NULL, // pointer to name of load ordering group
        NULL, // pointer to variable to get tag identifier
        NULL, // pointer to array of dependency names
        NULL, // pointer to account name of service
        NULL // pointer to password for service account
        );
    if (!schService)
    {
        ErrorHandler("In CreateService",GetLastError());
        return;
    }
    else
    {
        cout << "Service installed\n";
    }

    CloseServiceHandle(schService);
    CloseServiceHandle(schSCManager);
    cout << "Install Ending...\n";
}

void MyServiceModel::UninstallService()
{
    SC_HANDLE schService;
    SC_HANDLE schSCManager; 
    BOOL success;
    SERVICE_STATUS svcStatus;
    cout << "Uninstall Starting..."<<ENDL;
    schSCManager = OpenSCManager(
        0,// pointer to machine name string 
        0,// pointer to database name string
        SC_MANAGER_CREATE_SERVICE // type of access
        );
    if (!schSCManager)
    {
        ErrorHandler("In OpenScManager",GetLastError());
        return;
    }
    schService = OpenService(
        schSCManager, // handle to service control manager database
        SVCSTARTNAME, // pointer to name of service to start
        SERVICE_ALL_ACCESS | DELETE// type of access to service
        );
    if (!schService)
    {
        ErrorHandler("In OpenService",GetLastError());
        return;
    }

    success = QueryServiceStatus(schService, &svcStatus);
    if (!success)
    {
        ErrorHandler("In QueryServiceStatus",GetLastError());
        return;
    }
    if (svcStatus.dwCurrentState != SERVICE_STOPPED)
    {
        cout << "Stopping service..."<<ENDL;
        success = ControlService(
            schService, // handle to service
            SERVICE_CONTROL_STOP, // control code
            &svcStatus // pointer to service status structure
            );
        if (!success)
        {
            ErrorHandler("In ControlService",GetLastError());
            return;
        }
    }
    do
    {
        QueryServiceStatus(schService,&svcStatus); 
        Sleep(500); 
    }while(SERVICE_STOP_PENDING==svcStatus.dwCurrentState); 

    if(SERVICE_STOPPED!=svcStatus.dwCurrentState)
    {
        ErrorHandler("Failed to Stop Service",GetLastError()); 
        return;
    }

    success = DeleteService(schService);

    if (success)
    {
        cout << "Service removed\n";
    }
    else
    {
        ErrorHandler("In DeleteService",GetLastError());
        return;
    }
    CloseServiceHandle(schService);
    CloseServiceHandle(schSCManager);
    cout << "Uninstal Ending..."<<ENDL;
}

void WINAPI MyServiceModel::ServiceMain(DWORD argc,TCHAR *argv[])
{
    BOOL success;
    serviceStatusHandle = RegisterServiceCtrlHandler(
        SVCSTARTNAME, (LPHANDLER_FUNCTION)_ServiceControlHandler
        );
    if (! serviceStatusHandle)
    { 
        terminate(GetLastError());
        return;
    }

    success = SendStatusToSCM(
        SERVICE_START_PENDING,
        NO_ERROR, 
        0, 
        1, 
        5000
    );
    if (!success)
    {
        terminate(GetLastError()); 
        return;
    }

    terminateEvent = CreateEvent (
        0, 
        TRUE,
        FALSE,
        0
        );
    if (! terminateEvent)
    {
        terminate(GetLastError());
        return;
    }

    success = SendStatusToSCM(
        SERVICE_START_PENDING,
        NO_ERROR, 
        0, 
        2, 
        1000
        );
    if (!success)
    {
        terminate(GetLastError()); 
        return;
    }

    success = InitService();
    if (!success)
    {
        terminate(GetLastError());
        return;
    }

    success = SendStatusToSCM(
        SERVICE_RUNNING,
        NO_ERROR, 
        0, 
        0, 
        0
        );
    if (!success)
    {
        terminate(GetLastError()); 
        return;
    }

    WaitForSingleObject ( terminateEvent, INFINITE);
    terminate(0);
}

void WINAPI MyServiceModel::ServiceControlHandler(DWORD contorlCode )
{
    DWORD currentState = 0;
    BOOL success;

    switch(contorlCode)
    {
    // There is no START option because
    // ServiceMain gets called on a start
    case SERVICE_CONTROL_STOP:
        currentState = SERVICE_STOP_PENDING;
        // Tell the SCM what's happening
        success = SendStatusToSCM(
            SERVICE_STOP_PENDING,
            NO_ERROR, 
            0, 
            1, 
            5000
            );
        // Not much to do if not successful
        // Stop the service
        StopService();
        return;
    case SERVICE_CONTROL_PAUSE:
        if ( runningService && ! pauseService)
        {
            // Tell the SCM what's happening
            success = SendStatusToSCM(
                SERVICE_PAUSE_PENDING,
                NO_ERROR, 
                0, 
                1, 
                1000);
            PauseService();
            currentState = SERVICE_PAUSED;
        }
        break;
    case SERVICE_CONTROL_CONTINUE:
        if ( runningService && pauseService)
        {
        // Tell the SCM what's happening
            success = SendStatusToSCM(
            SERVICE_CONTINUE_PENDING,
                NO_ERROR, 
                0, 
                1, 
                1000
                );
            ResumeService();
            currentState = SERVICE_RUNNING;
        }
        break;

    case SERVICE_CONTROL_INTERROGATE:
        // it will fall to bottom and send status
        break;
        // Do nothing in a shutdown. Could do cleanup
        // here but it must be very quick.
    case SERVICE_CONTROL_SHUTDOWN:
        // Do nothing on shutdown
        return;
    default:
        break;
    }

    SendStatusToSCM(
        currentState, 
        NO_ERROR,
        0, 
        0, 
        0
        );
}


LRESULT CALLBACK MyServiceModel::WndProc(HWND hDlg, UINT Msg, WPARAM wParam ,LPARAM lParam)
{
    LRESULT lrRet;
    switch(Msg)
    { 
        case WM_DESTROY:
        PostQuitMessage(0);
        break;
    };
    lrRet = DefWindowProc(hDlg,Msg,wParam,lParam);
    return lrRet;
}

void MyServiceModel::ErrorHandler(char *s, DWORD err)
{
    cout <<s<<ENDL;
    cout << "Error number: " << err << endl;

    char str1[50];
    char str2[20];

    memset(str1,0,sizeof(str1));
    memset(str2,0,sizeof(str2));

    strcpy(str1,s);

    strcat(str1,", Error Code: ");
    _itoa(err,str2,10);
    strcat(str1,str2);

    ExitProcess(err);
}

VOID MyServiceModel::terminate(DWORD error)
{
    // if terminateEvent has been created, close it.
    if (terminateEvent)
        CloseHandle(terminateEvent);

    // Send a message to the scm to tell about
    // stopage
    if (serviceStatusHandle)
        SendStatusToSCM(SERVICE_STOPPED, error, 0, 0, 0);

    // If the thread has started kill it off
    if (threadHandle)
        CloseHandle(threadHandle);
    // Do not need to close serviceStatusHandle
}


BOOL MyServiceModel::SendStatusToSCM (DWORD dwCurrentState, DWORD dwWin32ExitCode, 
                        DWORD dwServiceSpecificExitCode, DWORD dwCheckPoint, DWORD dwWaitHint)
{
    BOOL success;
    SERVICE_STATUS serviceStatus;

    // Fill in all of the SERVICE_STATUS fields
    serviceStatus.dwServiceType =
    SERVICE_WIN32_OWN_PROCESS;
    serviceStatus.dwCurrentState = dwCurrentState;

    // If in the process of something, then accept
    // no control events, else accept anything
    if (dwCurrentState == SERVICE_START_PENDING)
    {
        serviceStatus.dwControlsAccepted = 0;
    }
    else
    {
        serviceStatus.dwControlsAccepted = SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_PAUSE_CONTINUE | SERVICE_ACCEPT_SHUTDOWN;
    }

    // if a specific exit code is defines, set up
    // the win32 exit code properly
    if (dwServiceSpecificExitCode == 0)
    {
        serviceStatus.dwWin32ExitCode = dwWin32ExitCode;
    }
    else
    {
    serviceStatus.dwWin32ExitCode = ERROR_SERVICE_SPECIFIC_ERROR;
    }

    serviceStatus.dwServiceSpecificExitCode = dwServiceSpecificExitCode;

    serviceStatus.dwCheckPoint = dwCheckPoint;
    serviceStatus.dwWaitHint = dwWaitHint;

    // Pass the status record to the SCM
    success = SetServiceStatus (serviceStatusHandle, &serviceStatus);
    if (!success)
    {
        StopService();
    }
    return success;
}

BOOL MyServiceModel::InitService()
{
    DWORD id;
    // Start the service's thread
    threadHandle = CreateThread(0, 0,
        (LPTHREAD_START_ROUTINE) ServiceThread, 0, 0, &id);

    if (threadHandle==0)
    {
        return FALSE;
    }
    else
    {
        runningService = TRUE;
        return TRUE;
    }
}

BOOL MyServiceModel::RunService(LPCTSTR sSvcName)
{
    SC_HANDLE schSCManager;
    SC_HANDLE scHandle;
    BOOL boolRet;
    schSCManager = OpenSCManager(
        0, 
        0, 
        SC_MANAGER_ALL_ACCESS 
        );

    scHandle = OpenService(
        schSCManager, 
        sSvcName, 
        SERVICE_ALL_ACCESS 
        );

    boolRet = StartService(
        scHandle, 
        0, 
        NULL); 
    return boolRet;
}

VOID MyServiceModel::ResumeService()
{
    //pauseService=FALSE;
    //ResumeThread(threadHandle);
    return;
}

VOID MyServiceModel::PauseService()
{
    //pauseService = TRUE;
    //SuspendThread(threadHandle);
    return;
}


VOID MyServiceModel::StopService() 
{
    runningService=FALSE;
    // Set the event that is holding ServiceMain
    // so that ServiceMain can return
    SetEvent(terminateEvent);
}

DWORD WINAPI ServiceThread(LPDWORD param)
{ 
	//ftpController = new FtpController("192.168.0.210",21);
    return 0;
}

void WINAPI MyServiceModel::_ServiceMain(DWORD argc, TCHAR *argv[])
{
    objApp.ServiceMain(argc,argv);
}



void WINAPI MyServiceModel::_ServiceControlHandler(DWORD contorlCode)
{
    objApp.ServiceControlHandler(contorlCode);
}

int TransService::Transform(ftp__ArrayOfFile *flist, ftp__Position *from, int &response)
{
	FileSet fset = *flist;
	Position f = *from;
	response = ftpController->Transform(fset,f);
	return SOAP_OK;
}
int TransService::Stop(int &response)
{
	response = ftpController->Stop();
	return SOAP_OK;
}
int TransService::Resume(int &response)
{
	response = ftpController->Resume();
	return SOAP_OK;
}
int TransService::DeleteAll(int &response)
{
	response = ftpController->DeleteAll();
	return SOAP_OK;
}
int TransService::Delete(LONG64 id, int &response)
{
	printf("TransService::Delete\n");
	response = ftpController->Delete(id);
	if(response == -1)
	{
		return SOAP_FAULT;
	}
	return SOAP_OK;
}
int TransService::Start(int &response)
{
	response = ftpController->Start();
	return SOAP_OK;
}
int TransService::Pause(int &response)
{
	response = ftpController->Pause();
	return SOAP_OK;
}
int TransService::GetState(ftp__State *response)
{
	return SOAP_OK;
}