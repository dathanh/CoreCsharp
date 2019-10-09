
/****** Object:  Table [dbo].[DocumentType]    Script Date: 6/12/2019 1:54:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Title] [nvarchar](1000) NOT NULL,
	[Order] [int] NOT NULL,
	[CreatedById] [int] NOT NULL,
	[LastUserId] [int] NOT NULL,
	[LastTime] [datetime] NOT NULL CONSTRAINT [DF__DocumentType__LastTime]  DEFAULT (getdate()),
	[CreatedOn] [datetime] NOT NULL,
	[LastModified] [timestamp] NOT NULL,
	[Type] [nvarchar](50) NULL,
 CONSTRAINT [PK_DocumentType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GridConfig]    Script Date: 6/12/2019 1:54:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GridConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentTypeId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[XmlText] [xml] NOT NULL,
	[GridInternalName] [nvarchar](255) NOT NULL,
	[CreatedById] [int] NOT NULL,
	[LastUserId] [int] NOT NULL,
	[LastTime] [datetime] NOT NULL CONSTRAINT [DF__GridConfig__LastTime__3A81B327]  DEFAULT (getdate()),
	[CreatedOn] [datetime] NOT NULL,
	[LastModified] [timestamp] NOT NULL,
 CONSTRAINT [PK_GridConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SecurityOperation]    Script Date: 6/12/2019 1:54:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SecurityOperation](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[CreatedById] [int] NOT NULL,
	[LastUserId] [int] NOT NULL,
	[LastTime] [datetime] NOT NULL CONSTRAINT [DF__SecurityO__LastT__2F10007B]  DEFAULT (getdate()),
	[CreatedOn] [datetime] NOT NULL,
	[LastModified] [timestamp] NOT NULL,
 CONSTRAINT [PK_SecurityOperation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 6/12/2019 1:54:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[UserRoleId] [int] NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__User__IsActive__02C769E9]  DEFAULT ((0)),
	[FullName] [nvarchar](200) NOT NULL,
	[Phone] [varchar](20) NULL,
	[Email] [varchar](100) NOT NULL,
	[Avatar] [image] NULL,
	[CreatedById] [int] NOT NULL,
	[LastUserId] [int] NOT NULL,
	[LastTime] [datetime] NOT NULL CONSTRAINT [DF_User_LastTime]  DEFAULT (getdate()),
	[CreatedOn] [datetime] NOT NULL,
	[LastModified] [timestamp] NOT NULL,
	[IsSystemUser] [bit] NULL,
	[EmailPassword] [nvarchar](200) NULL,
	[IsAccountFacebook] [bit] NULL,
	[IsAccountGoogle] [bit] NULL,
	[PassportImageId] [nvarchar](300) NULL,
	[Passport] [nvarchar](1000) NULL,
	[StatusId] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 6/12/2019 1:54:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreatedById] [int] NOT NULL,
	[LastUserId] [int] NOT NULL,
	[LastTime] [datetime] NOT NULL CONSTRAINT [DF__UserRole__LastTi__267ABA7A]  DEFAULT (getdate()),
	[CreatedOn] [datetime] NOT NULL,
	[LastModified] [timestamp] NOT NULL,
	[AppRoleName] [nvarchar](50) NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRoleFunction]    Script Date: 6/12/2019 1:54:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleFunction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserRoleId] [int] NOT NULL,
	[SecurityOperationId] [int] NOT NULL,
	[DocumentTypeId] [int] NOT NULL,
	[CreatedById] [int] NOT NULL,
	[LastUserId] [int] NOT NULL,
	[LastTime] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModified] [timestamp] NOT NULL,
 CONSTRAINT [PK_UserRoleFunction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (1, N'User', N'Người dùng', 1, 1, 1, CAST(N'2015-04-01 00:00:00.000' AS DateTime), CAST(N'2015-04-01 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (2, N'UserRole', N'Phân quyền', 2, 1, 1, CAST(N'2015-04-01 00:00:00.000' AS DateTime), CAST(N'2015-04-01 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (3, N'Config', N'Cấu hình', 3, 1, 1, CAST(N'2015-04-01 00:00:00.000' AS DateTime), CAST(N'2015-04-01 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (4, N'Host', N'Chủ kho', 4, 1, 1, CAST(N'2019-01-01 00:00:00.000' AS DateTime), CAST(N'2019-01-01 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (5, N'ApproveHost', N'Approve chủ kho', 5, 1, 1, CAST(N'2019-01-01 00:00:00.000' AS DateTime), CAST(N'2019-01-01 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (6, N'WareHouse', N'Kho', 6, 1, 1, CAST(N'2019-01-01 00:00:00.000' AS DateTime), CAST(N'2019-01-01 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (7, N'City', N'Thành phố', 7, 1, 1, CAST(N'2019-01-01 00:00:00.000' AS DateTime), CAST(N'2019-01-01 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (8, N'District', N'Quận huyện', 8, 1, 1, CAST(N'2019-01-01 00:00:00.000' AS DateTime), CAST(N'2019-01-01 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (9, N'Booking', N'Booking', 9, 1, 1, CAST(N'2019-01-01 00:00:00.000' AS DateTime), CAST(N'2019-01-01 00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentType] ([Id], [Name], [Title], [Order], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [Type]) VALUES (10, N'ApproveWareHouse', N'Approve Kho', 6, 1, 1, CAST(N'2019-01-01 00:00:00.000' AS DateTime), CAST(N'2019-01-01 00:00:00.000' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[GridConfig] ON 

GO
INSERT [dbo].[GridConfig] ([Id], [DocumentTypeId], [UserId], [XmlText], [GridInternalName], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (1, 6, 1, N'<ArrayOfViewColumnViewModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><ViewColumnViewModel><Text /><Name>Command</Name><ColumnWidth>125</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>0</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Tên kho</Text><Name>Title</Name><ColumnWidth>234</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>1</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Địa chỉ</Text><Name>Address</Name><ColumnWidth>327</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>2</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Cập nhật lần cuối</Text><Name>ModifiedDateStr</Name><ColumnWidth>134</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>3</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Giá mỗi đêm</Text><Name>PriceInNightStr</Name><ColumnWidth>146</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>4</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Chủ kho</Text><Name>HostName</Name><ColumnWidth>90</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>5</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Tình trạng</Text><Name>Status</Name><ColumnWidth>100</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>6</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel></ArrayOfViewColumnViewModel>', N'WareHouse', 1, 1, CAST(N'2019-06-05 03:54:51.190' AS DateTime), CAST(N'2019-06-05 03:44:08.757' AS DateTime))
GO
INSERT [dbo].[GridConfig] ([Id], [DocumentTypeId], [UserId], [XmlText], [GridInternalName], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2, 4, 1, N'<ArrayOfViewColumnViewModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><ViewColumnViewModel><Text /><Name>Command</Name><ColumnWidth>150</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>0</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Chủ kho</Text><Name>FullName</Name><ColumnWidth>196</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>1</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Email</Text><Name>Email</Name><ColumnWidth>200</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>2</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Điện thoại</Text><Name>PhoneInFormat</Name><ColumnWidth>125</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>3</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Trạng thái</Text><Name>Status</Name><ColumnWidth>188</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>4</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel><ViewColumnViewModel><Text>Số lượng kho</Text><Name>TotalWareHouse</Name><ColumnWidth>100</ColumnWidth><ColumnJustification>Left</ColumnJustification><UtcFormat xsi:nil="true" /><HideColumn>false</HideColumn><ColumnOrder>5</ColumnOrder><Mandatory>false</Mandatory><Sortable>true</Sortable><Filterable>false</Filterable></ViewColumnViewModel></ArrayOfViewColumnViewModel>', N'Host', 1, 1, CAST(N'2019-06-05 03:54:05.807' AS DateTime), CAST(N'2019-06-05 03:44:18.743' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[GridConfig] OFF
GO
INSERT [dbo].[SecurityOperation] ([Id], [Name], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (1, N'AllowView', 1, 1, CAST(N'2014-11-05 08:30:00.000' AS DateTime), CAST(N'2014-11-05 08:30:00.000' AS DateTime))
GO
INSERT [dbo].[SecurityOperation] ([Id], [Name], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2, N'AllowProcess', 1, 1, CAST(N'2014-11-05 08:30:00.000' AS DateTime), CAST(N'2014-11-05 08:30:00.000' AS DateTime))
GO
INSERT [dbo].[SecurityOperation] ([Id], [Name], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (3, N'AllowAdd', 1, 1, CAST(N'2014-11-05 08:30:00.000' AS DateTime), CAST(N'2014-11-05 08:30:00.000' AS DateTime))
GO
INSERT [dbo].[SecurityOperation] ([Id], [Name], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (4, N'AllowDelete', 1, 1, CAST(N'2014-11-05 08:30:00.000' AS DateTime), CAST(N'2014-11-05 08:30:00.000' AS DateTime))
GO
INSERT [dbo].[SecurityOperation] ([Id], [Name], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (5, N'AllowUpdate', 1, 1, CAST(N'2014-11-05 08:30:00.000' AS DateTime), CAST(N'2014-11-05 08:30:00.000' AS DateTime))
GO
INSERT [dbo].[SecurityOperation] ([Id], [Name], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (6, N'ShowMenu', 1, 1, CAST(N'2014-01-01 00:00:00.000' AS DateTime), CAST(N'2014-01-01 00:00:00.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([Id], [UserName], [Password], [UserRoleId], [IsActive], [FullName], [Phone], [Email], [Avatar], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [IsSystemUser], [EmailPassword], [IsAccountFacebook], [IsAccountGoogle], [PassportImageId], [Passport], [StatusId]) VALUES (1, N'cuongnguyen', N'2F9B2DDC-BDE81EF5-8B6A6D9E-513281DD-50CD4E74-DE92B3D7-18E284C3-5B7E1DCE', 1, 1, N'Cuong Nguyen Phan Ky', N'09096655111', N'danitthehemoi1@gmail.com', NULL, 1, 1, CAST(N'2018-03-17 11:11:30.370' AS DateTime), CAST(N'2016-08-08 00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([Id], [UserName], [Password], [UserRoleId], [IsActive], [FullName], [Phone], [Email], [Avatar], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [IsSystemUser], [EmailPassword], [IsAccountFacebook], [IsAccountGoogle], [PassportImageId], [Passport], [StatusId]) VALUES (2, N'admin', N'37FCAC28-6D26F211-3BC6A9AB-BF24B388-EB4F4FFC-D39FD316-08A89E4B-AA8199BA', 1, 1, N'Admin1', N'0909999888', N'admin@abc.com', NULL, 1, 1, CAST(N'2019-06-12 06:53:09.850' AS DateTime), CAST(N'2019-01-11 04:24:24.887' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRole] ON 

GO
INSERT [dbo].[UserRole] ([Id], [Name], [CreatedById], [LastUserId], [LastTime], [CreatedOn], [AppRoleName]) VALUES (1, N'Admin', 1, 1, CAST(N'2019-05-06 08:19:11.913' AS DateTime), CAST(N'2016-08-08 00:00:00.000' AS DateTime), N'GlobalAdmin')
GO
SET IDENTITY_INSERT [dbo].[UserRole] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoleFunction] ON 

GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2066, 1, 3, 6, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2067, 1, 4, 6, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2068, 1, 5, 6, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2069, 1, 6, 6, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2070, 1, 1, 7, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2071, 1, 3, 7, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2072, 1, 4, 7, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2073, 1, 5, 7, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2074, 1, 6, 7, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2075, 1, 1, 8, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2076, 1, 3, 8, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2077, 1, 4, 8, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2078, 1, 5, 8, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2079, 1, 6, 8, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2080, 1, 1, 9, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2081, 1, 3, 9, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2082, 1, 4, 9, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2083, 1, 5, 9, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2084, 1, 6, 9, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2085, 1, 1, 10, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2086, 1, 3, 10, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2087, 1, 4, 10, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2088, 1, 1, 6, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2089, 1, 6, 5, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2090, 1, 6, 2, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2091, 1, 4, 5, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2092, 1, 1, 1, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.917' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2093, 1, 3, 1, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2094, 1, 4, 1, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2095, 1, 5, 1, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2096, 1, 6, 1, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2097, 1, 1, 2, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2098, 1, 3, 2, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2099, 1, 4, 2, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2100, 1, 5, 2, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2101, 1, 5, 10, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2102, 1, 5, 5, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2103, 1, 1, 3, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2104, 1, 4, 3, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2105, 1, 5, 3, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2106, 1, 6, 3, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2107, 1, 1, 4, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2108, 1, 3, 4, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2109, 1, 4, 4, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2110, 1, 5, 4, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2111, 1, 6, 4, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2112, 1, 1, 5, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2113, 1, 3, 5, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2114, 1, 3, 3, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
INSERT [dbo].[UserRoleFunction] ([Id], [UserRoleId], [SecurityOperationId], [DocumentTypeId], [CreatedById], [LastUserId], [LastTime], [CreatedOn]) VALUES (2115, 1, 6, 10, 1, 1, CAST(N'2019-05-06 08:19:11.920' AS DateTime), CAST(N'2019-05-06 08:19:11.920' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[UserRoleFunction] OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_GridConfig_FunctionId_UserId]    Script Date: 6/12/2019 1:54:01 PM ******/
ALTER TABLE [dbo].[GridConfig] ADD  CONSTRAINT [UK_GridConfig_FunctionId_UserId] UNIQUE NONCLUSTERED 
(
	[DocumentTypeId] ASC,
	[GridInternalName] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_SecurityOperation_Name]    Script Date: 6/12/2019 1:54:01 PM ******/
ALTER TABLE [dbo].[SecurityOperation] ADD  CONSTRAINT [UK_SecurityOperation_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_UserRole_Name]    Script Date: 6/12/2019 1:54:01 PM ******/
ALTER TABLE [dbo].[UserRole] ADD  CONSTRAINT [UK_UserRole_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GridConfig]  WITH CHECK ADD  CONSTRAINT [FK_GridConfig_DocumentType] FOREIGN KEY([DocumentTypeId])
REFERENCES [dbo].[DocumentType] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GridConfig] CHECK CONSTRAINT [FK_GridConfig_DocumentType]
GO
ALTER TABLE [dbo].[GridConfig]  WITH CHECK ADD  CONSTRAINT [FK_GridConfig_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GridConfig] CHECK CONSTRAINT [FK_GridConfig_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_UserRole] FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRole] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_UserRole]
GO
ALTER TABLE [dbo].[UserRoleFunction]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleFunction_DocumentType] FOREIGN KEY([DocumentTypeId])
REFERENCES [dbo].[DocumentType] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoleFunction] CHECK CONSTRAINT [FK_UserRoleFunction_DocumentType]
GO
ALTER TABLE [dbo].[UserRoleFunction]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleFunction_SecurityOperation] FOREIGN KEY([SecurityOperationId])
REFERENCES [dbo].[SecurityOperation] ([Id])
GO
ALTER TABLE [dbo].[UserRoleFunction] CHECK CONSTRAINT [FK_UserRoleFunction_SecurityOperation]
GO
ALTER TABLE [dbo].[UserRoleFunction]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleFunction_UserRole] FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRole] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoleFunction] CHECK CONSTRAINT [FK_UserRoleFunction_UserRole]
GO
/****** =============================================================================================================================== ******/
