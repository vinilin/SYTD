#include "File.h"

File::File()
{

}
File::File(ftp__File const& file)
{
	*this = file;
}
File& File::operator = (ftp__File const & file)
{
	m_id = file.id;
	m_name = file.name;
	m_status = file.status;
	return *this;
}

