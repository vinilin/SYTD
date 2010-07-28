#include "stdafx.h"
#include "FtpController.h"
#include <fstream>
#include "../FileSetMan/FSMFileSetManSoap12Proxy.h"
#include "../FileSetMan/FileSetManSoap12.nsmap"
#include <boost/filesystem.hpp>
#include <boost/archive/xml_iarchive.hpp>
using namespace FSM;

typedef int (*progress_call_back)(__int64 m_total, __int64 downloaded);
extern SOCKET WINAPI connect(std::string const& ip, unsigned int const &port );
//extern int WINAPI download(SOCKET s, std::string src, std::string dst, progress_call_back call_back = NULL);
extern int WINAPI send_command(SOCKET s,std::string const& cmd, std::string &answer);
extern int WINAPI send_command(SOCKET s,std::string const& cmd);
extern int WINAPI recv_answer(SOCKET s,std::string & ans);
extern int WINAPI file_size(SOCKET s, std::string const &file,__int64 &size);
extern int WINAPI getaddress(SOCKET s, std::string& ip, unsigned int &port);
extern SOCKET WINAPI connect(std::string const& ip, unsigned int const &port );
extern int WINAPI loggin(SOCKET s, std::string const& usr, std::string const &port);
extern int WINAPI greeting(SOCKET s);

FtpController::FtpController(int bufferSize)
	:m_control(INVALID_SOCKET), m_data(INVALID_SOCKET), m_buferSize(bufferSize)
	, m_workThread(INVALID_HANDLE_VALUE)
{
	LoadFromFile();
	if(m_state.m_status == RUNNING)
	{
		Start();
	}
}
FtpController::~FtpController()
{

}
int FtpController::Download(std::string src, std::string dst,__int64 id)
{
	m_state.RestFState();
	int rv = 0;
	_ASSERT(m_control != INVALID_SOCKET);
	// 设置传输模式
	rv = send_command(m_control, "TYPE I");
	int fd;
	// 获取文件尺寸
	rv = file_size(m_control, src, m_state.m_fsize);
	if(m_state.m_fsize < 0)
	{
		CommitError("获取文件大小失败！");
		return -1;
	}

	int flag = _O_BINARY | _O_RDWR;
	// 判断本地文件是否存在
	if(_access(dst.c_str(),0) == -1)
	{
		flag |= _O_CREAT;
	}
	// 打开文件
	fd = _open(dst.c_str(), flag, _S_IWRITE | _S_IREAD);
	if(fd == -1)
	{
		CommitError("打开文件失败！");
		return -1;
	}
	// 转移至文件末尾
	_lseeki64(fd,0,SEEK_END);
	// 获取文件大小
	m_state.m_fdownloaded = _telli64(fd);
	m_state.m_downloaded += m_state.m_fdownloaded;

	// 判断文件是否传输完成
	if(m_state.m_fdownloaded == m_state.m_fsize)
	{
		ProgressNotify(id);
		_close(fd);
		return 0;
	}
	// 判断是否需要断点续传
	if(m_state.m_fdownloaded > 0)
	{
		// 需要断点续传，设置文件传输偏移量
		char cmd[255] = {0};
		_snprintf(cmd, 255,"REST %lld",m_state.m_fdownloaded);
		rv  = send_command(m_control,cmd);
	}

	unsigned int port;
	std::string ip;
	// 获取数据端口
	getaddress(m_control, ip, port);

	// 准备传输文件
	rv = send_command(m_control, "RETR " + src);
	if((rv != 150) && (rv != 125) && (rv != 226))
	{
		_close(fd);
		return 0 ;
	}

	// 链接Ftp服务器数据端口
	m_data = connect(ip,port);
	if(m_data == INVALID_SOCKET)
	{
		_close(fd);
		return 0;
	}
	//读取数据
	char *buf = new char[m_buferSize];
	__int64 nc = 0;
	while (m_state.m_fdownloaded < m_state.m_fsize)
	{
		// 判断状态
		if((m_state.m_status == STOP) || (m_state.m_status == PAUSE))
		{
			SaveToFile();
			closesocket(m_data);
			_close(fd);
			m_data = INVALID_SOCKET;
			return -1;
		}
		int toread;
		int writen;
		int bytes;
		// 计算数据读取长度
		__int64 left = m_state.m_fsize - m_state.m_fdownloaded;
		toread = (left < m_buferSize) ? left  : m_buferSize;
		// 接受数据
		bytes = recv(m_data, buf, toread, 0);
		if(bytes <= 0)
			break;
		//printf("recv bytes = %d\n bufferlen = %d",bytes,m_buferSize);
		// 写入文件
		writen = _write(fd,buf,bytes);
		if(writen != bytes )
			break;
		// 本次下载量
		m_state.m_fdownloaded += bytes;
		// 累计下载量
		m_state.m_downloaded += bytes;
		nc+=bytes;
		if(nc >(5*1024*1024))
		{
			nc = 0;
			ProgressNotify(id);
		}
	}
	ProgressNotify(id);
	closesocket(m_data);
	_close(fd);
	m_data = INVALID_SOCKET;
	std::string answer;
	recv_answer(m_control,answer);
	delete []buf;
	return 0;
}
void FtpController::WorkThread(LPVOID lparam)
{
	FtpController * my = (FtpController *)lparam;
	int rv = 0;
	// 连接ftp服务器
	my->m_control = connect(my->m_from.Ip(), my->m_from.Port());
	if(my->m_control == INVALID_SOCKET)
	{
		DWORD error = WSAGetLastError();
		printf("socket error, errcode = %d",error);
		my->CommitError("链接服务器失败");
		my->m_workThread = INVALID_HANDLE_VALUE;
		return;
	}
	// 接收欢迎信息
	greeting(my->m_control);
	// 登陆ftp服务器
	rv = loggin(my->m_control,my->m_from.User(), my->m_from.Pwd());
	for(int i =0; i < my->m_fsets.size(); ++i)
	{
		my->m_state.RestAll();
		boost::filesystem::path srcPath(my->m_fsets[i].m_path);
		std::string cmd = "CWD "+srcPath.string()+"\r\n";
		send_command(my->m_control,cmd);
		my->m_state.m_total = my->CaclTotalSize(my->m_fsets[i]);
		for(int j = 0; j < my->m_fsets[i].size(); ++j)
		{
			boost::filesystem::create_directories(my->m_fsets[i].m_path);
			std::string name = my->m_fsets[i][j].Name();
			if(my->Download(name, my->m_fsets[i].Path() + name,my->m_fsets[i].m_id) != 0)
			{
				my->CommitError("下载文件失败");
				continue;
			}
		}
		cmd = "CWD /\r\n";
		send_command(my->m_control,cmd);
		my->CommitFileSet(my->m_fsets[i].m_id);
	}
	my->m_fsets.clear();
	remove("Download.xml");
	my->m_workThread = INVALID_HANDLE_VALUE;
	send_command(my->m_control,"QUIT");
	return;
}
void FtpController::ProgressNotify(__int64 id)
{
	FileSetManSoap12Proxy proxy;
	proxy.namespaces = FSM::namespaces;
	_ns1__ProgressNotify notify;
	notify.id = id;
	notify.total = m_state.m_total;
	notify.downloaded = m_state.m_downloaded;
	printf("id = %lld,total = %lld, downloade =%lld \n",id,m_state.m_total,m_state.m_downloaded);
	_ns1__ProgressNotifyResponse resp;

	if(proxy.ProgressNotify(&notify,&resp) != SOAP_OK)
	{
		proxy.soap_stream_fault(std::cout);
	}
}
void FtpController::CommitFileSet(__int64 id)
{
	FileSetManSoap12Proxy proxy;
	proxy.namespaces = FSM::namespaces;
	_ns1__CommitFileSetResponse resp;
	_ns1__CommitFileSet fs;
	fs.id = id;
	if(proxy.CommitFileSet(&fs,&resp) != SOAP_OK)
	{
		proxy.soap_stream_fault(std::cout);
	}
}

void FtpController::CommitError(std::string const&err)
{
	/*
	FileSetManSoap12Proxy proxy;
	proxy.namespaces = FSM::namespaces;
	_ns1__CommitError res;
	_ns1__CommitErrorResponse resp;
	res.msg = &err;
	if(proxy.CommitError(&res,&resp) != SOAP_OK)
	{
		proxy.soap_stream_fault(std::cout);
	}
	*/
}
std::string FtpController::QueryFileSetPath(__int64 id)
{
	printf("FtpController::QueryFileSetPath(%lld)\n",id);
	FileSetManSoap12Proxy proxy;
	soap_set_mode(&proxy, SOAP_C_MBSTRING);
	proxy.namespaces = FSM::namespaces;
	_ns1__QureyFileSet res;
	_ns1__QureyFileSetResponse resp;
	res.id = id;
	if(proxy.QureyFileSet(&res, &resp) != SOAP_OK)
	{
		proxy.soap_stream_fault(std::cout);
		return "";
	}
	if(resp.QureyFileSetResult == NULL)
	{
		return "";
	}
	return *resp.QureyFileSetResult->Path;

}
__int64 FtpController::CaclTotalSize(FileSet &fset)
{
	__int64 total=0;
	for(int j = 0; j < fset.size(); ++j)
	{
		__int64 fsize = 0;
		std::string name = fset[j].Name();
		file_size(m_control, name, fsize);
		if(fsize >0)
		{
			total+=fsize;
		}
	}
	return total;
}
int FtpController::Transform(FileSet const &fset ,Position &from)
{
	//printf("FtpController::Transform(%s,%s)\n",fset.Path().c_str(),from.Ip().c_str());
	m_from = from;
	m_from.Port(21);
	m_from.User("lqq");
	m_from.Pwd("lqq");
	printf("FtpController::Transform(%s,%s)\n",fset.m_path.c_str(),from.Ip().c_str());
	m_fsets.push_back(fset);
	SaveToFile();
	return 0;
}

int FtpController::Start()
{
	if(m_workThread != INVALID_HANDLE_VALUE)
	{
		_ASSERT((m_state.m_status == STOP) || (m_state.m_status == PAUSE));
		return -1;
	}
	//_ASSERT(m_workThread == INVALID_HANDLE_VALUE);
	m_state.m_status = RUNNING;
	SaveToFile();
	m_workThread = (HANDLE) _beginthread(WorkThread,0,this);
	return 0;
}
int FtpController::Stop()
{
	printf("FtpController::Stop()\n");
//	_ASSERT(m_state.m_status == RUNNING);
	m_state.m_status = STOP;
	SaveToFile();
	return 0;
}
int FtpController::Pause()
{
	printf("FtpController::Pause()\n");
	_ASSERT(m_state.m_status == RUNNING);
	m_state.m_status = PAUSE;
	SaveToFile();
	return 0;
}
int FtpController::Resume()
{
	printf("FtpController::Resume()\n");
	_ASSERT(m_state.m_status == PAUSE);
	LoadFromFile();
	Start();
	return 0;
}
int FtpController::Delete(__int64 id)
{
	printf("FtpController::Delete(%lld)\n",id);
	FileSetType::iterator beg,end;	
	beg = m_fsets.begin();
	end = m_fsets.end();
	for (;beg  != end; )
	{
		if(beg->m_id == id)
		{
			beg = m_fsets.erase(beg);
			SaveToFile();
			break;
		}
		else
		{
			++beg;
		}
	}
	std::string path = QueryFileSetPath(id);
	printf("QueryFileSetPath(%lld) : %s \n",id,path.c_str());
	if(path.length() != 0)
	{
		try
		{
			if(boost::filesystem::exists(path)) 
				boost::filesystem::remove_all(path);
		}
		catch(std::exception &e)
		{
			return -1;
		}
	}
	return 0;
}
int FtpController::DeleteAll()
{
	return 0;
}

int FtpController::SaveToFile()
{
	std::ofstream ofs("download.xml");
    boost::archive::xml_oarchive oa(ofs);
    oa << BOOST_SERIALIZATION_NVP(this);
	return 0;
}
int FtpController::LoadFromFile()
{
	std::ifstream ifs("download.xml", std::ios::binary);
	if(!ifs.good())
	{
		return 0;
	}
	boost::archive::xml_iarchive ia(ifs);
	ia >> BOOST_SERIALIZATION_NVP(*this);
	return 0;
}

State const& FtpController::GetState()
{
	return m_state;
}