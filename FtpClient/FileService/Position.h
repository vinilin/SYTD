#pragma once
#include "FtpTransService.h"
#include <boost/archive/xml_oarchive.hpp>
class CIni;

class Position
{
    friend class boost::serialization::access;
private:
	std::string m_path;
	std::string m_ip;
	std::string m_usr;
	std::string m_pwd;
	int m_port;
public:
	Position();
	Position(ftp__Position const& position);
public:
	std::string const& Path();
	std::string const& Ip();
	std::string const& User();
	void User(std::string const& rhs)
	{
		m_usr = rhs;
	}
	std::string const& Pwd();
	void Pwd(std::string const& rhs)
	{
		m_pwd = rhs;
	}
	int Port();
	void Port(int port);
public:
    template<class Archive>
    void serialize(Archive & ar, const unsigned int /* file_version */)
	{
        ar  & BOOST_SERIALIZATION_NVP(m_ip)
            & BOOST_SERIALIZATION_NVP(m_port)
            & BOOST_SERIALIZATION_NVP(m_path)
            & BOOST_SERIALIZATION_NVP(m_usr)
            & BOOST_SERIALIZATION_NVP(m_pwd);
    }
	Position& operator = (ftp__Position const &position);
	int SaveToFile(CIni &iniFile);
	int LoadFromFile(CIni &iniFile);
};