// FtpClient.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"

#include <iostream>
#include <string>
#include <io.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <winsock2.h>

#pragma comment(lib,"ws2_32.lib")

int WINAPI recv_answer(SOCKET s, std::string &answer)
{
	_ASSERT(s != INVALID_SOCKET);
	char buffer[1024] = {0};
	int rt;
	int size = recv(s,buffer,1024,0);
	if(size == SOCKET_ERROR)
	{
		return SOCKET_ERROR;
	}
	_ASSERT((size > 4) && (size < 1024));
	answer = buffer;
	rt = atoi(answer.substr(0,3).c_str());
	return rt;
}

__declspec(dllexport) int WINAPI send_command(SOCKET s,std::string const& cmd, std::string &answer)
{
	_ASSERT(s != INVALID_SOCKET);
	std::string buff = cmd + "\r\n";
	int rv = send(s,buff.c_str(), buff.length(),0);
	if((rv == SOCKET_ERROR))
	{
		return SOCKET_ERROR;		
	}
	_ASSERT(rv == buff.length());
	rv = recv_answer(s,answer);
//#if defined _DEBUG	
	printf("command:%s\nresponse:%s\n",cmd.c_str(),answer.c_str());
//#endif
	return rv;
}

__declspec(dllexport) int WINAPI send_command(SOCKET s, std::string const& cmd)
{
	std::string answer;
	return send_command(s, cmd, answer);
}


int WINAPI file_size(SOCKET s, std::string const &file,__int64 &size)
{
	_ASSERT(s != INVALID_SOCKET);
	std::string answer;
	int rv = send_command(s, "SIZE " + file, answer);
	if((rv == SOCKET_ERROR) || (rv != 213))
	{
		size = -1;
		return rv;
	}

	sscanf(answer.substr(4,answer.length()).c_str(),"%I64d",&size);
	return rv;
}

int WINAPI getaddress(SOCKET s, std::string& ip, unsigned int &port) 
{
    unsigned int b[6]; 
	int rv ;
	std::string answer;

    if ((rv = send_command(s, "PASV", answer)) != 227) 
	{
		return 0;
	}
	int begin,end;
	begin = answer.find('(');
	end = answer.find(')');
	std::string port_str = answer.substr(begin+1,end - begin - 1);
    if (sscanf(port_str.c_str(), "%u,%u,%u,%u,%u,%u", 
	       b, b + 1, b + 2, b + 3, b + 4, b + 5) != 6)
	{
		return 0;
	}
    port =  b[4] * 256 + b[5];
	char tmp[255] = {0};
	_snprintf(tmp, 255, "%d.%d.%d.%d",b[0], b[1], b[2], b[3]);
	ip = tmp;
	return rv;
}

int WINAPI getport(SOCKET s, unsigned int &port)
{
	std::string ip;
	return getaddress(s, ip , port);
}

__declspec(dllexport) SOCKET WINAPI connect(std::string const& ip, unsigned int const &port )
{
    struct sockaddr_in address; 
	int s;
	s = socket(AF_INET, SOCK_STREAM, 0);
    if (s == INVALID_SOCKET)
	{
		return INVALID_SOCKET;
	}

    address.sin_family = AF_INET;
    address.sin_port = htons(port);
	struct hostent* server = 0;                        
	if (!(server = gethostbyname(ip.c_str()))) 
	{
		return INVALID_SOCKET;
	}
	memcpy(&address.sin_addr.s_addr, server->h_addr, server->h_length);
    if (connect(s, (struct sockaddr*) &address, sizeof(address)) == -1)
	{
		return INVALID_SOCKET;
	}
	return s;
}

typedef int (*progress_call_back)(__int64 total, __int64 downloaded);

int WINAPI download(SOCKET s, std::string src, std::string dst, progress_call_back call_back = NULL)
{
	_ASSERT(s != INVALID_SOCKET);
	int rv = 0;
	rv = send_command(s, "TYPE I");
	__int64 total = 0;
	__int64 start_pos = 0;
	int fd;

	rv = file_size(s, src, total);
	int flag = _O_BINARY | _O_RDWR;
	if(_access(dst.c_str(),0) == -1)
	{
		flag |= _O_CREAT;
	}

	fd = _open(dst.c_str(), flag, _S_IWRITE | _S_IREAD);
	_lseeki64(fd,0,SEEK_END);
	start_pos = _telli64(fd);

	if(start_pos > 0)
	{
		char cmd[255] = {0};
		_snprintf(cmd, 255,"REST %lld",start_pos);
		rv  = send_command(s,cmd);
	}

	unsigned int port;
	std::string ip;
	getaddress(s, ip, port);
	SOCKET data_sock = connect(ip,port);
	if(data_sock == INVALID_SOCKET)
	{
		return 0;
	}

	rv = send_command(s, "RETR " + src);
	if((rv != 150) && (rv != 125) && (rv != 226))
	{
		closesocket(data_sock);
	}
	while (start_pos < total)
	{
		if(call_back != NULL)
		{
			int ret = call_back(total , start_pos);
			if(ret == 0)
			{
				closesocket(data_sock);
				_close(fd);
				return 0;
			}
		}
		int toread;
		int writen;
		int bytes;
		char buf[2047] = {0};
		toread = ((total - start_pos) < 2047) ? total - start_pos : 2047;
		bytes = recv(data_sock, buf, toread, 0);
		if(bytes <= 0)
			break;
		writen = _write(fd,buf,bytes);
		if(writen != bytes )
			break;
		start_pos += bytes;
	}
	closesocket(data_sock);
	_close(fd);
	return 0;
}

__declspec(dllexport) int WINAPI loggin(SOCKET s, std::string const& usr, std::string const& pwd)
{
	std::string cmd("USER ");
	cmd += usr;
	int rv = send_command(s,cmd);
	if(rv == SOCKET_ERROR)
	{
		return SOCKET_ERROR;
	}
	switch(rv)
	{
	case 331:
		break;
	case 230:
	default:
		return rv;
	}
	cmd = "PASS " + pwd;
	rv = send_command(s,cmd);
	return rv;
}
__declspec(dllexport) int WINAPI greeting(SOCKET s)
{
	std::string welcome;
	return recv_answer(s,welcome);	
}
