create database [DNS-Site];

use [DNS-Site];

create table [Employees](
	[Id] INT IDENTITY(1,1) NOT NULL,
		CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
	[Name] nvarchar(50) NOT NULL,
	[Surname] nvarchar(50) NOT NULL,
	[Patronymic] nvarchar(50) NULL,
	[Department] nvarchar(50) NOT NULL,
	[Position] nvarchar(50) NOT NULL,
	[Supervisor] int NULL,
		CONSTRAINT [FK_Employees_Supervisor] FOREIGN KEY ([Supervisor])
			REFERENCES [Employees]([Id]),
	[JobStartDate] datetime NOT NULL DEFAULT GETDATE()
);

create table [EmployeesDeleted](
	[Id] INT IDENTITY(1,1) NOT NULL,
		CONSTRAINT [PK_EmployeesDeleted] PRIMARY KEY ([Id]),
	[Name] nvarchar(50) NOT NULL,
	[Surname] nvarchar(50) NOT NULL,
	[Patronymic] nvarchar(50) NULL,
	[Department] nvarchar(50) NOT NULL,
	[Position] nvarchar(50) NOT NULL,
	[Supervisor] int NULL,
		CONSTRAINT [FK_EmployeesDeleted_Supervisor] FOREIGN KEY ([Supervisor])
			REFERENCES [Employees]([Id]),
	[JobStartDate] datetime NOT NULL DEFAULT GETDATE()
);