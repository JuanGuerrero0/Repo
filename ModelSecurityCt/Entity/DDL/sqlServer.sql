CREATE TABLE [Person]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[FirstName] VARCHAR(100) NOT NULL,
	[LastName] VARCHAR(100) NOT NULL,
	[DocumentType] VARCHAR(100) NOT NULL,
	[Document] VARCHAR(100) NOT NULL,
	[DateBorn] VARCHAR(100) NOT NULL,
	[PhoneNumber] VARCHAR(20) NOT NULL,
	[Eps] VARCHAR(20) NOT NULL,
	[Genero] VARCHAR(20) NOT NULL,
	[RelatedPerson] BIT,
	[IsDeleted] BIT DEFAULT 0
);

CREATE TABLE [User]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Email] VARCHAR(100) NOT NULL,
	[Password] VARCHAR(100) NOT NULL,
	[Active] BIT NOT NULL,
	[IsDeleted] BIT DEFAULT 0,
	[RegistrationDate] DATETIME,
	[PersonId] INT UNIQUE NOT NULL
);

CREATE TABLE [RolUser]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[RolId] INT NOT NULL,
	[UserId] INT NOT NULL,
	[IsDeleted] BIT NOT NULL
);

CREATE TABLE [Role]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(100) NOT NULL,
	[Description] VARCHAR(100) NOT NULL,
	[IsDeleted] BIT DEFAULT 0
);

CREATE TABLE [RolFormPermission]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[RolId] INT NOT NULL,
	[FormId] INT NOT NULL,
	[PermissionId] INT NOT NULL,
	[IsDeleted] BIT NOT NULL
);

CREATE TABLE [Permission]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(100) NOT NULL,
	[Description] TEXT NOT NULL,
	[IsDeleted] BIT DEFAULT 0
);

CREATE TABLE [Form]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(100) NOT NULL,
	[Description] TEXT NOT NULL,
	[Url] VARCHAR(100) NOT NULL,
	[IsDeleted] BIT DEFAULT 0
);

CREATE TABLE [FormModule]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[ModuleId] INT NOT NULL,
	[FormId] INT NOT NULL,
	[IsDeleted] BIT DEFAULT 0
);

CREATE TABLE [Module]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(100) NOT NULL,
	[Description] TEXT NOT NULL,
	[IsDeleted] BIT NOT NULL
);

-- Relaciones

ALTER TABLE [User] ADD CONSTRAINT [FK_User_Person] FOREIGN KEY ([PersonId]) REFERENCES [Person]([Id]);

ALTER TABLE [RolUser] ADD CONSTRAINT [FK_RolUser_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]);

ALTER TABLE [RolUser] ADD CONSTRAINT [FK_RolUser_Rol] FOREIGN KEY ([RolId]) REFERENCES [Role]([Id]);

ALTER TABLE [FormModule] ADD CONSTRAINT [FK_FormModule_Module] FOREIGN KEY ([ModuleId]) REFERENCES [Module]([Id]);
ALTER TABLE [FormModule] ADD CONSTRAINT [FK_FormModule_Form] FOREIGN KEY ([FormId]) REFERENCES [Form]([Id]);

ALTER TABLE [RolFormPermission] ADD CONSTRAINT [FK_RolFormPermission_Rol] FOREIGN KEY ([RolId]) REFERENCES [Role]([Id]);
ALTER TABLE [RolFormPermission] ADD CONSTRAINT [FK_RolFormPermission_Form] FOREIGN KEY ([FormId]) REFERENCES [Form]([Id]);
ALTER TABLE [RolFormPermission] ADD CONSTRAINT [FK_RolFormPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id]);
