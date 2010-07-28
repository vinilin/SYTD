#pragma  once
#include "File.h"
#include <boost/serialization/utility.hpp>
#include <boost/serialization/list.hpp>
#include <boost/serialization/vector.hpp>
#include <boost/serialization/version.hpp>

class FileSet: public std::vector<File>
{
	typedef std::vector<File> BaseType;
    friend class boost::serialization::access;
public:
	__int64		m_id;
	int			m_status;
	std::string m_path;
public:
	FileSet();
	FileSet(ftp__ArrayOfFile const &fset);
public:
	std::string const& Path()
	{
		return m_path;
	}
public:
    template<class Archive>
    void serialize(Archive & ar, const unsigned int /* file_version */)
	{
		ar	& BOOST_SERIALIZATION_BASE_OBJECT_NVP(BaseType)
            & BOOST_SERIALIZATION_NVP(m_id)
            & BOOST_SERIALIZATION_NVP(m_path)
            & BOOST_SERIALIZATION_NVP(m_status);
    }
	FileSet& operator=(ftp__ArrayOfFile const &fset);
	int SaveToFile(CIni &file);
	int LoadFromFile(std::string fname = "Download.lst");
};