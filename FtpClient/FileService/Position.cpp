#include "Position.h"
Position::Position()
{
	m_usr = "lqq";
	m_pwd = "lqq";
	m_port = 21;

}

std::string const& Position::User()
{
	return m_usr;
}
std::string const& Position::Pwd()
{
	return m_pwd;
}
std::string const& Position::Path()
{
	return m_path;
}
std::string const& Position::Ip()
{
	return m_ip;
}

void Position::Port(int port)
{
	m_port = port;
}
int Position::Port()
{
	return m_port;
}
Position::Position(ftp__Position const& position)
{
	*this  = position;
}
Position& Position::operator = (ftp__Position const &position)
{
	m_path = position.path;
	m_ip = position.ip;
	m_port = position.port;
	m_usr = position.user;
	m_pwd = position.pwd;
	return *this;
}

int Position::SaveToFile(CIni &iniFile)
{
	/*
	iniFile.WritePrivateIntWL("Position","port",m_port);
	iniFile.WritePrivateStrWL("Position","ip",m_ip.c_str());
	iniFile.WritePrivateStrWL("Position","path",m_path.c_str());
	iniFile.WritePrivateStrWL("Position","usr",m_usr.c_str());
	iniFile.WritePrivateStrWL("Position","pwd",m_pwd.c_str());
	iniFile.SaveIniFile();
	*/
	return 0;
}
int Position::LoadFromFile(CIni &iniFile)
{
	/*
	m_port = iniFile.ReadPrivateIntWL("Position","port");
	iniFile.ReadPrivateStringWL("Position","ip",m_ip);
	iniFile.ReadPrivateStringWL("Position","path",m_path);
	iniFile.ReadPrivateStringWL("Position","usr",m_usr);
	iniFile.ReadPrivateStringWL("Position","pwd",m_pwd);
	*/
	return 0;
}