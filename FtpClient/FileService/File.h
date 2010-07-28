#pragma once
#include "FtpTransService.h"
#include <boost/serialization/nvp.hpp>
#include <boost/serialization/version.hpp>
class CIni;
class File
{
    friend class boost::serialization::access;
public:
	__int64		m_id;
	int			m_status;
	std::string m_name;
public:
	File();
	File(ftp__File const& file);
public:
	std::string const& Name()
	{
		return m_name;
	}
public:
    template<class Archive>
    void serialize(Archive & ar, const unsigned int /* file_version */)
	{
        ar  & BOOST_SERIALIZATION_NVP(m_id)
            & BOOST_SERIALIZATION_NVP(m_name)
            & BOOST_SERIALIZATION_NVP(m_status);
    }
	File& operator = (ftp__File const & file);
};

