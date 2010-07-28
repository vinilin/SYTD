/* FSMFileSetManSoapService.cpp
   Generated by gSOAP 2.7.14 from FileSetMan.h
   Copyright(C) 2000-2009, Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/

#include "FSMFileSetManSoapService.h"

namespace FSM {

FileSetManSoapService::FileSetManSoapService()
{	FileSetManSoapService_init(SOAP_IO_DEFAULT, SOAP_IO_DEFAULT);
}

FileSetManSoapService::FileSetManSoapService(const struct soap &soap)
{	soap_copy_context(this, &soap);
	FileSetManSoapService_init(soap.imode, soap.omode);
}

FileSetManSoapService::FileSetManSoapService(soap_mode iomode)
{	FileSetManSoapService_init(iomode, iomode);
}

FileSetManSoapService::FileSetManSoapService(soap_mode imode, soap_mode omode)
{	FileSetManSoapService_init(imode, omode);
}

FileSetManSoapService::~FileSetManSoapService()
{ }

void FileSetManSoapService::FileSetManSoapService_init(soap_mode imode, soap_mode omode)
{	static const struct Namespace namespaces[] =
{
	{"SOAP-ENV", "http://www.w3.org/2003/05/soap-envelope", "http://www.w3.org/2003/05/soap-envelope", NULL},
	{"SOAP-ENC", "http://www.w3.org/2003/05/soap-encoding", "http://www.w3.org/2003/05/soap-encoding", NULL},
	{"xsi", "http://www.w3.org/2001/XMLSchema-instance", "http://www.w3.org/*/XMLSchema-instance", NULL},
	{"xsd", "http://www.w3.org/2001/XMLSchema", "http://www.w3.org/*/XMLSchema", NULL},
	{"ns2", "http://localhost/FSM/FileSetManSoap", NULL, NULL},
	{"ns1", "http://localhost/FSM", NULL, NULL},
	{"ns3", "http://localhost/FSM/FileSetManSoap12", NULL, NULL},
	{NULL, NULL, NULL, NULL}
};
	soap_imode(this, imode);
	soap_omode(this, omode);
	if (!this->namespaces)
		this->namespaces = namespaces;
};

FileSetManSoapService *FileSetManSoapService::copy()
{	FileSetManSoapService *dup = new FileSetManSoapService(*(struct soap*)this);
	return dup;
}

int FileSetManSoapService::soap_close_socket()
{	return soap_closesock(this);
}

int FileSetManSoapService::soap_senderfault(const char *string, const char *detailXML)
{	return ::soap_sender_fault(this, string, detailXML);
}

int FileSetManSoapService::soap_senderfault(const char *subcodeQName, const char *string, const char *detailXML)
{	return ::soap_sender_fault_subcode(this, subcodeQName, string, detailXML);
}

int FileSetManSoapService::soap_receiverfault(const char *string, const char *detailXML)
{	return ::soap_receiver_fault(this, string, detailXML);
}

int FileSetManSoapService::soap_receiverfault(const char *subcodeQName, const char *string, const char *detailXML)
{	return ::soap_receiver_fault_subcode(this, subcodeQName, string, detailXML);
}

void FileSetManSoapService::soap_print_fault(FILE *fd)
{	::soap_print_fault(this, fd);
}

#ifndef WITH_LEAN
void FileSetManSoapService::soap_stream_fault(std::ostream& os)
{	::soap_stream_fault(this, os);
}

char *FileSetManSoapService::soap_sprint_fault(char *buf, size_t len)
{	return ::soap_sprint_fault(this, buf, len);
}
#endif

void FileSetManSoapService::soap_noheader()
{	header = NULL;
}

int FileSetManSoapService::run(int port)
{	if (soap_valid_socket(bind(NULL, port, 100)))
	{	for (;;)
		{	if (!soap_valid_socket(accept()))
				return this->error;
			(void)serve();
			soap_destroy(this);
			soap_end(this);
		}
	}
	else
		return this->error;
	return SOAP_OK;
}

SOAP_SOCKET FileSetManSoapService::bind(const char *host, int port, int backlog)
{	return soap_bind(this, host, port, backlog);
}

SOAP_SOCKET FileSetManSoapService::accept()
{	return soap_accept(this);
}

int FileSetManSoapService::serve()
{
#ifndef WITH_FASTCGI
	unsigned int k = this->max_keep_alive;
#endif
	do
	{	soap_begin(this);
#ifdef WITH_FASTCGI
		if (FCGI_Accept() < 0)
		{
			this->error = SOAP_EOF;
			return soap_send_fault(this);
		}
#endif

		soap_begin(this);

#ifndef WITH_FASTCGI
		if (this->max_keep_alive > 0 && !--k)
			this->keep_alive = 0;
#endif

		if (soap_begin_recv(this))
		{	if (this->error < SOAP_STOP)
			{
#ifdef WITH_FASTCGI
				soap_send_fault(this);
#else 
				return soap_send_fault(this);
#endif
			}
			soap_closesock(this);

			continue;
		}

		if (soap_envelope_begin_in(this)
		 || soap_recv_header(this)
		 || soap_body_begin_in(this)
		 || dispatch() || (this->fserveloop && this->fserveloop(this)))
		{
#ifdef WITH_FASTCGI
			soap_send_fault(this);
#else
			return soap_send_fault(this);
#endif
		}

#ifdef WITH_FASTCGI
		soap_destroy(this);
		soap_end(this);
	} while (1);
#else
	} while (this->keep_alive);
#endif
	return SOAP_OK;
}

static int serve___ns2__CreateFileSet(FileSetManSoapService*);
static int serve___ns2__QureyFileSet(FileSetManSoapService*);
static int serve___ns2__DeleteFileSet(FileSetManSoapService*);
static int serve___ns2__CommitFileSet(FileSetManSoapService*);
static int serve___ns2__ProgressNotify(FileSetManSoapService*);
static int serve___ns2__CommitFile(FileSetManSoapService*);
static int serve___ns2__CommitError(FileSetManSoapService*);

int FileSetManSoapService::dispatch()
{	if (soap_peek_element(this))
		return this->error;
	if (!soap_match_tag(this, this->tag, "ns1:CreateFileSet"))
		return serve___ns2__CreateFileSet(this);
	if (!soap_match_tag(this, this->tag, "ns1:QureyFileSet"))
		return serve___ns2__QureyFileSet(this);
	if (!soap_match_tag(this, this->tag, "ns1:DeleteFileSet"))
		return serve___ns2__DeleteFileSet(this);
	if (!soap_match_tag(this, this->tag, "ns1:CommitFileSet"))
		return serve___ns2__CommitFileSet(this);
	if (!soap_match_tag(this, this->tag, "ns1:ProgressNotify"))
		return serve___ns2__ProgressNotify(this);
	if (!soap_match_tag(this, this->tag, "ns1:CommitFile"))
		return serve___ns2__CommitFile(this);
	if (!soap_match_tag(this, this->tag, "ns1:CommitError"))
		return serve___ns2__CommitError(this);
	return this->error = SOAP_NO_METHOD;
}

static int serve___ns2__CreateFileSet(FileSetManSoapService *soap)
{	struct __ns2__CreateFileSet soap_tmp___ns2__CreateFileSet;
	_ns1__CreateFileSetResponse ns1__CreateFileSetResponse;
	ns1__CreateFileSetResponse.soap_default(soap);
	soap_default___ns2__CreateFileSet(soap, &soap_tmp___ns2__CreateFileSet);
	soap->encodingStyle = NULL;
	if (!soap_get___ns2__CreateFileSet(soap, &soap_tmp___ns2__CreateFileSet, "-ns2:CreateFileSet", NULL))
		return soap->error;
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap->error;
	soap->error = soap->CreateFileSet(soap_tmp___ns2__CreateFileSet.ns1__CreateFileSet, &ns1__CreateFileSetResponse);
	if (soap->error)
		return soap->error;
	soap_serializeheader(soap);
	ns1__CreateFileSetResponse.soap_serialize(soap);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || ns1__CreateFileSetResponse.soap_put(soap, "ns1:CreateFileSetResponse", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	};
	if (soap_end_count(soap)
	 || soap_response(soap, SOAP_OK)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || ns1__CreateFileSetResponse.soap_put(soap, "ns1:CreateFileSetResponse", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap->error;
	return soap_closesock(soap);
}

static int serve___ns2__QureyFileSet(FileSetManSoapService *soap)
{	struct __ns2__QureyFileSet soap_tmp___ns2__QureyFileSet;
	_ns1__QureyFileSetResponse ns1__QureyFileSetResponse;
	ns1__QureyFileSetResponse.soap_default(soap);
	soap_default___ns2__QureyFileSet(soap, &soap_tmp___ns2__QureyFileSet);
	soap->encodingStyle = NULL;
	if (!soap_get___ns2__QureyFileSet(soap, &soap_tmp___ns2__QureyFileSet, "-ns2:QureyFileSet", NULL))
		return soap->error;
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap->error;
	soap->error = soap->QureyFileSet(soap_tmp___ns2__QureyFileSet.ns1__QureyFileSet, &ns1__QureyFileSetResponse);
	if (soap->error)
		return soap->error;
	soap_serializeheader(soap);
	ns1__QureyFileSetResponse.soap_serialize(soap);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || ns1__QureyFileSetResponse.soap_put(soap, "ns1:QureyFileSetResponse", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	};
	if (soap_end_count(soap)
	 || soap_response(soap, SOAP_OK)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || ns1__QureyFileSetResponse.soap_put(soap, "ns1:QureyFileSetResponse", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap->error;
	return soap_closesock(soap);
}

static int serve___ns2__DeleteFileSet(FileSetManSoapService *soap)
{	struct __ns2__DeleteFileSet soap_tmp___ns2__DeleteFileSet;
	_ns1__DeleteFileSetResponse ns1__DeleteFileSetResponse;
	ns1__DeleteFileSetResponse.soap_default(soap);
	soap_default___ns2__DeleteFileSet(soap, &soap_tmp___ns2__DeleteFileSet);
	soap->encodingStyle = NULL;
	if (!soap_get___ns2__DeleteFileSet(soap, &soap_tmp___ns2__DeleteFileSet, "-ns2:DeleteFileSet", NULL))
		return soap->error;
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap->error;
	soap->error = soap->DeleteFileSet(soap_tmp___ns2__DeleteFileSet.ns1__DeleteFileSet, &ns1__DeleteFileSetResponse);
	if (soap->error)
		return soap->error;
	soap_serializeheader(soap);
	ns1__DeleteFileSetResponse.soap_serialize(soap);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || ns1__DeleteFileSetResponse.soap_put(soap, "ns1:DeleteFileSetResponse", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	};
	if (soap_end_count(soap)
	 || soap_response(soap, SOAP_OK)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || ns1__DeleteFileSetResponse.soap_put(soap, "ns1:DeleteFileSetResponse", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap->error;
	return soap_closesock(soap);
}

static int serve___ns2__CommitFileSet(FileSetManSoapService *soap)
{	struct __ns2__CommitFileSet soap_tmp___ns2__CommitFileSet;
	_ns1__CommitFileSetResponse ns1__CommitFileSetResponse;
	ns1__CommitFileSetResponse.soap_default(soap);
	soap_default___ns2__CommitFileSet(soap, &soap_tmp___ns2__CommitFileSet);
	soap->encodingStyle = NULL;
	if (!soap_get___ns2__CommitFileSet(soap, &soap_tmp___ns2__CommitFileSet, "-ns2:CommitFileSet", NULL))
		return soap->error;
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap->error;
	soap->error = soap->CommitFileSet(soap_tmp___ns2__CommitFileSet.ns1__CommitFileSet, &ns1__CommitFileSetResponse);
	if (soap->error)
		return soap->error;
	soap_serializeheader(soap);
	ns1__CommitFileSetResponse.soap_serialize(soap);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || ns1__CommitFileSetResponse.soap_put(soap, "ns1:CommitFileSetResponse", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	};
	if (soap_end_count(soap)
	 || soap_response(soap, SOAP_OK)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || ns1__CommitFileSetResponse.soap_put(soap, "ns1:CommitFileSetResponse", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap->error;
	return soap_closesock(soap);
}

static int serve___ns2__ProgressNotify(FileSetManSoapService *soap)
{	struct __ns2__ProgressNotify soap_tmp___ns2__ProgressNotify;
	_ns1__ProgressNotifyResponse ns1__ProgressNotifyResponse;
	ns1__ProgressNotifyResponse.soap_default(soap);
	soap_default___ns2__ProgressNotify(soap, &soap_tmp___ns2__ProgressNotify);
	soap->encodingStyle = NULL;
	if (!soap_get___ns2__ProgressNotify(soap, &soap_tmp___ns2__ProgressNotify, "-ns2:ProgressNotify", NULL))
		return soap->error;
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap->error;
	soap->error = soap->ProgressNotify(soap_tmp___ns2__ProgressNotify.ns1__ProgressNotify, &ns1__ProgressNotifyResponse);
	if (soap->error)
		return soap->error;
	soap_serializeheader(soap);
	ns1__ProgressNotifyResponse.soap_serialize(soap);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || ns1__ProgressNotifyResponse.soap_put(soap, "ns1:ProgressNotifyResponse", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	};
	if (soap_end_count(soap)
	 || soap_response(soap, SOAP_OK)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || ns1__ProgressNotifyResponse.soap_put(soap, "ns1:ProgressNotifyResponse", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap->error;
	return soap_closesock(soap);
}

static int serve___ns2__CommitFile(FileSetManSoapService *soap)
{	struct __ns2__CommitFile soap_tmp___ns2__CommitFile;
	_ns1__CommitFileResponse ns1__CommitFileResponse;
	ns1__CommitFileResponse.soap_default(soap);
	soap_default___ns2__CommitFile(soap, &soap_tmp___ns2__CommitFile);
	soap->encodingStyle = NULL;
	if (!soap_get___ns2__CommitFile(soap, &soap_tmp___ns2__CommitFile, "-ns2:CommitFile", NULL))
		return soap->error;
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap->error;
	soap->error = soap->CommitFile(soap_tmp___ns2__CommitFile.ns1__CommitFile, &ns1__CommitFileResponse);
	if (soap->error)
		return soap->error;
	soap_serializeheader(soap);
	ns1__CommitFileResponse.soap_serialize(soap);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || ns1__CommitFileResponse.soap_put(soap, "ns1:CommitFileResponse", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	};
	if (soap_end_count(soap)
	 || soap_response(soap, SOAP_OK)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || ns1__CommitFileResponse.soap_put(soap, "ns1:CommitFileResponse", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap->error;
	return soap_closesock(soap);
}

static int serve___ns2__CommitError(FileSetManSoapService *soap)
{	struct __ns2__CommitError soap_tmp___ns2__CommitError;
	_ns1__CommitErrorResponse ns1__CommitErrorResponse;
	ns1__CommitErrorResponse.soap_default(soap);
	soap_default___ns2__CommitError(soap, &soap_tmp___ns2__CommitError);
	soap->encodingStyle = NULL;
	if (!soap_get___ns2__CommitError(soap, &soap_tmp___ns2__CommitError, "-ns2:CommitError", NULL))
		return soap->error;
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap->error;
	soap->error = soap->CommitError(soap_tmp___ns2__CommitError.ns1__CommitError, &ns1__CommitErrorResponse);
	if (soap->error)
		return soap->error;
	soap_serializeheader(soap);
	ns1__CommitErrorResponse.soap_serialize(soap);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || ns1__CommitErrorResponse.soap_put(soap, "ns1:CommitErrorResponse", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	};
	if (soap_end_count(soap)
	 || soap_response(soap, SOAP_OK)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || ns1__CommitErrorResponse.soap_put(soap, "ns1:CommitErrorResponse", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap->error;
	return soap_closesock(soap);
}

} // namespace FSM

/* End of server object code */