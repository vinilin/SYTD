#pragma once
#include <vector>
#include <map>
#include "FileSet.h"
#include "Position.h"
#include "State.h"

class FtpController
{
    friend class boost::serialization::access;
	typedef std::vector<FileSet> FileSetType;
public:
private:
	SOCKET			m_control;
	SOCKET			m_data;
	int				m_buferSize;
	HANDLE			m_workThread;
	FileSetType		m_fsets;
	Position		m_from;
	State			m_state;
public:
	FtpController(int bufferSize = 1024*1024);
	~FtpController();

public:
	int Transform(FileSet const& fset,Position &src);
	int Stop();
	int Pause();
	int Start();
	int Resume();
	int Delete(__int64 id);
	int DeleteAll();
	int Download(std::string src, std::string dst,__int64);
	__int64 Total();	
	__int64 Downloaded();	
	void CommitFileSet(__int64 );
	void CommitError(std::string const&err);
	std::string QueryFileSetPath(__int64 id);
	__int64 CaclTotalSize(FileSet &fset);
	void ProgressNotify(__int64 id);
	State const& GetState();
	
private:
    template<class Archive>
    void serialize(Archive & ar, const unsigned int /* file_version */)
	{
        ar  & BOOST_SERIALIZATION_NVP(m_buferSize)
            & BOOST_SERIALIZATION_NVP(m_fsets)
            & BOOST_SERIALIZATION_NVP(m_from)
            & BOOST_SERIALIZATION_NVP(m_state);
    }
	int SaveToFile();
	int LoadFromFile();
	static void WorkThread(LPVOID lparam);
};
