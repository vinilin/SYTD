﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.3053
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShareMan
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="ShareMan")]
	public partial class ShareManDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertArea(Area instance);
    partial void UpdateArea(Area instance);
    partial void DeleteArea(Area instance);
    partial void InsertAudit(Audit instance);
    partial void UpdateAudit(Audit instance);
    partial void DeleteAudit(Audit instance);
    partial void InsertBaseItem(BaseItem instance);
    partial void UpdateBaseItem(BaseItem instance);
    partial void DeleteBaseItem(BaseItem instance);
    partial void InsertDistribute(Distribute instance);
    partial void UpdateDistribute(Distribute instance);
    partial void DeleteDistribute(Distribute instance);
    partial void InsertFileItem(FileItem instance);
    partial void UpdateFileItem(FileItem instance);
    partial void DeleteFileItem(FileItem instance);
    partial void InsertItemLink(ItemLink instance);
    partial void UpdateItemLink(ItemLink instance);
    partial void DeleteItemLink(ItemLink instance);
    partial void InsertPublishType(PublishType instance);
    partial void UpdatePublishType(PublishType instance);
    partial void DeletePublishType(PublishType instance);
    partial void InsertFileSetLink(FileSetLink instance);
    partial void UpdateFileSetLink(FileSetLink instance);
    partial void DeleteFileSetLink(FileSetLink instance);
    partial void InsertCartoon(Cartoon instance);
    partial void UpdateCartoon(Cartoon instance);
    partial void DeleteCartoon(Cartoon instance);
    partial void InsertMovie(Movie instance);
    partial void UpdateMovie(Movie instance);
    partial void DeleteMovie(Movie instance);
    partial void InsertSoftware(Software instance);
    partial void UpdateSoftware(Software instance);
    partial void DeleteSoftware(Software instance);
    partial void InsertMusic(Music instance);
    partial void UpdateMusic(Music instance);
    partial void DeleteMusic(Music instance);
    partial void InsertDuration(Duration instance);
    partial void UpdateDuration(Duration instance);
    partial void DeleteDuration(Duration instance);
    #endregion
		
		public ShareManDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["ShareManConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ShareManDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ShareManDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ShareManDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ShareManDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Area> Area
		{
			get
			{
				return this.GetTable<Area>();
			}
		}
		
		public System.Data.Linq.Table<Audit> Audit
		{
			get
			{
				return this.GetTable<Audit>();
			}
		}
		
		public System.Data.Linq.Table<BaseItem> BaseItem
		{
			get
			{
				return this.GetTable<BaseItem>();
			}
		}
		
		public System.Data.Linq.Table<Distribute> Distribute
		{
			get
			{
				return this.GetTable<Distribute>();
			}
		}
		
		public System.Data.Linq.Table<FileItem> FileItem
		{
			get
			{
				return this.GetTable<FileItem>();
			}
		}
		
		public System.Data.Linq.Table<ItemLink> ItemLink
		{
			get
			{
				return this.GetTable<ItemLink>();
			}
		}
		
		public System.Data.Linq.Table<PublishType> PublishType
		{
			get
			{
				return this.GetTable<PublishType>();
			}
		}
		
		public System.Data.Linq.Table<FileSetLink> FileSetLink
		{
			get
			{
				return this.GetTable<FileSetLink>();
			}
		}
		
		public System.Data.Linq.Table<Cartoon> Cartoon
		{
			get
			{
				return this.GetTable<Cartoon>();
			}
		}
		
		public System.Data.Linq.Table<Movie> Movie
		{
			get
			{
				return this.GetTable<Movie>();
			}
		}
		
		public System.Data.Linq.Table<Software> Software
		{
			get
			{
				return this.GetTable<Software>();
			}
		}
		
		public System.Data.Linq.Table<Music> Music
		{
			get
			{
				return this.GetTable<Music>();
			}
		}
		
		public System.Data.Linq.Table<Duration> Duration
		{
			get
			{
				return this.GetTable<Duration>();
			}
		}
	}
	
	[Table(Name="dbo.Area")]
	public partial class Area : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _Name;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public Area()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Audit")]
	public partial class Audit : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private long _AuditOwner;
		
		private bool _State;
		
		private System.DateTime _AuditDate;
		
		private string _Reason;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnAuditOwnerChanging(long value);
    partial void OnAuditOwnerChanged();
    partial void OnStateChanging(bool value);
    partial void OnStateChanged();
    partial void OnAuditDateChanging(System.DateTime value);
    partial void OnAuditDateChanged();
    partial void OnReasonChanging(string value);
    partial void OnReasonChanged();
    #endregion
		
		public Audit()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_AuditOwner", DbType="BigInt NOT NULL")]
		public long AuditOwner
		{
			get
			{
				return this._AuditOwner;
			}
			set
			{
				if ((this._AuditOwner != value))
				{
					this.OnAuditOwnerChanging(value);
					this.SendPropertyChanging();
					this._AuditOwner = value;
					this.SendPropertyChanged("AuditOwner");
					this.OnAuditOwnerChanged();
				}
			}
		}
		
		[Column(Storage="_State", DbType="Bit NOT NULL")]
		public bool State
		{
			get
			{
				return this._State;
			}
			set
			{
				if ((this._State != value))
				{
					this.OnStateChanging(value);
					this.SendPropertyChanging();
					this._State = value;
					this.SendPropertyChanged("State");
					this.OnStateChanged();
				}
			}
		}
		
		[Column(Storage="_AuditDate", DbType="DateTime NOT NULL")]
		public System.DateTime AuditDate
		{
			get
			{
				return this._AuditDate;
			}
			set
			{
				if ((this._AuditDate != value))
				{
					this.OnAuditDateChanging(value);
					this.SendPropertyChanging();
					this._AuditDate = value;
					this.SendPropertyChanged("AuditDate");
					this.OnAuditDateChanged();
				}
			}
		}
		
		[Column(Storage="_Reason", DbType="VarChar(256) NOT NULL", CanBeNull=false)]
		public string Reason
		{
			get
			{
				return this._Reason;
			}
			set
			{
				if ((this._Reason != value))
				{
					this.OnReasonChanging(value);
					this.SendPropertyChanging();
					this._Reason = value;
					this.SendPropertyChanged("Reason");
					this.OnReasonChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.BaseItem")]
	public partial class BaseItem : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private long _Owner;
		
		private System.DateTime _Birth;
		
		private string _Title;
		
		private long _Category;
		
		private long _PublishType;
		
		private System.DateTime _IssueDate;
		
		private string _Brief;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnOwnerChanging(long value);
    partial void OnOwnerChanged();
    partial void OnBirthChanging(System.DateTime value);
    partial void OnBirthChanged();
    partial void OnTitleChanging(string value);
    partial void OnTitleChanged();
    partial void OnCategoryChanging(long value);
    partial void OnCategoryChanged();
    partial void OnPublishTypeChanging(long value);
    partial void OnPublishTypeChanged();
    partial void OnIssueDateChanging(System.DateTime value);
    partial void OnIssueDateChanged();
    partial void OnBriefChanging(string value);
    partial void OnBriefChanged();
    #endregion
		
		public BaseItem()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_Owner", DbType="BigInt NOT NULL")]
		public long Owner
		{
			get
			{
				return this._Owner;
			}
			set
			{
				if ((this._Owner != value))
				{
					this.OnOwnerChanging(value);
					this.SendPropertyChanging();
					this._Owner = value;
					this.SendPropertyChanged("Owner");
					this.OnOwnerChanged();
				}
			}
		}
		
		[Column(Storage="_Birth", DbType="DateTime NOT NULL")]
		public System.DateTime Birth
		{
			get
			{
				return this._Birth;
			}
			set
			{
				if ((this._Birth != value))
				{
					this.OnBirthChanging(value);
					this.SendPropertyChanging();
					this._Birth = value;
					this.SendPropertyChanged("Birth");
					this.OnBirthChanged();
				}
			}
		}
		
		[Column(Storage="_Title", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}
			}
		}
		
		[Column(Storage="_Category", DbType="BigInt NOT NULL")]
		public long Category
		{
			get
			{
				return this._Category;
			}
			set
			{
				if ((this._Category != value))
				{
					this.OnCategoryChanging(value);
					this.SendPropertyChanging();
					this._Category = value;
					this.SendPropertyChanged("Category");
					this.OnCategoryChanged();
				}
			}
		}
		
		[Column(Storage="_PublishType", DbType="BigInt NOT NULL")]
		public long PublishType
		{
			get
			{
				return this._PublishType;
			}
			set
			{
				if ((this._PublishType != value))
				{
					this.OnPublishTypeChanging(value);
					this.SendPropertyChanging();
					this._PublishType = value;
					this.SendPropertyChanged("PublishType");
					this.OnPublishTypeChanged();
				}
			}
		}
		
		[Column(Storage="_IssueDate", DbType="DateTime NOT NULL")]
		public System.DateTime IssueDate
		{
			get
			{
				return this._IssueDate;
			}
			set
			{
				if ((this._IssueDate != value))
				{
					this.OnIssueDateChanging(value);
					this.SendPropertyChanging();
					this._IssueDate = value;
					this.SendPropertyChanged("IssueDate");
					this.OnIssueDateChanged();
				}
			}
		}
		
		[Column(Storage="_Brief", DbType="VarChar(2048) NOT NULL", CanBeNull=false)]
		public string Brief
		{
			get
			{
				return this._Brief;
			}
			set
			{
				if ((this._Brief != value))
				{
					this.OnBriefChanging(value);
					this.SendPropertyChanging();
					this._Brief = value;
					this.SendPropertyChanged("Brief");
					this.OnBriefChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Distribute")]
	public partial class Distribute : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private long _AreaID;
		
		private long _ItemID;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnAreaIDChanging(long value);
    partial void OnAreaIDChanged();
    partial void OnItemIDChanging(long value);
    partial void OnItemIDChanged();
    #endregion
		
		public Distribute()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_AreaID", DbType="BigInt NOT NULL")]
		public long AreaID
		{
			get
			{
				return this._AreaID;
			}
			set
			{
				if ((this._AreaID != value))
				{
					this.OnAreaIDChanging(value);
					this.SendPropertyChanging();
					this._AreaID = value;
					this.SendPropertyChanged("AreaID");
					this.OnAreaIDChanged();
				}
			}
		}
		
		[Column(Storage="_ItemID", DbType="BigInt NOT NULL")]
		public long ItemID
		{
			get
			{
				return this._ItemID;
			}
			set
			{
				if ((this._ItemID != value))
				{
					this.OnItemIDChanging(value);
					this.SendPropertyChanging();
					this._ItemID = value;
					this.SendPropertyChanged("ItemID");
					this.OnItemIDChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.FileItem")]
	public partial class FileItem : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    #endregion
		
		public FileItem()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.ItemLink")]
	public partial class ItemLink : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private long _SrcLink;
		
		private long _InterLink;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnSrcLinkChanging(long value);
    partial void OnSrcLinkChanged();
    partial void OnInterLinkChanging(long value);
    partial void OnInterLinkChanged();
    #endregion
		
		public ItemLink()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_SrcLink", DbType="BigInt NOT NULL")]
		public long SrcLink
		{
			get
			{
				return this._SrcLink;
			}
			set
			{
				if ((this._SrcLink != value))
				{
					this.OnSrcLinkChanging(value);
					this.SendPropertyChanging();
					this._SrcLink = value;
					this.SendPropertyChanged("SrcLink");
					this.OnSrcLinkChanged();
				}
			}
		}
		
		[Column(Storage="_InterLink", DbType="BigInt NOT NULL")]
		public long InterLink
		{
			get
			{
				return this._InterLink;
			}
			set
			{
				if ((this._InterLink != value))
				{
					this.OnInterLinkChanging(value);
					this.SendPropertyChanging();
					this._InterLink = value;
					this.SendPropertyChanged("InterLink");
					this.OnInterLinkChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.PublishType")]
	public partial class PublishType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private long _Category;
		
		private string _Name;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnCategoryChanging(long value);
    partial void OnCategoryChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public PublishType()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_Category", DbType="BigInt NOT NULL")]
		public long Category
		{
			get
			{
				return this._Category;
			}
			set
			{
				if ((this._Category != value))
				{
					this.OnCategoryChanging(value);
					this.SendPropertyChanging();
					this._Category = value;
					this.SendPropertyChanged("Category");
					this.OnCategoryChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="VarChar(32) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.FileSetLink")]
	public partial class FileSetLink : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _IP;
		
		private long _FileSetID;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnIPChanging(string value);
    partial void OnIPChanged();
    partial void OnFileSetIDChanging(long value);
    partial void OnFileSetIDChanged();
    #endregion
		
		public FileSetLink()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_IP", DbType="VarChar(15) NOT NULL", CanBeNull=false)]
		public string IP
		{
			get
			{
				return this._IP;
			}
			set
			{
				if ((this._IP != value))
				{
					this.OnIPChanging(value);
					this.SendPropertyChanging();
					this._IP = value;
					this.SendPropertyChanged("IP");
					this.OnIPChanged();
				}
			}
		}
		
		[Column(Storage="_FileSetID", DbType="BigInt NOT NULL")]
		public long FileSetID
		{
			get
			{
				return this._FileSetID;
			}
			set
			{
				if ((this._FileSetID != value))
				{
					this.OnFileSetIDChanging(value);
					this.SendPropertyChanging();
					this._FileSetID = value;
					this.SendPropertyChanged("FileSetID");
					this.OnFileSetIDChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Cartoon")]
	public partial class Cartoon : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _Author;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnAuthorChanging(string value);
    partial void OnAuthorChanged();
    #endregion
		
		public Cartoon()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_Author", DbType="VarChar(255) NOT NULL", CanBeNull=false)]
		public string Author
		{
			get
			{
				return this._Author;
			}
			set
			{
				if ((this._Author != value))
				{
					this.OnAuthorChanging(value);
					this.SendPropertyChanging();
					this._Author = value;
					this.SendPropertyChanged("Author");
					this.OnAuthorChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Movie")]
	public partial class Movie : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _Player;
		
		private string _Director;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnPlayerChanging(string value);
    partial void OnPlayerChanged();
    partial void OnDirectorChanging(string value);
    partial void OnDirectorChanged();
    #endregion
		
		public Movie()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_Player", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string Player
		{
			get
			{
				return this._Player;
			}
			set
			{
				if ((this._Player != value))
				{
					this.OnPlayerChanging(value);
					this.SendPropertyChanging();
					this._Player = value;
					this.SendPropertyChanged("Player");
					this.OnPlayerChanged();
				}
			}
		}
		
		[Column(Storage="_Director", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string Director
		{
			get
			{
				return this._Director;
			}
			set
			{
				if ((this._Director != value))
				{
					this.OnDirectorChanging(value);
					this.SendPropertyChanging();
					this._Director = value;
					this.SendPropertyChanged("Director");
					this.OnDirectorChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Software")]
	public partial class Software : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _Manufacturer;
		
		private string _Version;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnManufacturerChanging(string value);
    partial void OnManufacturerChanged();
    partial void OnVersionChanging(string value);
    partial void OnVersionChanged();
    #endregion
		
		public Software()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_Manufacturer", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string Manufacturer
		{
			get
			{
				return this._Manufacturer;
			}
			set
			{
				if ((this._Manufacturer != value))
				{
					this.OnManufacturerChanging(value);
					this.SendPropertyChanging();
					this._Manufacturer = value;
					this.SendPropertyChanged("Manufacturer");
					this.OnManufacturerChanged();
				}
			}
		}
		
		[Column(Storage="_Version", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				if ((this._Version != value))
				{
					this.OnVersionChanging(value);
					this.SendPropertyChanging();
					this._Version = value;
					this.SendPropertyChanged("Version");
					this.OnVersionChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Music")]
	public partial class Music : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _Singer;
		
		private string _Author;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnSingerChanging(string value);
    partial void OnSingerChanged();
    partial void OnAuthorChanging(string value);
    partial void OnAuthorChanged();
    #endregion
		
		public Music()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_Singer", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string Singer
		{
			get
			{
				return this._Singer;
			}
			set
			{
				if ((this._Singer != value))
				{
					this.OnSingerChanging(value);
					this.SendPropertyChanging();
					this._Singer = value;
					this.SendPropertyChanged("Singer");
					this.OnSingerChanged();
				}
			}
		}
		
		[Column(Storage="_Author", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string Author
		{
			get
			{
				return this._Author;
			}
			set
			{
				if ((this._Author != value))
				{
					this.OnAuthorChanging(value);
					this.SendPropertyChanging();
					this._Author = value;
					this.SendPropertyChanged("Author");
					this.OnAuthorChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Duration")]
	public partial class Duration : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private System.DateTime _BeginTime;
		
		private System.DateTime _EndTime;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnBeginTimeChanging(System.DateTime value);
    partial void OnBeginTimeChanged();
    partial void OnEndTimeChanging(System.DateTime value);
    partial void OnEndTimeChanged();
    #endregion
		
		public Duration()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_BeginTime", DbType="DateTime NOT NULL")]
		public System.DateTime BeginTime
		{
			get
			{
				return this._BeginTime;
			}
			set
			{
				if ((this._BeginTime != value))
				{
					this.OnBeginTimeChanging(value);
					this.SendPropertyChanging();
					this._BeginTime = value;
					this.SendPropertyChanged("BeginTime");
					this.OnBeginTimeChanged();
				}
			}
		}
		
		[Column(Storage="_EndTime", DbType="DateTime NOT NULL")]
		public System.DateTime EndTime
		{
			get
			{
				return this._EndTime;
			}
			set
			{
				if ((this._EndTime != value))
				{
					this.OnEndTimeChanging(value);
					this.SendPropertyChanging();
					this._EndTime = value;
					this.SendPropertyChanged("EndTime");
					this.OnEndTimeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
