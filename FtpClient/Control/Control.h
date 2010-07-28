//gsoap ftp service name: Trans
//gsoap ftp service style: rpc
//gsoap ftp service encoding: encoded
//gsoap ftp service namespace: http://localhost:50000/
//gsoap ftp service location: http://localhost:50000/
//gsoap ftp schema namespace: urn:ftp

#import "stlvector.h"

typedef long long xsd__positiveInteger;
typedef std::string xsd__string;

class ftp__File
{
public:
	xsd__positiveInteger id;
	int status;
	xsd__string name;
};
class ftp__Position
{
public:	
	xsd__string path;
	xsd__string ip;
	xsd__string user;
	xsd__string pwd;
	int port;
};
class ftp__ArrayOfFile 
{
public:
	xsd__positiveInteger id;
	int status;
	xsd__string path;
	std::vector<ftp__File *> __ptr;
};
enum ftp__Status
{
	STOP = 1,
	RUNNING = 2,
	PAUSE = 3
};

class ftp__State
{
public:
	xsd__positiveInteger total;
	xsd__positiveInteger downloaded;
	xsd__positiveInteger fsize;
	xsd__positiveInteger fdownloaded;
	enum ftp__Status status;
	xsd__string fname;
};

int ftp__Transform(ftp__ArrayOfFile *flist,ftp__Position* position, int &response);
int ftp__Stop(int &response);
int ftp__Start(int &response);
int ftp__Pause(int &response);
int ftp__Resume(int &response);
int ftp__Delete(xsd__positiveInteger id, int &response);
int ftp__DeleteAll(int &response);
int ftp__GetState(ftp__State *response);
