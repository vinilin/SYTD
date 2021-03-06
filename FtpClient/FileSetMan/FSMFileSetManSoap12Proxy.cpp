/* FSMFileSetManSoap12Proxy.cpp
   Generated by gSOAP 2.7.14 from FileSetMan.h
   Copyright(C) 2000-2009, Robert van Engelen, Genivia Inc. All Rights Reserved.
   This part of the software is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/

#include "FSMFileSetManSoap12Proxy.h"

namespace FSM {

FileSetManSoap12Proxy::FileSetManSoap12Proxy()
{	FileSetManSoap12Proxy_init(SOAP_IO_DEFAULT, SOAP_IO_DEFAULT);
}

FileSetManSoap12Proxy::FileSetManSoap12Proxy(const struct soap &soap)
{	soap_copy_context(this, &soap);
	FileSetManSoap12Proxy_init(soap.imode, soap.omode);
}

FileSetManSoap12Proxy::FileSetManSoap12Proxy(soap_mode iomode)
{	FileSetManSoap12Proxy_init(iomode, iomode);
}

FileSetManSoap12Proxy::FileSetManSoap12Proxy(soap_mode imode, soap_mode omode)
{	FileSetManSoap12Proxy_init(imode, omode);
}

void FileSetManSoap12Proxy::FileSetManSoap12Proxy_init(soap_mode imode, soap_mode omode)
{	soap_imode(this, imode);
	soap_omode(this, omode);
	soap_endpoint = NULL;
	static const struct Namespace namespaces[] =
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
	if (!this->namespaces)
		this->namespaces = namespaces;
}

FileSetManSoap12Proxy::~FileSetManSoap12Proxy()
{ }

void FileSetManSoap12Proxy::soap_noheader()
{	header = NULL;
}

const SOAP_ENV__Fault *FileSetManSoap12Proxy::soap_fault()
{	return (const FSM::SOAP_ENV__Fault*)this->fault;
}

const char *FileSetManSoap12Proxy::soap_fault_string()
{	return *soap_faultstring(this);
}

const char *FileSetManSoap12Proxy::soap_fault_detail()
{	return *soap_faultdetail(this);
}

int FileSetManSoap12Proxy::soap_close_socket()
{	return soap_closesock(this);
}

void FileSetManSoap12Proxy::soap_print_fault(FILE *fd)
{	::soap_print_fault(this, fd);
}

#ifndef WITH_LEAN
void FileSetManSoap12Proxy::soap_stream_fault(std::ostream& os)
{	::soap_stream_fault(this, os);
}

char *FileSetManSoap12Proxy::soap_sprint_fault(char *buf, size_t len)
{	return ::soap_sprint_fault(this, buf, len);
}
#endif

int FileSetManSoap12Proxy::CreateFileSet(_ns1__CreateFileSet *ns1__CreateFileSet, _ns1__CreateFileSetResponse *ns1__CreateFileSetResponse)
{	struct soap *soap = this;
	struct __ns3__CreateFileSet soap_tmp___ns3__CreateFileSet;
	const char *soap_action = NULL;
	if (!soap_endpoint)
		soap_endpoint = "http://localhost/FSM/FileSetMan.asmx";
	soap_action = "http://localhost/FSM/CreateFileSet";
	soap->encodingStyle = NULL;
	soap_tmp___ns3__CreateFileSet.ns1__CreateFileSet = ns1__CreateFileSet;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___ns3__CreateFileSet(soap, &soap_tmp___ns3__CreateFileSet);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___ns3__CreateFileSet(soap, &soap_tmp___ns3__CreateFileSet, "-ns3:CreateFileSet", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___ns3__CreateFileSet(soap, &soap_tmp___ns3__CreateFileSet, "-ns3:CreateFileSet", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!ns1__CreateFileSetResponse)
		return soap_closesock(soap);
	ns1__CreateFileSetResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	ns1__CreateFileSetResponse->soap_get(soap, "ns1:CreateFileSetResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

int FileSetManSoap12Proxy::QureyFileSet(_ns1__QureyFileSet *ns1__QureyFileSet, _ns1__QureyFileSetResponse *ns1__QureyFileSetResponse)
{	struct soap *soap = this;
	struct __ns3__QureyFileSet soap_tmp___ns3__QureyFileSet;
	const char *soap_action = NULL;
	if (!soap_endpoint)
		soap_endpoint = "http://localhost/FSM/FileSetMan.asmx";
	soap_action = "http://localhost/FSM/QureyFileSet";
	soap->encodingStyle = NULL;
	soap_tmp___ns3__QureyFileSet.ns1__QureyFileSet = ns1__QureyFileSet;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___ns3__QureyFileSet(soap, &soap_tmp___ns3__QureyFileSet);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___ns3__QureyFileSet(soap, &soap_tmp___ns3__QureyFileSet, "-ns3:QureyFileSet", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___ns3__QureyFileSet(soap, &soap_tmp___ns3__QureyFileSet, "-ns3:QureyFileSet", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!ns1__QureyFileSetResponse)
		return soap_closesock(soap);
	ns1__QureyFileSetResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	ns1__QureyFileSetResponse->soap_get(soap, "ns1:QureyFileSetResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

int FileSetManSoap12Proxy::DeleteFileSet(_ns1__DeleteFileSet *ns1__DeleteFileSet, _ns1__DeleteFileSetResponse *ns1__DeleteFileSetResponse)
{	struct soap *soap = this;
	struct __ns3__DeleteFileSet soap_tmp___ns3__DeleteFileSet;
	const char *soap_action = NULL;
	if (!soap_endpoint)
		soap_endpoint = "http://localhost/FSM/FileSetMan.asmx";
	soap_action = "http://localhost/FSM/DeleteFileSet";
	soap->encodingStyle = NULL;
	soap_tmp___ns3__DeleteFileSet.ns1__DeleteFileSet = ns1__DeleteFileSet;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___ns3__DeleteFileSet(soap, &soap_tmp___ns3__DeleteFileSet);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___ns3__DeleteFileSet(soap, &soap_tmp___ns3__DeleteFileSet, "-ns3:DeleteFileSet", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___ns3__DeleteFileSet(soap, &soap_tmp___ns3__DeleteFileSet, "-ns3:DeleteFileSet", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!ns1__DeleteFileSetResponse)
		return soap_closesock(soap);
	ns1__DeleteFileSetResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	ns1__DeleteFileSetResponse->soap_get(soap, "ns1:DeleteFileSetResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

int FileSetManSoap12Proxy::CommitFileSet(_ns1__CommitFileSet *ns1__CommitFileSet, _ns1__CommitFileSetResponse *ns1__CommitFileSetResponse)
{	struct soap *soap = this;
	struct __ns3__CommitFileSet soap_tmp___ns3__CommitFileSet;
	const char *soap_action = NULL;
	if (!soap_endpoint)
		soap_endpoint = "http://localhost/FSM/FileSetMan.asmx";
	soap_action = "http://localhost/FSM/CommitFileSet";
	soap->encodingStyle = NULL;
	soap_tmp___ns3__CommitFileSet.ns1__CommitFileSet = ns1__CommitFileSet;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___ns3__CommitFileSet(soap, &soap_tmp___ns3__CommitFileSet);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___ns3__CommitFileSet(soap, &soap_tmp___ns3__CommitFileSet, "-ns3:CommitFileSet", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___ns3__CommitFileSet(soap, &soap_tmp___ns3__CommitFileSet, "-ns3:CommitFileSet", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!ns1__CommitFileSetResponse)
		return soap_closesock(soap);
	ns1__CommitFileSetResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	ns1__CommitFileSetResponse->soap_get(soap, "ns1:CommitFileSetResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

int FileSetManSoap12Proxy::ProgressNotify(_ns1__ProgressNotify *ns1__ProgressNotify, _ns1__ProgressNotifyResponse *ns1__ProgressNotifyResponse)
{	struct soap *soap = this;
	struct __ns3__ProgressNotify soap_tmp___ns3__ProgressNotify;
	const char *soap_action = NULL;
	if (!soap_endpoint)
		soap_endpoint = "http://localhost/FSM/FileSetMan.asmx";
	soap_action = "http://localhost/FSM/ProgressNotify";
	soap->encodingStyle = NULL;
	soap_tmp___ns3__ProgressNotify.ns1__ProgressNotify = ns1__ProgressNotify;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___ns3__ProgressNotify(soap, &soap_tmp___ns3__ProgressNotify);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___ns3__ProgressNotify(soap, &soap_tmp___ns3__ProgressNotify, "-ns3:ProgressNotify", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___ns3__ProgressNotify(soap, &soap_tmp___ns3__ProgressNotify, "-ns3:ProgressNotify", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!ns1__ProgressNotifyResponse)
		return soap_closesock(soap);
	ns1__ProgressNotifyResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	ns1__ProgressNotifyResponse->soap_get(soap, "ns1:ProgressNotifyResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

int FileSetManSoap12Proxy::CommitFile(_ns1__CommitFile *ns1__CommitFile, _ns1__CommitFileResponse *ns1__CommitFileResponse)
{	struct soap *soap = this;
	struct __ns3__CommitFile soap_tmp___ns3__CommitFile;
	const char *soap_action = NULL;
	if (!soap_endpoint)
		soap_endpoint = "http://localhost/FSM/FileSetMan.asmx";
	soap_action = "http://localhost/FSM/CommitFile";
	soap->encodingStyle = NULL;
	soap_tmp___ns3__CommitFile.ns1__CommitFile = ns1__CommitFile;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___ns3__CommitFile(soap, &soap_tmp___ns3__CommitFile);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___ns3__CommitFile(soap, &soap_tmp___ns3__CommitFile, "-ns3:CommitFile", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___ns3__CommitFile(soap, &soap_tmp___ns3__CommitFile, "-ns3:CommitFile", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!ns1__CommitFileResponse)
		return soap_closesock(soap);
	ns1__CommitFileResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	ns1__CommitFileResponse->soap_get(soap, "ns1:CommitFileResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

int FileSetManSoap12Proxy::CommitError(_ns1__CommitError *ns1__CommitError, _ns1__CommitErrorResponse *ns1__CommitErrorResponse)
{	struct soap *soap = this;
	struct __ns3__CommitError soap_tmp___ns3__CommitError;
	const char *soap_action = NULL;
	if (!soap_endpoint)
		soap_endpoint = "http://localhost/FSM/FileSetMan.asmx";
	soap_action = "http://localhost/FSM/CommitError";
	soap->encodingStyle = NULL;
	soap_tmp___ns3__CommitError.ns1__CommitError = ns1__CommitError;
	soap_begin(soap);
	soap_serializeheader(soap);
	soap_serialize___ns3__CommitError(soap, &soap_tmp___ns3__CommitError);
	if (soap_begin_count(soap))
		return soap->error;
	if (soap->mode & SOAP_IO_LENGTH)
	{	if (soap_envelope_begin_out(soap)
		 || soap_putheader(soap)
		 || soap_body_begin_out(soap)
		 || soap_put___ns3__CommitError(soap, &soap_tmp___ns3__CommitError, "-ns3:CommitError", "")
		 || soap_body_end_out(soap)
		 || soap_envelope_end_out(soap))
			 return soap->error;
	}
	if (soap_end_count(soap))
		return soap->error;
	if (soap_connect(soap, soap_endpoint, soap_action)
	 || soap_envelope_begin_out(soap)
	 || soap_putheader(soap)
	 || soap_body_begin_out(soap)
	 || soap_put___ns3__CommitError(soap, &soap_tmp___ns3__CommitError, "-ns3:CommitError", "")
	 || soap_body_end_out(soap)
	 || soap_envelope_end_out(soap)
	 || soap_end_send(soap))
		return soap_closesock(soap);
	if (!ns1__CommitErrorResponse)
		return soap_closesock(soap);
	ns1__CommitErrorResponse->soap_default(soap);
	if (soap_begin_recv(soap)
	 || soap_envelope_begin_in(soap)
	 || soap_recv_header(soap)
	 || soap_body_begin_in(soap))
		return soap_closesock(soap);
	ns1__CommitErrorResponse->soap_get(soap, "ns1:CommitErrorResponse", "");
	if (soap->error)
	{	if (soap->error == SOAP_TAG_MISMATCH && soap->level == 2)
			return soap_recv_fault(soap);
		return soap_closesock(soap);
	}
	if (soap_body_end_in(soap)
	 || soap_envelope_end_in(soap)
	 || soap_end_recv(soap))
		return soap_closesock(soap);
	return soap_closesock(soap);
}

} // namespace FSM

/* End of client proxy code */
