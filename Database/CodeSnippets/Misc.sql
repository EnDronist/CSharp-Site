--insert into [Employees](
--	[Name], [Surname], [Patronymic], [Department], [Position], [Supervisor]
--)
--values(
--	'John', 'Goldrim', 'Alex', 'Office', 'Manager', 9
--);

--select convert(bit, case when [Id] = 2 then 1 else 0 end) from [Employees]
--	where [Id] = 2;

--select [Id] from [Employees] where [Id] = 1;

--begin transaction;

--insert into [EmployeesDeleted](
--	[Name], [Surname], [Patronymic], [Department], [Position], [Supervisor], [JobStartDate]
--)
--select [Name], [Surname], [Patronymic], [Department], [Position], [Supervisor], [JobStartDate]
--	from [Employees] where [Id] = 17;

--delete from [Employees] where [Id] = 15;

--insert into [Employees](
--	[Name], [Surname], [Patronymic], [Department], [Position], [Supervisor], [JobStartDate]
--)
--select [Name], [Surname], [Patronymic], [Department], [Position], [Supervisor], [JobStartDate]
--	from [EmployeesDeleted] where [Id] = 14;

--delete from [EmployeesDeleted] where [Id] = 14;

--commit;

--alter table [Employees]
--	drop constraint [FK_Employees_Supervisor];

--alter table [Employees]
--	add constraint [FK_Employees_Supervisor] FOREIGN KEY ([Supervisor])
--		REFERENCES [Employees]([Id]);

--select * from [Employees]
--	order by [Id] offset 0 rows fetch next 5 rows only;
--select * from [EmployeesDeleted];

--select [Name] from [Employees] where [Id] in (
--	select top (1) [Supervisor] from [Employees] where [Id] = 4
--);

--exec sp_columns [Employees];