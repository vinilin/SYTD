#include "State.h"

void State::RestAll()
{
	m_total = 0;
	m_downloaded = 0;
	m_fsize = 0;
	m_fdownloaded = 0;
	m_fname = "";
//	m_status = 0;
}
void State::RestFState()
{
	m_fsize = 0;
	m_fdownloaded = 0;
	m_fname = "";
}
