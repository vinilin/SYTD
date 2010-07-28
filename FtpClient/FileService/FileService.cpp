// FileService.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <fstream>
#include <boost/archive/xml_oarchive.hpp>
#include <boost/archive/xml_iarchive.hpp>
#include "MyServiceModel.h"
#include "FtpController.h"
#include "FtpTransService.h"
extern FtpController* ftpController;
int _tmain(int argc, _TCHAR* argv[])
{
	WSADATA wsdata;
	WSAStartup(MAKEWORD(2,2),&wsdata);
	ftpController = new FtpController();
	/*
	{
	std::ofstream ofs("download.xml");
    boost::archive::xml_oarchive oa(ofs);
    oa << BOOST_SERIALIZATION_NVP(ftpController);
	}
	*/

//	Service srv(SOAP_C_UTFSTRING, SOAP_C_UTFSTRING);
	setlocale(LC_ALL,"");
	TransService srv;
	soap_set_mode(&srv, SOAP_C_MBSTRING);
	srv.run(50000);
//	objApp.main(argc, argv);
	return 0;
}
