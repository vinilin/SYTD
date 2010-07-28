#include "XmlConfig.h"
#include <fstream>

std::string const& XmlConfig::Root()
{
	return m_rootPath;
}
std::string const& XmlConfig::User()
{
	return m_usr;
}
std::string const& XmlConfig::Password()
{
	return m_pwd;
}
unsigned short const& XmlConfig::Port()
{
	return m_port;
}

void XmlConfig::Root(std::string const& rhs)
{
	m_rootPath = rhs;
}
void XmlConfig::User(std::string const& rhs)
{
	m_usr = rhs;
}
void XmlConfig::Password(std::string const& rhs)
{
	m_pwd = rhs;
}
void XmlConfig::FtpPort(unsigned short const &rhs)
{
	m_ftpPort = rhs;
}
void XmlConfig::Port(unsigned short const &rhs)
{
	m_port = rhs;
}

template<class Archive>
void XmlConfig::serialize(Archive & ar, const unsigned int /* file_version */)
{
    ar  & BOOST_SERIALIZATION_NVP(m_rootPath)
		& BOOST_SERIALIZATION_NVP(m_usr)
        & BOOST_SERIALIZATION_NVP(m_pwd)
        & BOOST_SERIALIZATION_NVP(m_port);
}

XmlConfig::XmlConfig(std::string const&fname)
{
	m_configPath = fname;
	m_rootPath = "";
	m_usr = "lqq";
	m_pwd = "lqq";
	m_port = 50000;
	m_ftpPort = 21;
	std::ifstream ifs("config.xml",std::ifstream::binary);
	if(ifs.good())
	{
		boost::archive::xml_iarchive ia(ifs);
		ia >> BOOST_SERIALIZATION_NVP(*this);
	}
	else
	{
		std::ofstream ofs("config.xml",std::ofstream::binary);
		if(ofs.good())
		{
			boost::archive::xml_oarchive oa(ofs);
			oa << BOOST_SERIALIZATION_NVP(this);
		}
	}
}

XmlConfig::~XmlConfig()
{

}