# DNS-Site
## Краткое описание проекта:
SPA-приложение, использующее в качестве сервера технологию **ASP.NET**, а в качестве фреймворка (библиотеки) для работы клиентской части - **React** совместно с **Redux**-хранилищем.
## Требуемое программное обеспечение:
* Visual Studio 2019
* .NET Core 3.1
* Microsoft SQL Server 2019
* Microsoft SQL Server Management Studio 18

При установке **Microsoft SQL Server 2019** создайте сервер базы данных. Затем запустите **Microsoft SQL Server Management Studio 18** и подключитесь к серверу по названию, которое вы указали при установке **SQL Server**'а в формате **имя_компьютера\название_сервера**. Название вашего компьютера в **Windows 7/8/10** можно узнать, нажав ПКМ на **Компьютер -> Свойства -> Подробно -> Владелец**, до первого знака **\\**.

Откройте файл **DNS-Site.sln** в корне проекта с помощью **Visual Studio 2019** и скомпилируйте в **Release** режиме. При первой сборке вам будет предлложено автоматически создать сертификат для работы в режиме https (шифрования данных, передаваемых между сервером и клиентом).

Откройте консоль и перейдите в корень проекта, после чего запустите проект командой "**dotnet run DNS-Site.csproj**". При этом убедитесь, что служба, отвечающая за работу **SQL Server**'а, запущена, а порты 5000 и 5001 не заняты другими программами. Если данные порты недоступны, их можно сменить на другие в файле настроек проекта "**Properties/launchSettings.json"**, значение **"profiles" -> "DNS_Site" -> "applicationUrl"**.

Откройте браузер и перейдите по адресу, указанному адресу в файле настроек проекта.
