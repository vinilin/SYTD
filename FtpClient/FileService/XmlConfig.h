#pragma  once
#include <boost/archive/polymorphic_xml_oarchive.hpp>
#include <boost/archive/polymorphic_xml_iarchive.hpp>

class XmlConfig
{
    friend class boost::serialization::access;
private:
	std::string		m_rootPath;
	std::string		m_usr;
	std::string		m_pwd;
	unsigned short	m_port;
	unsigned short	m_ftpPort;
private:
	std::string		m_configPath;
public:
	XmlConfig(std::string const& rhs);
	~XmlConfig();
public:
	std::string const& Root();
	std::string const& User();
	std::string const& Password();
	unsigned short const& Port();
	unsigned short const& FtpPort();

	void Root(std::string const& rhs);
	void User(std::string const& rhs);
	void Password(std::string const& rhs);
	void Port(unsigned short const& rhs);
	void FtpPort(unsigned short const& rhs);
public:
	void Load();
	void Save();
    template<class Archive>
    void serialize(Archive & ar, const unsigned int /* file_version */);

};