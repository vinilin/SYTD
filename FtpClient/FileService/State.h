#pragma once
#include "FtpTransService.h"
#include <boost/archive/xml_oarchive.hpp>
class CIni;
/*
enum Status
{
	STOP = 1,
	RUNNING = 2,
	PAUSE = 3
};
*/
class State
{
    friend class boost::serialization::access;
public:
	__int64 m_total;
	__int64 m_downloaded;
	__int64 m_fsize;
	__int64 m_fdownloaded;
	std::string m_fname;
	int m_status;
public:
	void RestAll();
	void RestFState();

    template<class Archive>
    void serialize(Archive & ar, const unsigned int /* file_version */)
	{
        ar  & BOOST_SERIALIZATION_NVP(m_total)
            & BOOST_SERIALIZATION_NVP(m_downloaded)
            & BOOST_SERIALIZATION_NVP(m_fsize)
            & BOOST_SERIALIZATION_NVP(m_fdownloaded)
            & BOOST_SERIALIZATION_NVP(m_fname)
            & BOOST_SERIALIZATION_NVP(m_status);
    }
public:
	int SaveToFile(CIni &iniFile);
	int LoadFromFile(CIni &iniFile);
};