#include "FileSet.h"

FileSet::FileSet()
{

}
FileSet::FileSet(ftp__ArrayOfFile const &fset)
{
	*this = fset;
}
FileSet& FileSet::operator=(ftp__ArrayOfFile const &fset)
{
	m_id = fset.id;
	m_path = fset.path;
	m_status = fset.status;
	for(int i = 0; i < fset.__ptr.size(); ++i)
	{
		push_back(*fset.__ptr[i]);
	}
	return *this;
}
